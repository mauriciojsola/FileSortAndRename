using System;
using System.IO;
using System.Web.Mvc;

namespace FileSortAndRename.Controllers
{
    public class FileUploadController : BaseController
    {
        //
        // GET: /FilesUpload/

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult UploadSizeReducedFiles()
        {
            string fileName = null;
            string uploadedFilePath = null;

            try
            {
                fileName = Request.Headers["X-File-Name"];

                var directoryPath = Server.MapPath("~/Uploads/");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var stream = new StreamReader(Request.InputStream);
                var image = stream.ReadToEnd();

                image = image.Substring("image=data:image/jpeg;base64,".Length);
                var buffer = Convert.FromBase64String(image);

                uploadedFilePath = string.Format("{0}{1}", directoryPath, fileName);

                System.IO.File.WriteAllBytes(uploadedFilePath, buffer);

                return Json(fileName + " completed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                if (System.IO.File.Exists(uploadedFilePath))
                {
                    if (uploadedFilePath != null) System.IO.File.Delete(uploadedFilePath);
                }

                Response.StatusCode = 500;
                return Json(fileName + " fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
