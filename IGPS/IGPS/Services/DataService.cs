using IGPS.Models;
using IGPS.Services.Server;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        readonly string firstVoiceSetStatusFile = "FirstVoiceStatus.txt";

        //public DataTable voiceDataTable = null;
        //public List<VoiceDataItem> voiceDataItems;
        public Dictionary<int, string[]> voiceTextData; // Section, Data List
        public Dictionary<int, int[]> voiceStatusData;
        public List<int> firstVoiceSetStatusData;

        public DataService()
        {
            RefreshData();
        }

        public void RefreshData()
        {
            LoadTextData();
            CheckVoiceStatusFile();
            CheckFirstVoiceStatusFile();
            LoadVoiceStatus();
            LoadFirstVoiceStatus();
        }

        private void LoadTextData()
        {
            voiceTextData = new Dictionary<int, string[]>
            {
                { 1, AppResources.Section1.Split('\n') },
                { 2, AppResources.Section2.Split('\n') }
            };
        }

        private void CheckVoiceStatusFile()
        {
            string filePath = Path.Combine(LocalDataPath, voiceStatusFile);
            string serverPath = Path.Combine(ServerUserDataDirPath, voiceStatusFile);

            if (!Directory.Exists(LocalDataPath))
            {
                Directory.CreateDirectory(LocalDataPath);
            }

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

        private void CheckFirstVoiceStatusFile()
        {
            string filePath = Path.Combine(LocalDataPath, firstVoiceSetStatusFile);
            string serverPath = Path.Combine(ServerUserDataDirPath, firstVoiceSetStatusFile);

            if (!Directory.Exists(LocalDataPath))
            {
                Directory.CreateDirectory(LocalDataPath);
            }

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
                        values.Add(int.Parse(value.ToString()));
                    }

                    voiceStatusData.Add(key, values.ToArray());
                }
            }
            else
            {
                DependencyService.Get<IToast>().Show("Cannot load voice status");
            }
        }

        private void LoadFirstVoiceStatus()
        {
            string filePath = Path.Combine(LocalDataPath, firstVoiceSetStatusFile);

            firstVoiceSetStatusData = new List<int>();

            if (File.Exists(filePath))
            {
                string valueString = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(valueString))
                {
                    firstVoiceSetStatusData.AddRange(new int[9]);
                }
                else
                {
                    foreach (var value in valueString)
                    {
                        firstVoiceSetStatusData.Add(int.Parse(value.ToString()));
                    }
                }
            }
            else
            {
                DependencyService.Get<IToast>().Show("Cannot load first voice status");
            }
        }

        public bool SaveVoiceStatus()
        {
            string filePath = Path.Combine(LocalDataPath, voiceStatusFile);

            try
            {
                var builder = new StringBuilder();

                foreach (var key in voiceStatusData.Keys)
                {
                    foreach (var value in voiceStatusData[key])
                    {
                        builder.Append(value);
                    }

                    builder.AppendLine();
                }

                File.WriteAllText(filePath, builder.ToString());
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool SaveFirstVoiceStatus()
        {
            string filePath = Path.Combine(LocalDataPath, firstVoiceSetStatusFile);

            try
            {
                var builder = new StringBuilder();

                foreach (var value in firstVoiceSetStatusData)
                {
                    builder.Append(value);
                }

                File.WriteAllText(filePath, builder.ToString());
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /*public bool ListItem()
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
        }*/

        /*public DataRow FindDataRow(int section, int number)
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
        }*/

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

        public bool UploadFirstVoiceStatus()
        {
            string filePath = Path.Combine(LocalDataPath, firstVoiceSetStatusFile);
            string serverPath = Path.Combine(ServerUserDataDirPath, firstVoiceSetStatusFile);

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
