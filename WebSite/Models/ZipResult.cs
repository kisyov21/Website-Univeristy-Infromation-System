namespace WebSite.Models
{
    internal class ZipResult
    {
        public string GoogleDrive_DownloadUrl { get; set; }
        public long? GoogleDrive_FileSize { get; set; }
        public string GoogleDrive_Id { get; set; }
        public object GoogleDrive_Title { get; set; }
        public string Message { get; set; }
        public string SourceDirName { get; set; }
        public object Status { get; set; }
        public string ZipPath { get; set; }
    }
}