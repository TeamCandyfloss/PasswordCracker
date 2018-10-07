using System;
using System.Collections.Generic;
using System.IO;

namespace PasswordCrackerMaster
{
    public class PasswordFileHandler
    {
        private Dictionary<string,string> HashesKVP = new Dictionary<string, string>();
        public PasswordFileHandler(string PasswordFilePath)
        {
            

            using (FileStream fs = new FileStream(PasswordFilePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader ReadingStream = new StreamReader(fs))
                {
                    while (!ReadingStream.EndOfStream)
                    {
                        string Entry = ReadingStream.ReadLine();
                        string[] EntrySplit = Entry.Split(':');

                        HashesKVP.Add(EntrySplit[0], EntrySplit[1]);
                    }
                }
            }

        }

        public Dictionary<string, string> GetHashes()
        {
            return HashesKVP;
        }
    }
}