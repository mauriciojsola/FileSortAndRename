
var filequeue = new Array();

function fileSelected() {

    try {

        if (window.File && window.FileReader && window.FileList && window.Blob) {

            var selectedfile;

            var files = document.getElementById('filesToUpload').files;
            if (files.length > 100) {
                alert('You can only upload 100 files per session');
            } else {

                for (var i = 0; i < files.length; i++) {

                    selectedfile = files[i];

                    if (!selectedfile.type.match('image.*')) {
                        alert('Only image files are allowed');
                    } else if (selectedfile.size > 10485760) {
                        alert('Maximum file size exceeds');
                    }
                    if (selectedfile.type.match('image.*') && selectedfile.size < 10485760) {

                        var reader = new FileReader();

                        reader.onload = (function (theFile) {
                            return function (e) {

                                var li = document.createElement("li");
                                
                                var newDiv = document.createElement('div');

                                var img = document.createElement("img");
                                img.setAttribute("src", e.target.result);
                                img.setAttribute("height", "80");
                                img.setAttribute("width", "80");
                                img.setAttribute("class", "img-polaroid");
                                img.setAttribute("style", "display:block;");

                                newDiv.appendChild(img);

                                var label = document.createElement("label");
                                var labelText = document.createTextNode(theFile.name);
                                label.appendChild(labelText);
                                newDiv.appendChild(label);

                                li.appendChild(newDiv);

                                document.getElementById("uploadfilelist").appendChild(li);

                                filequeue.push({ file: theFile, lisetElement: li });

                            };
                        })(selectedfile);

                        reader.readAsDataURL(selectedfile);

                    }
                }
            } // end (files.length > 500) 

        } else {
            
            alert("The HTML5 File Api is not compatible with this browser!");
        }

        $("#filesToUpload").val("");

    } catch (err) {
        alert("Exception " + err);
    }
}


function uploadFiles() {

    try {

        if (filequeue != null && filequeue.length != 0) {
            while (filequeue.length > 0) {
                var item = filequeue.pop();
                var file = item.file;
                var li = item.lisetElement;
                resizeAndUpload(file, li);
            }
            
        }

    } catch (err) {
        alert("Exception " + err);
    }

}

function resizeAndUpload(file, li) {

    try {

        if (window.File && window.FileReader && window.FileList && window.Blob) {

            var uploadurl = UPLOAD_URL;

            var reader = new FileReader();
            
            reader.onloadend = function (evt) {

                if (evt.target.readyState == FileReader.DONE) {

                    var tempImg = new Image();

                    tempImg.onload = function () {

                        var MAX_WIDTH = 2800;
                        var MAX_HEIGHT = 2800;

                        var tempWidth = tempImg.width;
                        var tempHeight = tempImg.height;

                        if (tempWidth > tempHeight) {
                            if (tempWidth > MAX_WIDTH) {
                                tempHeight *= MAX_WIDTH / tempWidth;
                                tempWidth = MAX_WIDTH;
                            }
                        } else {
                            if (tempHeight > MAX_HEIGHT) {
                                tempWidth *= MAX_HEIGHT / tempHeight;
                                tempHeight = MAX_HEIGHT;
                            }
                        }

                        var canvas = document.createElement('canvas');
                        canvas.setAttribute("height", tempHeight);
                        canvas.setAttribute("width", tempWidth);
                        var context = canvas.getContext("2d");
                        context.drawImage(tempImg, 0, 0, tempWidth, tempHeight);

                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", uploadurl);
                        xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
                        xhr.setRequestHeader("X-File-Name", file.name);
                        var data = 'image=' + canvas.toDataURL("image/jpeg");
                        xhr.send(data);

                        xhr.onreadystatechange = function () {

                            var containtext;

                            if (xhr.readyState != 4) {
                                return;
                            }

                            else if (xhr.readyState == 4) {
                                var label = $(li).find("label");
                                
                                if (xhr.status == 500) {
                                    containtext = label.text();
                                    label.text(containtext + " upload error");
                                    label.css("color", "#FF0000");
                                }
                                else if (xhr.status == 200) {
                                    containtext = label.text();
                                    label.text(containtext + " (complete)");
                                    label.css("color", "#1309eb");
                                    label.css("font-weight", "bold");
                                }

                            }

                        };

                    };
                    tempImg.src = reader.result;

                };

            };
            
            reader.readAsDataURL(file);

        } else {
            alert("html5 file api is not compatible with this browser!");
        }

    }
    catch (err) {
        alert("Exception " + err);
    }
}
