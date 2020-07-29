﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace IGPS.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public bool LoggedInWithSNSAccount { get; set; }
        public SNSProvider Provider { get; set; }

        public void SaveUserInfo()
        {
            try
            {
                var serializeSettings = JsonOptions.GetSerializerSettings();
                string serialized = JsonConvert.SerializeObject(this, serializeSettings);

                Preferences.Set("User", serialized);
            }
            catch (Exception)
            {

            }
        }

        public static UserInfo LoadUserInfo()
        {
            UserInfo result = default;

            try
            {
                string serialized = Preferences.Get("User", null);
                var serializeSettings = JsonOptions.GetSerializerSettings();

                result = string.IsNullOrEmpty(serialized) ? null : JsonConvert.DeserializeObject<UserInfo>(serialized);
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static void RemoveUserInfo()
        {
            try
            {
                Preferences.Set("User", string.Empty);                
            }
            catch (Exception)
            {

            }
        }
    }
}