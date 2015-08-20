using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FileSortAndRename.Helpers;
using FileSortAndRename.Models;

namespace FileSortAndRename.Controllers
{
    public class FilesSortController : BaseController
    {
        //
        // GET: /FilesSort/

        public ActionResult Index()
        {
            return View(BuildFileList());
        }

        public ActionResult GetFilesList()
        {
            return Json(BuildFileList(), JsonRequestBehavior.AllowGet);
        }

        private IList<FileItem> BuildFileList()
        {
            var fileList = new List<FileItem>();

            var files = Directory.GetFiles(Server.MapPath("~/Uploads"));

            foreach (var file in files.ToList().OrderBy(x => x))
            {
                var fileInfo = new FileInfo(file);

                var fileItem = new FileItem
                {
                    Name = Path.GetFileName(file),
                    Size = fileInfo.Length,
                    Path = ("Uploads/") + Path.GetFileName(file) + "?w=75&h=75&mode=max"
                };

                fileList.Add(fileItem);
            }
            return fileList;
        }

        public ActionResult SaveFiles(string filesList)
        {
            if(string.IsNullOrEmpty(filesList)) return new EmptyResult();
            var orderIndex = 1;
            var files = filesList.Split('|').ToList();

            try
            {
                foreach (var fileName in files)
                {
                    var sourceFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        // Create the target file path
                        var destFilePath = Path.Combine(Server.MapPath("~/Uploads"), string.Format("{0}_{1}", orderIndex.ToString().PadLeft(3, '0'), GetFileNameWithoutOrderIndex(fileName)));

                        if (!sourceFilePath.Equals(destFilePath, StringComparison.OrdinalIgnoreCase))
                        {
                            // Copy the new ordered file
                            System.IO.File.Copy(sourceFilePath, destFilePath);
                            System.IO.File.Delete(sourceFilePath);
                        }
                    }
                    orderIndex++;
                }

            }
            catch (Exception ex)
            {
                //Flash[FlashKey.Error] = ex.Message;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            //Flash[FlashKey.Notice] = "Files saved successfuly.";
            return Json(BuildFileList(), JsonRequestBehavior.AllowGet);
        }

        private string GetFileNameWithoutOrderIndex(string fileName)
        {
            // If a file was previously indexed, then it should be in a format: 001_SomeName.ext.
            // Then, extract only the file name after the underscore char (_)
            return fileName.Substring(3, 1).Equals("_", StringComparison.InvariantCultureIgnoreCase) 
                ? fileName.Substring(4) 
                : fileName;
        }

        private void DeleteOldFiles(string[] oldOrderedFiles, string fileName)
        {
            var oldFiles = oldOrderedFiles.Where(x => x.Contains(fileName)).ToList();

            foreach (var oldFile in oldFiles)
            {
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }
        }
    }
}
