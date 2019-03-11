using ChatAnalyzer.Services;
using ChatModels;
using Newtonsoft.Json;
using SharkbotConfiguration;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConversationDatabase.Services
{
    public class ConversationLoadService
    {
        private readonly string _databaseDirectory;
        private readonly AnalyzationService _analyzationService;

        public ConversationLoadService(string databaseDirectory, AnalyzationService analyzationService)
        {
            _databaseDirectory = databaseDirectory;
            _analyzationService = analyzationService;
        }

        public ConcurrentDictionary<string, ConversationList> LoadConversations()
        {
            var analyzationVersion = ConfigurationService.AnalyzationVersion;

            var conversationLists = new ConcurrentDictionary<string, ConversationList>();

            foreach (var directory in Directory.GetDirectories(_databaseDirectory))
            {
                conversationLists[Path.GetFileName(directory)] = LoadConversationList(directory);
            }

            foreach (var conversationList in conversationLists)
            {               
                foreach (var conversation in conversationList.Value.conversations)
                {
                    if (conversation.Value.analyzationVersion != analyzationVersion)
                    {
                        var analyzedConversation = _analyzationService.AnalyzeConversationAsync(conversation.Value);
                        conversationLists[conversationList.Key].conversations[conversation.Key] = analyzedConversation;
                        var json = JsonConvert.SerializeObject(analyzedConversation);
                        File.WriteAllText(_databaseDirectory + "\\" + conversationList.Value.type + "\\" + analyzedConversation.name + ".json", json, Encoding.Unicode);
                    }
                }
            }

            return conversationLists;
        }

        private ConversationList LoadConversationList(string directory)
        {
            var conversations = new ConcurrentDictionary<string, Conversation>();

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
                    conversations[conversationName] = conversation;
                }
            });

            return new ConversationList { conversations = conversations, type = Path.GetFileName(directory) };
        }
    }
}
