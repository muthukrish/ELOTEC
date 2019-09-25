namespace ELOTEC.Infrastructure.Helpers
{
    using System;
    using System.IO;

    public static class Helper
    {
        public static string ContentRootPath { get; set; }
        public static string UploadFolderName = "Upload";
        public static string DownloadFolderName = "Download";

        public static string CombinePath(string dirPath, string fileName)
        {
            return Path.Combine(dirPath, fileName);
        }

        public static string UploadPath
        {
            get
            {
                string uploadPath = CombinePath(ContentRootPath, UploadFolderName);
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
                return uploadPath;
            }
        }

        public static string DownloadPath
        {
            get
            {
                string downloadPath = CombinePath(ContentRootPath, DownloadFolderName);
                if (!Directory.Exists(downloadPath)) Directory.CreateDirectory(downloadPath);
                return downloadPath;
            }
        }

        public static string LogFolderPath
        {
            get
            {
                string logFolderPath = Path.Combine(ContentRootPath, "Logs");
                if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath);
                return logFolderPath;
            }
        }

        public static string LogFolderName { get => CombinePath(LogFolderPath, string.Format("{0}_{1}.{2}", "Log", DateTime.UtcNow.ToString("yyyyMMdd"), "txt")); }

        public static string IsFileExists(string fileName)
        {
            string filePath = Path.Combine(UploadPath, fileName);

            if (File.Exists(filePath))
            {
                string ext = Path.GetExtension(filePath).Replace(".", "");
                fileName = string.Format("{0}.{1}", Guid.NewGuid(), ext);
            }

            return fileName;
        }

        public static byte[] FileToByteArray(string fileName)
        {
            byte[] fileContent = null;
            fileName = Path.Combine(UploadPath, fileName);
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fs);
            long byteLength = new FileInfo(fileName).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();

            return fileContent;
        }

        public static void WriteLog(Exception ex)
        {
            string filePath = Path.Combine(LogFolderPath, LogFolderName);
            string errorText = string.Format("{0} : {1}", DateTime.UtcNow.ToString(), ex.ToString());

            if (!File.Exists(filePath))
            {
                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine(errorText);
                }
            }
            else
            {
                using (var tw = new StreamWriter(filePath, true))
                {
                    tw.WriteLine(errorText);
                }
            }
        }
    }
}
