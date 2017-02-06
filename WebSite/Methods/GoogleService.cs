using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using GoogleApi1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using WebSite.Models;
using File = Google.Apis.Drive.v2.Data.File;

namespace WebSite.Methods
{
    public class GoogleService
    {
    //    //public static DriveService AuthenticateServiceAccount()
    //    //{
    //    //    string keyFilePath = @"D:\Repositories\Website-Univeristy-Infromation-System\GoogleDriveApi\UniversitySystem-c4d16256210d.p12";
    //    //    string serviceAccountEmail = "801416430238-compute@developer.gserviceaccount.com";
    //    //    using (var stream = new FileStream(keyFilePath,FileMode.Open,FileAccess.Read)) {

    //    //    }
    //    //    if (!File.Exists(keyFilePath))
    //    //    {
    //    //        //TODO 
    //    //        //error message - key file doest not exist
    //    //        return null;
    //    //    }
    //    //    string[] scopes = new string[] { DriveService.Scope.Drive };

    //    //    var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
    //    //    try
    //    //    {
    //    //        ServiceAccountCredential credential = new ServiceAccountCredential(
    //    //            new ServiceAccountCredential.Initializer(serviceAccountEmail)
    //    //            {
    //    //                Scopes = scopes
    //    //            }.FromCertificate(certificate));

    //    //        //create service
    //    //        DriveService service = new DriveService(new BaseClientService.Initializer()
    //    //        {
    //    //            HttpClientInitializer = credential,
    //    //            ApplicationName = "Drive API Sample"
    //    //        });
    //    //        return service;
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return null;
    //    //    }
    //    //}
    //    private string UploadZipToGoogleDrive(ZipResult zipRes)
    //    {
    //        try
    //        {
    //            string contentType = "application/zip, application/octet-stream";


    //            var service = GoogleDrive.NewService();
    //            var title = zipRes.ZipPath.Split('\\').Last();
    //            var parent = new ParentReference { Id = "0B6g-9fVqqIureUZZQVhiTzU3UzQ" };
    //            var parents = new List<ParentReference> { parent };

    //            var body = new File
    //            {
    //                Title = title,
    //                MimeType = contentType,
    //                Parents = parents
    //            };

    //            using (var stream = new FileStream(zipRes.ZipPath, FileMode.Open))
    //            {
    //                var bytesTotal = stream.Length;
    //                int chunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 5; // ~ 1.25 MB

    //                var request = service.Files.Insert(body, stream, contentType);
    //                request.ChunkSize = chunkSize;

    //                request.ProgressChanged += (sender) => Upload_ProgressChanged(sender, bytesTotal);
    //                request.ResponseReceived += (sender) => Upload_ResponseReceived(sender, zipRes);

    //                request.Upload();
    //            }
    //        }
    //        catch (Exception exception)
    //        {
    //            return "FAILED";
    //        }

    //        return "UPLOADED";
    //    }

    //    private static void Upload_ProgressChanged(IUploadProgress progress, long bytesTotal)
    //    {
    //        var status = progress.Status.ToString().ToUpper();
    //        long bytesRemaining = bytesTotal - progress.BytesSent;
    //    }

    //    private static void Upload_ResponseReceived(File file, ZipResult zipRes)
    //    {
    //        zipRes.GoogleDrive_Title = file.Title;
    //        zipRes.GoogleDrive_Id = file.Id;
    //        zipRes.GoogleDrive_DownloadUrl = file.DownloadUrl;
    //        zipRes.GoogleDrive_FileSize = file.FileSize;
    //    }

    //    private string DownloadZipFromGoogleDrive(ZipResult zipRes)
    //    {
    //        string output = string.Empty;

    //        try
    //        {
    //            output = @"E:\Misc\Downloads\" + zipRes.GoogleDrive_Title;

    //            if (System.IO.File.Exists(output))
    //            {
    //                System.IO.File.Delete(output);
    //            }

    //            var service = GoogleDrive.NewService();

    //            var bytesTotal = zipRes.GoogleDrive_FileSize;
    //            int chunkSize = FilesResource.InsertMediaUpload.MinimumChunkSize * 10; // ~ 2.5 MB

    //            var downloader = new MediaDownloader(service);
    //            downloader.ChunkSize = chunkSize;

    //            downloader.ProgressChanged += (sender) => Download_ProgressChanged(sender, bytesTotal);

    //            using (var stream = new FileStream(output, FileMode.Create, FileAccess.Write))
    //            {
    //                var progress = downloader.Download(zipRes.GoogleDrive_DownloadUrl, stream);

    //                if (progress.Status != DownloadStatus.Completed)
    //                {
    //                    return "FAILED";
    //                }
    //            }
    //        }
    //        catch (Exception exception)
    //        {
    //            return "FAILED";
    //        }

    //        return string.Format("SAVED TO {0}", output);
    //    }

    //    private void Download_ProgressChanged(IDownloadProgress progress, long? bytesTotal)
    //    {
    //        var status = progress.Status.ToString().ToUpper();
    //        long? bytesRemaining = bytesTotal - progress.BytesDownloaded;
    //    }

    //    private string DeleteZipFromGoogleDrive(ZipResult zipRes)
    //    {
    //        try
    //        {
    //            if (!string.IsNullOrWhiteSpace(zipRes.GoogleDrive_Id))
    //            {
    //                var service = GoogleDrive.NewService();

    //                var request = service.Files.Delete(zipRes.GoogleDrive_Id);
    //                var result = request.Execute();

    //                result = service.Files.EmptyTrash().Execute();

    //                if (string.IsNullOrWhiteSpace(result))
    //                {
    //                    zipRes.GoogleDrive_Title = null;
    //                    zipRes.GoogleDrive_Id = null;
    //                    zipRes.GoogleDrive_DownloadUrl = null;
    //                    zipRes.GoogleDrive_FileSize = null;

    //                    return "DELETED";
    //                }
    //            }
    //        }
    //        catch (Exception exception)
    //        {
    //        }

    //        return "FAILED";
    //    }

    }
}