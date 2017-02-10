using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleApi1.Code
{
    public class GoogleDriveFile
    {
        public string Title { get; set; }
        public string GoogleDrive_ID { get; set; }
        public string DownloadURL { get; set; }
        public string FileSize { get; set; }

        public GoogleDriveFile(string _title, string _googleDriveID, string _downloadURL, string _fileSize )
        {
            this.Title = _title;
            this.GoogleDrive_ID = _googleDriveID;
            this.DownloadURL = _downloadURL;
            this.FileSize = _fileSize;
        }
    }
}
