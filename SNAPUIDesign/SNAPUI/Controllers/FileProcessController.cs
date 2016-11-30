using SNAPUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNAPUI.Controllers
{
    public class FileProcessController : Controller
    {
        //
        // GET: /FileProcess/
        DownloadFiles obj;
        public FileProcessController()
        {
            obj = new DownloadFiles();
        }  
        public ActionResult Index()
        {
            //return File(@"C:\Users\ChHassan\Documents\SNAPUserInterfaceDesign\SNAPUIDesign\SNAPUI\bin\MyFiles\JsonTree.Json", System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(@"C:\Users\ChHassan\Documents\SNAPUserInterfaceDesign\SNAPUIDesign\SNAPUI\bin\MyFiles\JsonTree.Json"));
            var filesCollection = obj.GetFiles();
            return View(filesCollection); 
        }

        public FileResult Download(string FileID)
        {
            int CurrentFileID = Convert.ToInt32(FileID);
            var filesCol = obj.GetFiles();
            string CurrentFileName = (from fls in filesCol
                                      where fls.FileId == CurrentFileID
                                      select fls.FilePath).First();

            string contentType = string.Empty;

            if (CurrentFileName.Contains(".json"))
            {
                contentType = "application/json";
            }

            else if (CurrentFileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }
            return File(CurrentFileName, contentType, CurrentFileName);
        }  
	}
}