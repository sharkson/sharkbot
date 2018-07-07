using ChatModels;
using Newtonsoft.Json;
using SharkbotConfiguration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserDatabase.Services
{
    public class UserLoadService
    {
        private readonly string _databaseDirectory;

        public UserLoadService(string databaseDirectory)
        {
            _databaseDirectory = databaseDirectory;
        }

        public List<UserData> LoadUsers()
        {
            var analyzationVersion = ConfigurationService.AnalyzationVersion;

            var userDataList = new ConcurrentBag<UserData>();

            string[] filePaths = Directory.GetFiles(_databaseDirectory);

            Parallel.ForEach(filePaths, (fileName) => {
                using (StreamReader file = File.OpenText(fileName))
                {
                    var serializer = new JsonSerializer();
                    var userData = (UserData)serializer.Deserialize(file, typeof(UserData));
                    if (!string.IsNullOrWhiteSpace(userData.userName) && userData.nickNames != null && userData.nickNames.Count > 0 && userData.nickNames[0] != null)
                    {
                        if (userData.properties == null)
                        {
                            userData.properties = new List<UserProperty>();
                        }
                        userDataList.Add(userData);
                    }
                }
            });

            return userDataList.ToList();
        }
    }
}
