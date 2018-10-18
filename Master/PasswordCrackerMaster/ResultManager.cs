using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PasswordCrackerMaster
{
    /// <summary>
    /// Samler resulter fra clienten.
    /// </summary>
    static class ResultManager
    {
        private static Dictionary<string, string> _results = new Dictionary<string, string>();
        private static PasswordFileHandler _passwords = new PasswordFileHandler("passwords.txt");
        private static object ResultLock = new object();

        static ResultManager()
        {
           
        }

         public static void AddResult(Dictionary<string, string> partialResult)
        {
            lock (ResultLock)
            {
                foreach (var kvp in partialResult)
                {
                    if (!_results.ContainsKey(kvp.Key))
                    {
                    _results.Add(kvp.Key, kvp.Value);

                    }
                }

                if (_results.Count == _passwords.GetHashes().Count)
                {
                    LogHandler.StopStopWatch();
                }
            }
        }

        public static void ShowResult()
        {
            foreach (var kvp in _results)
            {
                Console.WriteLine($"{kvp.Key}, {kvp.Value}");
            }

            Console.WriteLine($"Der blev fundet {_results.Count} passwords du af {_passwords.GetHashes().Count} tilgængelige hashes. på {LogHandler.Stopwatch().Elapsed} ");
        }

        public static Dictionary<string, string> GetResults()
        {
            return _results;
        }

        
    }
}