using System.Collections.Generic;
using System.IO;
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

    }
}
