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

        public static string GetChunk(int amountOfWords)
        {
            lock (_chunckLock)
            {
                long totalWords = WordList.Count;
                bool changeToWordCurrentScanningWasMade = false;
                // Så længe  der flere ord at skanne og de ord der tilgængelige ikke overskrider mulige ord at hente.
                if (_currentlyScanning < totalWords && _currentlyScanning + amountOfWords < totalWords)
                {
                    changeToWordCurrentScanningWasMade = true;
                    return $"{_currentlyScanning} {_currentlyScanning + amountOfWords}";
                }
                else if (_currentlyScanning < totalWords && _currentlyScanning + amountOfWords > totalWords)
                {
                    changeToWordCurrentScanningWasMade = true;
                    return $"{_currentlyScanning} {totalWords - _currentlyScanning}";
                }

                if (changeToWordCurrentScanningWasMade)
                {
                    _currentlyScanning =+ amountOfWords;
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