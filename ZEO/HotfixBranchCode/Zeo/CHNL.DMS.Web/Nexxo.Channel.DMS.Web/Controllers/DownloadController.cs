using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using System.IO;
//TODO Merge using System.Web.Http;

namespace MGI.Channel.DMS.Web.Controllers
{
    [Authorize(Roles = "Manager, SystemAdmin, Tech")]
    public class DownloadController : BaseController
    {
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
        public ActionResult ShowDownloaderOption()
        {
            DownloaderModel downloader = new DownloaderModel();
            downloader.Documents = new List<string>();
            downloader.Installers = new List<string>();
            downloader.Upgrades = new List<string>();

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\Common\PSInstaller"));

            FileInfo[] docs = directory.GetFiles("*.docx");
            foreach (FileInfo fileInfo in docs)
            {
                downloader.Documents.Add(fileInfo.Name);
            }

            FileInfo[] files = directory.GetFiles("*.msi");
            foreach (FileInfo fileInfo in files)
            {
                string fileName = fileInfo.Name;
                if (fileName.ToLower().Contains("installer"))
                    downloader.Installers.Add(fileName);
                if (fileName.ToLower().Contains("update"))
                    downloader.Upgrades.Add(fileName);
            }

            FileInfo[] zipFiles = directory.GetFiles("*.zip");
            foreach (FileInfo fileInfo in zipFiles)
            {
                string fileName = fileInfo.Name;
                if (fileName.ToLower().Contains("installer"))
                    downloader.Installers.Add(fileName);
                if (fileName.ToLower().Contains("update"))
                    downloader.Upgrades.Add(fileName);
            }

            downloader.Upgrades = SortListDesc(downloader.Upgrades);
            downloader.Documents = SortListDesc(downloader.Documents);
            downloader.Installers = SortListDesc(downloader.Installers);
            return View("Downloader", downloader);
        }

        private List<string> SortListDesc(List<string> list)
        {
            return list.OrderByDescending(c => c).ToList();
        }
    }
}
