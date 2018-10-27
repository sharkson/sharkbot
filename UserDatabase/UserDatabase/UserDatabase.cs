using ChatModels;
using System.Collections.Concurrent;
using System.Diagnostics;
using UserDatabase.Services;

namespace UserDatabase
{
    public class UserDatabase
    {
        public static ConcurrentBag<UserData> userDatabase { get; set; }

        public static string userDirectory;

        private static UserLoadService userLoadService;

        public static void LoadDatabase(string directory)
        {
            userLoadService = new UserLoadService(directory);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            userDatabase = new ConcurrentBag<UserData>(userLoadService.LoadUsers());

            stopwatch.Stop();
            Debug.WriteLine("user database load time: " + stopwatch.Elapsed);
        }
    }
}
