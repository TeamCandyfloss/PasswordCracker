using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace PasswordCrackerMaster
{/// <summary>
/// Giver et interval af ord der skal skannes.
/// </summary>
    public static class FileChunkBalancer
    {
        // chunkLock bruges til at låse metoden GetChunk så der ikke er 2 tråde der tilgår metoden samtidig
        private static readonly object _chunckLock = new object();
        private static int _currentlyScanning = 0;
        private static string _dictionaryPath = "webster.txt";
        private static List<string> WordList = new List<string>();
        private static bool _endOfLine = false;


        static FileChunkBalancer()
        {
            using (FileStream fs = new FileStream(_dictionaryPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader readingStream = new StreamReader(fs))
                {
                    while (!readingStream.EndOfStream)
                    {
                        string Entry = readingStream.ReadLine();

                        WordList.Add(Entry);
                    }
                }
            }
        }
        /// <summary>
        /// Gets an interval seperated by a space, amountOfWords is the interval of words to be scanned.
        /// </summary>
        /// <param name="amountOfWords"></param>
        /// <returns></returns>
        public static string GetChunk(int amountOfWords)
        {
            lock (_chunckLock)
            {
                long totalWords = WordList.Count;

                if (_endOfLine)
                {
                    return "666";
                }
                // Så længe  der flere ord at scanne og de ord der tilgængelige ikke overskrider mulige ord at hente.
                if (_currentlyScanning < totalWords && _currentlyScanning + amountOfWords < totalWords)
                {
                    return $"{_currentlyScanning} {_currentlyScanning += amountOfWords}";
                }
                else if (_currentlyScanning < totalWords && _currentlyScanning + amountOfWords > totalWords)
                {
                    _endOfLine = true;
                    return $"{_currentlyScanning} {(totalWords - _currentlyScanning)}";
                }

               
                // TODO throw exception - ikke flere ord
                return "666";
            }
        }
        /// <summary>
        /// ændre pathen til OrdListen
        /// </summary>
        public static string SetDictionaryPath
        {
            set => _dictionaryPath = value;
        }

       
    }

}