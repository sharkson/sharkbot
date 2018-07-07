using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ChatModels;

namespace SharkbotApi.Services
{
    public class ConversationService
    {
        public Conversation GetConversation(ChatRequest chatRequest)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase;

            var conversationList = conversationLists.Where(cl => cl.type == chatRequest.type).FirstOrDefault();
            Conversation conversation = null;
            if (conversationList != null)
            {
                var existingConversation = conversationList.conversations.Where(c => c.name == chatRequest.conversationName).FirstOrDefault();
                if(existingConversation != null)
                {
                    conversation = DeepCopy(existingConversation);
                }
            }

            if(conversation == null)
            {
                conversation = new Conversation
                {
                    name = chatRequest.conversationName,
                    responses = new List<AnalyzedChat>()
                };
            }

            var analyzedChat = new AnalyzedChat();
            analyzedChat.botName = chatRequest.chat.botName;
            analyzedChat.chat = chatRequest.chat;

            conversation.responses.Add(analyzedChat);

            return conversation;
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
