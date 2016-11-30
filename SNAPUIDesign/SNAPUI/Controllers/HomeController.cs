using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SNAPUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult snap(HttpPostedFileBase file)
        {
            string htmlfile =Request["HtmlFile"].ToString();
            string  cssifle = Request["CssFile"].ToString();
            string xpath =Request["XPath"].ToString();
            HtmlParser.Parser.HtmlDomParser dom = new HtmlParser.Parser.HtmlDomParser(htmlfile, cssifle, xpath);            
            return File(@"C:\Users\ChHassan\Documents\SNAPUserInterfaceDesign\SNAPUIDesign\SNAPUI\bin\MyFiles\JsonTree.Json", System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(@"C:\Users\ChHassan\Documents\SNAPUserInterfaceDesign\SNAPUIDesign\SNAPUI\bin\MyFiles\JsonTree.Json"));
        }

    }
}