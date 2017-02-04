using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleApi.Code
{
    public class Authentification
    {
        internal bool IsAuthenticated { get; set; }
        internal string AuthenticationResponse { get; set; }

        private string ApiServiceAccount { get; set; }
        private string ApiPrivateKey { get; set; }
        public string ApiDriveFolder { get; private set; }
        public DriveService ApiService { get; private set; }

        private readonly string[] Scopes = new[] { DriveService.Scope.DriveFile, DriveService.Scope.Drive };

        internal Authentification()
        {
            IsAuthenticated = false;
            ApiServiceAccount = ConfigurationManager.AppSettings["GoogleServiceAccount"];
            ApiPrivateKey = ConfigurationManager.AppSettings["GooglePrivateKey"];
            ApiDriveFolder = ConfigurationManager.AppSettings["GoogleParentFolder"];
            //<add key="GoogleServiceAccount" value="dst-637@dst-tool.iam.gserviceaccount.com" />
            //< add key = "GoogleParentFolder" value = "0B81THbNfE2Uqa0ltOW5GcnB4X2s" />
            //< add key = "GooglePrivateKey" value = "-----BEGIN PRIVATE KEY-----&#xA;MIIEvgIBADANBgkqhkiG9w0BAQEFAAY-----&#xA;" />
        }

        internal void Authenticate()
        {
            try
            {
                AuthenticateServiceAccount(ApiServiceAccount, ApiPrivateKey);

                if (ApiService != null)
                {
                    IsAuthenticated = true;
                }
            }
            catch (Exception ex)
            {
                AuthenticationResponse = ex.Message;
            }
        }

        private void AuthenticateServiceAccount(string serviceAccount, string privateKey)
        {
            var credentialInitializer = new ServiceAccountCredential.Initializer(serviceAccount) { Scopes = Scopes }.FromPrivateKey(privateKey);

            ServiceAccountCredential credentials = new ServiceAccountCredential(credentialInitializer);

            ApiService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "DST",
            });
        }
    }
}
