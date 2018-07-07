using ChatAnalyzer.Services;
using ChatModels;
using Newtonsoft.Json;
using SharkbotConfiguration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversationDatabase.Services
{
    public class ConversationLoadService
    {
        private readonly string _databaseDirectory;
        private AnalyzationService analyzationService;

        public ConversationLoadService(string databaseDirectory)
        {
            _databaseDirectory = databaseDirectory;
            analyzationService = new AnalyzationService();
        }

        public List<ConversationList> LoadConversations()
        {
            var analyzationVersion = ConfigurationService.AnalyzationVersion;

            var conversationLists = new List<ConversationList>();

            foreach (var directory in Directory.GetDirectories(_databaseDirectory))
            {
                conversationLists.Add(LoadConversationList(directory));
            }

            for (var index = 0; index < conversationLists.Count(); index++)
            {
                for(var subIndex = 0; subIndex < conversationLists[index].conversations.Count(); subIndex++)
                {
                    if (conversationLists[index].conversations[subIndex].analyzationVersion != analyzationVersion)
                    {
                        conversationLists[index].conversations[subIndex] = analyzationService.AnalyzeConversation(conversationLists[index].conversations[subIndex]);
                        var json = JsonConvert.SerializeObject(conversationLists[index].conversations[subIndex]);
                        File.WriteAllText(_databaseDirectory + "\\" + conversationLists[index].type + "\\" + conversationLists[index].conversations[subIndex].name + ".json", json, Encoding.Unicode);
                    }
                }
            }

            return conversationLists;
        }

        private ConversationList LoadConversationList(string directory)
        {
            var conversations = new ConcurrentBag<Conversation>();

            string[] filePaths = Directory.GetFiles(directory);

            Parallel.ForEach(filePaths, (fileName) => {
                using (StreamReader file = File.OpenText(fileName))
                {
                    var serializer = new JsonSerializer();
                    var conversation = (Conversation)serializer.Deserialize(file, typeof(Conversation));
                    var conversationName = Path.GetFileNameWithoutExtension(fileName);
                    if (conversation.name != conversationName)
                    {
                        conversation.name = conversationName;
                    }
                    conversations.Add(conversation);
                }
            });

            return new ConversationList { conversations = conversations.ToList(), type = Path.GetFileName(directory) };
        }
    }
}
