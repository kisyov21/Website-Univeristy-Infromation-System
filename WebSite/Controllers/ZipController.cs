
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Upload;
using GoogleApi1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using File = Google.Apis.Drive.v2.Data.File;

namespace WebSite.Controllers
{
    public class ZipController : Controller
    {
        private static ZipResult zipRes;

        [HttpGet]
        public ActionResult Index()
        {
            if (zipRes == null)
            {
                zipRes = new ZipResult
                {
                    Status = true,
                    Message = string.Empty,
                    ZipPath = @"E:\Misc\Uploads\AgentSystem.zip",
                    SourceDirName = @"Uploads",
                    GoogleDrive_Title = null,
                    GoogleDrive_Id = null,
                    GoogleDrive_DownloadUrl = null,
                    GoogleDrive_FileSize = null
                };
            }

            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            var result = UploadZipToGoogleDrive(zipRes);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Download()
        {
            var result = DownloadZipFromGoogleDrive(zipRes);
            return Json("result", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            string result = DeleteZipFromGoogleDrive(zipRes);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string UploadZipToGoogleDrive(ZipResult zipRes)
        {
            try
            {
                string contentType = "application/zip, application/octet-stream";

               
                var service = GoogleDrive.NewService();
                var title = zipRes.ZipPath.Split('\\').Last();
                var parent = new ParentReference { Id = "0B6g-9fVqqIureUZZQVhiTzU3UzQ" };
                var parents = new List<ParentReference> { parent };

                var body = new File
                {
                    Title = title,
                    MimeType = contentType,
                    Parents = parents
                };

                using (var stream = new FileStream(zipRes.ZipPath, FileMode.Open))
                {
                    var bytesTotal = stream.Length;
                    int chunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 5; // ~ 1.25 MB

                    var request = service.Files.Insert(body, stream, contentType);
                    request.ChunkSize = chunkSize;

                    request.ProgressChanged += (sender) => Upload_ProgressChanged(sender, bytesTotal);
                    request.ResponseReceived += (sender) => Upload_ResponseReceived(sender, zipRes);

                    request.Upload();
                }
            }
            catch (Exception exception)
            {
                return "FAILED";
            }

            return "UPLOADED";
        }

        private static void Upload_ProgressChanged(IUploadProgress progress, long bytesTotal)
        {
            var status = progress.Status.ToString().ToUpper();
            long bytesRemaining = bytesTotal - progress.BytesSent;
            Debug.WriteLine(string.Format("{0} - {1} bytes remaining", status, bytesRemaining));
        }

        private static void Upload_ResponseReceived(File file, ZipResult zipRes)
        {
            zipRes.GoogleDrive_Title = file.Title;
            zipRes.GoogleDrive_Id = file.Id;
            zipRes.GoogleDrive_DownloadUrl = file.DownloadUrl;
            zipRes.GoogleDrive_FileSize = file.FileSize;
        }

        private string DownloadZipFromGoogleDrive(ZipResult zipRes)
        {
            string output = string.Empty;

            try
            {
                output = @"E:\Misc\Downloads\" + zipRes.GoogleDrive_Title;

                if (System.IO.File.Exists(output))
                {
                    System.IO.File.Delete(output);
                }

                var service = GoogleDrive.NewService();

                var bytesTotal = zipRes.GoogleDrive_FileSize;
                int chunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 10; // ~ 2.5 MB

                var downloader = new MediaDownloader(service);
                downloader.ChunkSize = chunkSize;

                downloader.ProgressChanged += (sender) => Download_ProgressChanged(sender, bytesTotal);

                using (var stream = new FileStream(output, FileMode.Create, FileAccess.Write))
                {
                    var progress = downloader.Download(zipRes.GoogleDrive_DownloadUrl, stream);

                    if (progress.Status != DownloadStatus.Completed)
                    {
                        return "FAILED";
                    }
                }
            }
            catch (Exception exception)
            {
                return "FAILED";
            }

            return string.Format("SAVED TO {0}", output);
        }

        private void Download_ProgressChanged(IDownloadProgress progress, long? bytesTotal)
        {
            var status = progress.Status.ToString().ToUpper();
            long? bytesRemaining = bytesTotal - progress.BytesDownloaded;
            Debug.WriteLine(string.Format("{0} - {1} bytes remaining", status, bytesRemaining));
        }

        private string DeleteZipFromGoogleDrive(ZipResult zipRes)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(zipRes.GoogleDrive_Id))
                {
                    var service = GoogleDrive.NewService();

                    var request = service.Files.Delete(zipRes.GoogleDrive_Id);
                    var result = request.Execute();

                    result = service.Files.EmptyTrash().Execute();

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        zipRes.GoogleDrive_Title = null;
                        zipRes.GoogleDrive_Id = null;
                        zipRes.GoogleDrive_DownloadUrl = null;
                        zipRes.GoogleDrive_FileSize = null;

                        return "DELETED";
                    }
                }
            }
            catch (Exception exception)
            {
            }

            return "FAILED";
        }

    }
}