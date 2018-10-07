using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace PasswordCrackerMaster
{
    public static class FileChunkBalancer
    {
        private static readonly object _chunckLock = new object();
        private static int _currentlyScanning = 0;
        private static string _dictionaryPath = "webster.txt";
        private static List<string> WordList = new List<string>();

        static FileChunkBalancer()
        {
            using (FileStream fs = new FileStream(_dictionaryPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader ReadingStream = new StreamReader(fs))
                {
                    while (!ReadingStream.EndOfStream)
                    {
                        string Entry = ReadingStream.ReadLine();

                        WordList.Add(Entry);
                    }
                }
            }
        }

        public static string GetChunk(int AmountOfWords)
        {
            lock (_chunckLock)
            {
                long TotalWords = WordList.Count;

                // Så længe  der flere ord at skanne og de ord der tilgængelige ikke overskrider mulige ord at hente
                if (_currentlyScanning < TotalWords && _currentlyScanning + AmountOfWords < TotalWords)
                {
                    return $"{_currentlyScanning} {_currentlyScanning + AmountOfWords}";
                }
                else if (_currentlyScanning < TotalWords && _currentlyScanning + AmountOfWords > TotalWords)
                {
                    return $"{_currentlyScanning} {TotalWords - _currentlyScanning}";
                }

                return null;
            }
        }

        public static string SetDictionaryPath
        {
            set => _dictionaryPath = value;
        }
    }

}