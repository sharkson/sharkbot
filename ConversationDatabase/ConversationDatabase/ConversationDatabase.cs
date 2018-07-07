using ChatModels;
using ConversationDatabase.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConversationDatabase
{
    public static class ConversationDatabase
    {
        private static ConversationLoadService conversationService;
        public static List<ConversationList> conversationDatabase;
        public static string conversationDirectory;

        public static void LoadDatabase(string directory)
        {
            conversationService = new ConversationLoadService(directory);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            conversationDatabase = new List<ConversationList>(conversationService.LoadConversations());

            stopwatch.Stop();
            Debug.WriteLine("conversation database load time: " + stopwatch.Elapsed);
        }
    }
}