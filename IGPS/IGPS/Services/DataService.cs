using IGPS.Models;
using IGPS.Services.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IGPS.Services
{
    public class DataService
    {
        UserInfo User => AppEnvironment.authService.AuthenticatedUser;
        string LocalDataFilePath => Path.Combine(AppEnvironment.appDataPath, User.GetUserString(), "UserVoiceData.xml");
        string ServerDefaultDataDirPath => Path.Combine(AppEnvironment.ftpRootPath, "UserData", "Default");
        string ServerUserDataDirPath => Path.Combine(AppEnvironment.ftpRootPath, "UserData", User.GetUserString());
        readonly string serverFile = "VoiceData.xml";

        DataTable voiceDataTable = null;
        public List<VoiceDataItem> voiceDataItems;

        public DataService()
        {
            LoadTable();
            ListItem();
        }

        public bool ListItem()
        {
            try
            {
                if (voiceDataItems == null)
                {
                    voiceDataItems = new List<VoiceDataItem>();
                }

                foreach (DataRow dr in voiceDataTable.Rows)
                {
                    voiceDataItems.Add(new VoiceDataItem(dr));
                }

                voiceDataItems.TrimExcess();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public DataRow FindDataRow<T>(string columnIndex, T value)
        {
            if (voiceDataTable == null)
            {
                return null;
            }

            foreach (DataRow dr in voiceDataTable.Rows)
            {
                if (value.Equals((T)dr[columnIndex]))
                {
                    return dr;
                }
            }

            return null;
        }

        public bool LoadTable()
        {
            try
            {
                if (DownloadTable())
                {
                    if (voiceDataTable == null)
                    {
                        voiceDataTable = new DataTable();
                    }

                    voiceDataTable.ReadXml(LocalDataFilePath);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool RefreshTable()
        {
            try
            {
                if (DownloadTable())
                {
                    voiceDataTable.Clear();
                    voiceDataTable.ReadXml(LocalDataFilePath);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool DownloadTable()
        {
            string serverPath = Path.Combine(FTPService.CheckDirExist(ServerUserDataDirPath) ? ServerUserDataDirPath : ServerDefaultDataDirPath, serverFile); 

            return FTPService.DownloadFile(LocalDataFilePath, serverPath);
        }

        public bool UploadTable() => FTPService.UploadFile(LocalDataFilePath, Path.Combine(ServerUserDataDirPath, serverFile));
    }
}
