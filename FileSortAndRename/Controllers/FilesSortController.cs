using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FileSortAndRename.Models;

namespace FileSortAndRename.Controllers
{
    public class FilesSortController : Controller
    {
        //
        // GET: /FilesSort/

        public ActionResult Index()
        {
            var fileList = new List<FileItem>();

            var files = Directory.GetFiles(Server.MapPath("~/Uploads"));

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                var fileItem = new FileItem
                                   {
                                       Name = Path.GetFileName(file),
                                       Size = fileInfo.Length,
                                       Path = ("~/Uploads/") + Path.GetFileName(file)
                                   };

                fileList.Add(fileItem);
            }

            return View(fileList);
        }

        public ActionResult SaveFiles(string filesList)
        {
            var orderIndex = 1;
            var files = filesList.Split('|').ToList();

            // Get the files from the OrderedFiles directory
            //var oldOrderedFiles = Directory.GetFiles(Server.MapPath("~/Uploads/OrderedFiles"));

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
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private string GetFileNameWithoutOrderIndex(string fileName)
        {
            // If a file was previously indexed, then it should be in a format: 001_SomeName.ext.
            // Then, extract only the file name after the underscore char (_)
            if (fileName.Substring(3, 1).Equals("_", StringComparison.InvariantCultureIgnoreCase))
            {
                return fileName.Substring(4);
            }
            return fileName;
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
