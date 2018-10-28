using ChatModels;
using ConversationDatabase.Services;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ConversationDatabase
{
    public static class ConversationDatabase
    {
        private static ConversationLoadService conversationService;
        public static ConcurrentBag<ConversationList> conversationDatabase;
        public static string conversationDirectory;

        public static void LoadDatabase(string directory)
        {
            conversationService = new ConversationLoadService(directory);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            conversationDatabase = new ConcurrentBag<ConversationList>(conversationService.LoadConversations());

            stopwatch.Stop();
            Debug.WriteLine("conversation database load time: " + stopwatch.Elapsed);
        }
    }
}