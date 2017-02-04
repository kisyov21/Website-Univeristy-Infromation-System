using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
//using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace WebSite.Methods
{
    public class GoogleService
    {
        public static DriveService AuthenticateServiceAccount()
        {
            string keyFilePath = @"D:\Repositories\Website-Univeristy-Infromation-System\GoogleDriveApi\UniversitySystem-c4d16256210d.p12";
            string serviceAccountEmail = "801416430238-compute@developer.gserviceaccount.com";
            using (var stream = new FileStream(keyFilePath,FileMode.Open,FileAccess.Read)) {

            }
            if (!File.Exists(keyFilePath))
            {
                //TODO 
                //error message - key file doest not exist
                return null;
            }
            string[] scopes = new string[] { DriveService.Scope.Drive };

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                //create service
                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API Sample"
                });
                return service;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}