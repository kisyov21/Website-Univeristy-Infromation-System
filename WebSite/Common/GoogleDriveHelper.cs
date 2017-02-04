using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Upload;
using WebSite.GoogleApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using File = Google.Apis.Drive.v2.Data.File;

namespace WebSite.Common
{
    public class GoogleDriveHelper
    {
        private static GoogleDriveHelper _googleDriveHelper;

        public static GoogleDriveHelper GetGoogleDriveHelper()
        {
            if (_googleDriveHelper == null)
            {
                _googleDriveHelper = new GoogleDriveHelper();
            }
            return _googleDriveHelper;
        }

        public bool UploadToGoogleDrive(ZipResult zResult, out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                string contentType = "application/zip, application/octet-stream";

                var client = new ApiClient();
                var authentification = client.Methods.api.Authentification;
                var service = authentification.ApiService;
                var title = zResult.ZipPath.Split('\\').Last();
                var parent = new ParentReference { Id = authentification.ApiDriveFolder };
                var parents = new List<ParentReference> { parent };

                var fileGD = new File
                {
                    Title = title,
                    MimeType = contentType,
                    Parents = parents
                };

                Debug.WriteLine(string.Format("UploadToGoogleDrive - zipPath = {0}", zResult.ZipPath));

                using (var stream = new FileStream(zResult.ZipPath, FileMode.Open))
                {
                    var bytesTotal = stream.Length;
                    int chunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 5; // ~ 1.25 MB

                    var request = service.Files.Insert(fileGD, stream, contentType);
                    request.ChunkSize = chunkSize;

                    Exception uploadException = null;

                    request.ProgressChanged += (sender) => Upload_ProgressChanged(sender, bytesTotal, uploadException, out uploadException);
                    //request.ResponseReceived += (sender) => Upload_ResponseReceived(sender, zipRes);

                    request.Upload();

                    if (request.ResponseBody == null)
                    {
                        Debug.WriteLine(@"9000. UploadToGoogleDrive Problem- request.accessKey");
                        errMessage = "UploadToGoogleDrive: No response found!" + (uploadException != null ? uploadException.Message : string.Empty);
                        Logger.WriteExMessage(errMessage);
                        return false;
                    }
                    else
                    {
                        zResult.GoogleDrive_Title = fileGD.Title;
                        zResult.GoogleDrive_Id = request.ResponseBody.Id;
                        zResult.GoogleDrive_DownloadUrl = request.ResponseBody.DownloadUrl;
                        zResult.GoogleDrive_FileSize = request.ResponseBody.FileSize;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(@"9001. UploadToGoogleDrive Error");
                errMessage = exception.Message;
                Logger.WriteEx(exception);
                return false;
            }

            Debug.WriteLine(@"9002. UploadToGoogleDrive OK");
            return true;
        }

        private static void Upload_ProgressChanged(IUploadProgress progress, long bytesTotal, Exception exxIn, out Exception exx)
        {
            exx = null;
            if (exxIn != null)
            {
                exx = exxIn;
                return;
            }
            var status = progress.Status.ToString().ToUpper();
            long bytesRemaining = bytesTotal - progress.BytesSent;
            Debug.WriteLine(string.Format("{0} - {1} bytes remaining", status, bytesRemaining));
            if (progress.Exception != null)
            {
                exx = progress.Exception;
                Logger.WriteEx(progress.Exception);
                Debug.WriteLine(string.Format("{0}", progress.Exception.Message));
                Debug.WriteLine(string.Format("{0}", progress.Exception.StackTrace));
            }
        }

        public bool DeleteZipFromGoogleDrive(ZipResult zipRes, out string errMessage)
        {
            bool result = false;
            errMessage = string.Empty;
            string googleDriveID = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(zipRes.GoogleDrive_Id))
                {
                    googleDriveID = zipRes.GoogleDrive_Id;

                    var client = new ApiClient();
                    var service = client.Methods.api.Authentification.ApiService;

                    var request = service.Files.Delete(zipRes.GoogleDrive_Id);
                    string resultExec = request.Execute();
                    string resultExec2 = service.Files.EmptyTrash().Execute();

                    result = (string.IsNullOrWhiteSpace(resultExec2));
                }
            }
            catch (Exception exception)
            {
                result = false;
                errMessage = exception.Message;
                Logger.WriteExMessage(string.Format("ZipResult.GoogleDrive_Id = '{0}'", googleDriveID));
                Logger.WriteEx(exception);
            }

            return result;
        }


        //public string DownloadFromGoogleDrive(string outputFileName, string downloadUrl)
        //{
        //    string resMessage = string.Empty;

        //    try
        //    {
        //        //output = fileName; //@"E:\Misc\Downloads\" + zipRes.GoogleDrive_Title;

        //        if (System.IO.File.Exists(outputFileName))
        //        {
        //            System.IO.File.Delete(outputFileName);
        //        }

        //        var client = new ApiClient();
        //        var service = client.Methods.api.Authentification.ApiService;

        //        long bytesTotal = 1000000; // zipRes.GoogleDrive_FileSize;
        //        int chunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 10; // ~ 2.5 MB

        //        var downloader = new MediaDownloader(service);
        //        downloader.ChunkSize = chunkSize;

        //        downloader.ProgressChanged += (sender) => Download_ProgressChanged(sender, bytesTotal);

        //        using (var stream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
        //        {
        //            var progress = downloader.Download(downloadUrl, stream);

        //            if (progress.Status != DownloadStatus.Completed)
        //            {
        //                resMessage = string.Format("Final status is {0}! Not 'Completed'!", progress.Status.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        resMessage = exception.Message;
        //        Logger.WriteEx(exception);
        //    }

        //    return resMessage;
        //}

        private void Download_ProgressChanged(IDownloadProgress progress, long? bytesTotal)
        {
            var status = progress.Status.ToString().ToUpper();
            long? bytesRemaining = bytesTotal - progress.BytesDownloaded;
            Debug.WriteLine(string.Format("{0} - {1} bytes remaining", status, bytesRemaining));
        }

    }
}