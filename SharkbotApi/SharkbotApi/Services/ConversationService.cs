using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ChatModels;

namespace SharkbotApi.Services
{
    public class ConversationService
    {
        public Conversation GetConversation(string conversationName, string type)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase;
        
            Conversation conversation = null;
            if (conversationLists.ContainsKey(type))
            {
                var conversationList = conversationLists[type];
                if (conversationList != null && conversationList.conversations.ContainsKey(conversationName))
                {
                    var existingConversation = conversationList.conversations[conversationName];
                    if (existingConversation != null)
                    {
                        conversation = DeepCopy(existingConversation);
                    }
                }
            }

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    name = conversationName,
                    responses = new List<AnalyzedChat>()
                };
            }

            return conversation;
        }

        public Conversation GetConversation(ChatRequest chatRequest)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase;

            Conversation conversation = null;
            if (conversationLists.ContainsKey(chatRequest.type))
            {
                var conversationList = conversationLists[chatRequest.type];                
                if (conversationList != null && conversationList.conversations.ContainsKey(chatRequest.conversationName))
                {
                    var existingConversation = conversationList.conversations[chatRequest.conversationName];
                    if (existingConversation != null)
                    {
                        conversation = DeepCopy(existingConversation);
                    }
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


        public int ConversationLength(string conversationName, string type)
        {
            var conversationLists = ConversationDatabase.ConversationDatabase.conversationDatabase;

            if (conversationLists.ContainsKey(type))
            {
                var conversationList = conversationLists[type];
                if (conversationList != null && conversationList.conversations.ContainsKey(conversationName))
                {
                    var conversation = conversationList.conversations[conversationName];
                    if(conversation.responses != null)
                    {
                        return conversation.responses.Count();
                    }
                    return 0;
                }
            }

            return 0;
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
