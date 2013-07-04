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
            var oldOrderedFiles = Directory.GetFiles(Server.MapPath("~/Uploads/OrderedFiles"));

            try
            {
                foreach (var file in files)
                {
                    var sourceFilePath = Path.Combine(Server.MapPath("~/Uploads"), file);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        // Create the target file path
                        var destFilePath = Path.Combine(Server.MapPath("~/Uploads/OrderedFiles"), string.Format("{0}_{1}", orderIndex.ToString().PadLeft(3, '0'), file));

                        DeleteOldFiles(oldOrderedFiles, file);
                        
                        // Copy the new ordered file
                        System.IO.File.Copy(sourceFilePath, destFilePath);
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

        private void DeleteOldFiles(string[] oldOrderedFiles, string file)
        {
            var oldFiles = oldOrderedFiles.Where(x => x.Contains(file)).ToList();

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
