using System.Collections.Generic;

namespace PasswordCrackerClient
{
    public static class ResultManager
    {
        private static Dictionary<string, string> _partialResult = new Dictionary<string, string>();
        static ResultManager()
        {
            
        }

        public static void AddResult(string user, string password)
        {
            _partialResult.Add(user, password);
        }

        public static Dictionary<string, string> GetPartialResult()
        {
            return _partialResult;
        }


    }
}