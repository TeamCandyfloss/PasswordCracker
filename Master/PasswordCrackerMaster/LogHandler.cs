using System.Diagnostics;

namespace PasswordCrackerMaster
{
    static class LogHandler
    {
        private static int _connectedUsers;
        private static Stopwatch _stopwatch = new Stopwatch();
        private static string _Giveninterval;
        static LogHandler()
        {
            
        }

        public static void ConnectedUser()
        {
            _connectedUsers++;
        }

        public static void DisconnectedUser()
        {
            _connectedUsers--;
        }

        public static void StartStopWatch()
        {
            _stopwatch.Start();
        }

        public static void StopStopWatch()
        {
            _stopwatch.Stop();
        }

        public static void SetGivenValue(string value)
        {
            _Giveninterval = value;
        }

        public static string GetGivenValue()
        {
            return _Giveninterval;
        }

        public static string GetUsers()
        {
            return _connectedUsers.ToString();
        }

        public static Stopwatch Stopwatch()
        {
            return _stopwatch;
        }


    }
}