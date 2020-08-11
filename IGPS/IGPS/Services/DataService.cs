using IGPS.Models;
using IGPS.Services.Server;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Xamarin.Forms;

namespace IGPS.Services
{
    public class DataService
    {
        UserInfo User => AppEnvironment.authService.AuthenticatedUser;
        public string LocalDataPath => Path.Combine(AppEnvironment.appDataPath, User.GetUserString());
        public string LocalDataFilePath => Path.Combine(AppEnvironment.appDataPath, User.GetUserString(), "UserVoiceData.xml");
        string ServerDefaultDataDirPath => Path.Combine(AppEnvironment.ftpRootPath, "UserData", "Default");
        public string ServerUserDataDirPath => Path.Combine(AppEnvironment.ftpRootPath, "UserData", User.GetUserString());
        readonly string serverFile = "VoiceData.xml";
        readonly string voiceStatusFile = "VoiceStatus.txt";

        public DataTable voiceDataTable = null;
        public List<VoiceDataItem> voiceDataItems;
        public Dictionary<int, string[]> voiceTextData; // Section, Data List
        public Dictionary<int, int[]> voiceStatusData;

        public DataService()
        {
            LoadTextData();
            CheckVoiceStatusFile();
            LoadVoiceStatus();
        }

        private void LoadTextData()
        {
            voiceTextData = new Dictionary<int, string[]>();

            voiceTextData.Add(1, AppResources.Section1.Split('\n'));
            voiceTextData.Add(2, AppResources.Section2.Split('\n'));
        }

        private void CheckVoiceStatusFile()
        {
            string filePath = Path.Combine(LocalDataPath, voiceStatusFile);
            string serverPath = Path.Combine(ServerUserDataDirPath, voiceStatusFile);

            if (!File.Exists(filePath))
            {
                if (FTPService.CheckFileExist(serverPath))
                {
                    FTPService.DownloadFile(filePath, serverPath);
                }
                else
                {
                    File.WriteAllText(filePath, "");
                }
            }
        }

        private void LoadVoiceStatus()
        {
            string filePath = Path.Combine(LocalDataPath, voiceStatusFile);

            voiceStatusData = new Dictionary<int, int[]>();

            if (File.Exists(filePath))
            {
                string[] valueList = File.ReadAllText(filePath).Split('\n');
                var values = new List<int>();

                for (int i = 0; i < voiceTextData.Keys.Count; ++i)
                {
                    string valueString;
                    int key = i + 1;

                    try
                    {
                        valueString = valueList[i];

                        if (string.IsNullOrWhiteSpace(valueString))
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        voiceStatusData.Add(key, new int[voiceTextData[key].Length]);

                        continue;
                    }

                    values.Clear();

                    foreach (var value in valueString)
                    {
                        values.Add(Convert.ToInt32(value));
                    }

                    voiceStatusData.Add(key, values.ToArray());
                }
            }
            else
            {
                DependencyService.Get<IToast>().Show("Cannot load voice status");
            }
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

        public DataRow FindDataRow(int section, int number)
        {
            const string SectionIndex = "Section";
            const string NumberIndex = "No.";

            if (voiceDataTable == null)
            {
                return null;
            }

            foreach (DataRow dr in voiceDataTable.Rows)
            {
                if ((section == (int)dr[SectionIndex]) && (number == (int)dr[NumberIndex]))
                {
                    return dr;
                }
            }

            return null;
        }

        public void UpdateItem()
        {
            voiceDataItems.Clear();
            ListItem();
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

        public bool UploadVoiceStatus()
        {
            string filePath = Path.Combine(LocalDataPath, voiceStatusFile);
            string serverPath = Path.Combine(ServerUserDataDirPath, voiceStatusFile);

            if (!FTPService.CheckDirExist(ServerUserDataDirPath))
            {
                FTPService.CreateDir(ServerUserDataDirPath);
            }

            return FTPService.UploadFile(filePath, serverPath);
        }

        public bool DownloadTable()
        {
            string serverPath = Path.Combine(FTPService.CheckDirExist(ServerUserDataDirPath) ? ServerUserDataDirPath : ServerDefaultDataDirPath, serverFile);

            return FTPService.DownloadFile(LocalDataFilePath, serverPath);
        }

        public bool UploadTable()
        {
            if (!FTPService.CheckDirExist(ServerUserDataDirPath))
            {
                FTPService.CreateDir(ServerUserDataDirPath);
            }

            return FTPService.UploadFile(LocalDataFilePath, Path.Combine(ServerUserDataDirPath, serverFile));
        }
    }
}
