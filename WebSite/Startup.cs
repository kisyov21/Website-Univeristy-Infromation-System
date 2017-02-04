using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using GoogleApi1;
using Microsoft.Owin;
using Owin;
using WebSite.Methods;

[assembly: OwinStartupAttribute(typeof(WebSite.Startup))]
namespace WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var service = GoogleDrive.NewService();
            //GoogleDrive.createDirectory(service, "WebSite", "Drive space for Univeristy system", "0ADf6FvIvJw1hUk9PVA");
            //GoogleDrive.PrintParents(service, "0Bzf6FvIvJw1hc3RhcnRlcl9maWxl");
            // ParentList parents = service.parents();
            FilesResource.ListRequest request = service.Files.List();
            var files = request.Execute();
            
            ConfigureAuth(app);
        }
    }
}
