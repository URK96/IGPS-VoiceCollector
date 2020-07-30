using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IGPS.Services.Server
{
    public class FTPService
    {
        const string id = "ftpUser";
        const string pw = "1598462";

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
            catch (WebException ex)
            {
                return false;
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
                string serverParentDir = Path.GetDirectoryName(serverPath);

                if (!CheckDirExist(serverParentDir))
                {
                    CreateDir(serverParentDir);
                }

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
