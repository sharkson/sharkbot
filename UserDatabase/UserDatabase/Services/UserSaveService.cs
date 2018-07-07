using ChatModels;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace UserDatabase.Services
{
    public class UserSaveService
    {
        public bool SaveUserData(UserData userData)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userData);
                File.WriteAllText(UserDatabase.userDirectory + "\\" + userData.fileName + ".json", json, Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
