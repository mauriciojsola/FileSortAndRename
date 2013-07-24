
function FileListController($scope, $http) {

    $scope.loading = true;
    $scope.success = false;
    
    $http.get("/FilesSort/GetFilesList").success(function (data) {
        $scope.fileList = data;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading the images!";
        $scope.loading = false;
    });

    $scope.save = function () {
        // Get the sortable list and build up the data from it
        var list = $('#sortable').sortable("toArray").join("|");
        var data = { 'filesList': list };
        $http.post('/FilesSort/SaveFiles/', data).success(function (data) {
            $scope.fileList = data;
            $scope.loading = false;
            $scope.success = true;

        }).error(function (data) {
            $scope.error = "An Error has occured while saving the files! " + data;
            $scope.loading = false;
        });
    };

}