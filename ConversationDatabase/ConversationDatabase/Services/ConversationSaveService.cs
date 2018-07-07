using ChatModels;
using Newtonsoft.Json;
using System;
using System.Text;
using System.IO;

namespace ConversationDatabase.Services
{
    public class ConversationSaveService
    {
        public bool SaveConversation(Conversation conversation, string type)
        {
            try
            {
                string json = JsonConvert.SerializeObject(conversation);
                if(!Directory.Exists(ConversationDatabase.conversationDirectory + type))
                {
                    Directory.CreateDirectory(ConversationDatabase.conversationDirectory + type);
                }
                File.WriteAllText(ConversationDatabase.conversationDirectory + type + "\\" + conversation.name + ".json", json, Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
