﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PasswordCrackerMaster
{
    static class ResultManager
    {
        private static Dictionary<string, string> _results = new Dictionary<string, string>();
       private static PasswordFileHandler _passwords = new PasswordFileHandler("password.txt");
       private static Stopwatch _stopwatch = new Stopwatch();
        private static object ResultLock = new object();

        static ResultManager()
        {
            _stopwatch.Start();
        }

         public static void AddResult(Dictionary<string, string> partialResult)
        {
            lock (ResultLock)
            {
                foreach (var kvp in partialResult)
                {
                    _results.Add(kvp.Key, kvp.Value);
                }

                if (_results.Count == _passwords.GetHashes().Count)
                {
                    _stopwatch.Stop();
                }
            }
        }

        public static void ShowResult()
        {
            foreach (var kvp in _results)
            {
                Console.WriteLine(kvp.Key, kvp.Value);
            }

            Console.WriteLine($"Der blev fundet {_results.Count} passwords du af {_passwords.GetHashes().Count} tilgængelige hashes. på {_stopwatch.Elapsed} ");
        }

        
    }
}