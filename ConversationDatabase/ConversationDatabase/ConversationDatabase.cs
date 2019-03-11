using ChatAnalyzer.Services;
using ChatModels;
using ConversationDatabase.Services;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ConversationDatabase
{
    public static class ConversationDatabase
    {
        public static ConcurrentDictionary<string, ConversationList> conversationDatabase;
        public static string conversationDirectory;

        public static void LoadDatabase(string directory, AnalyzationService analyzationService)
        {
            var conversationService = new ConversationLoadService(directory, analyzationService);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            conversationDatabase = new ConcurrentDictionary<string, ConversationList>(conversationService.LoadConversations());

            stopwatch.Stop();
            Debug.WriteLine("conversation database load time: " + stopwatch.Elapsed);
        }
    }
}