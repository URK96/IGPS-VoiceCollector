using System;
using System.IO;
using System.Net;

namespace IGPS.Services.Server
{
    public class FTPService
    {
        const string id = "igpsUser";
        const string pw = "bise1549";

        public static FtpWebRequest CreateRequest(string path)
        {
            var request = WebRequest.Create(path) as FtpWebRequest;
            request.Credentials = new NetworkCredential(id, pw);

            return request;
        }

        public static bool CheckDirExist(string path)
        {
            var request = CreateRequest(path);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            try
            {
                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckFileExist(string path)
        {
            var request = CreateRequest(path);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CreateDir(string path)
        {
            var request = CreateRequest(path);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UploadFile(string filePath, string serverPath)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Credentials = new NetworkCredential(id, pw);

                    wc.UploadFileTaskAsync(serverPath, filePath);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool DownloadFile(string filePath, string serverPath)
        {
            try
            {
                string targetDir = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                using (var wc = new WebClient())
                {
                    wc.Credentials = new NetworkCredential(id, pw);

                    wc.DownloadFile(serverPath, filePath);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
