using System;
using System.Collections.Generic;
using System.IO;

namespace PasswordCrackerMaster
{
    public class FileChunkBalancer
    {
        private List<string> Passwords = new List<string>();
        public FileChunkBalancer(string dictionaryPath, string passwordPath)
        {
            using (FileStream fs = new FileStream(passwordPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader ReadingStream = new StreamReader(fs))
                {
                    while (!ReadingStream.EndOfStream)
                    {
                        string Entry = ReadingStream.ReadLine();

                        string[] allPasswords = Entry.Split(' ');
                        foreach (var word in allPasswords)
                        {
                            Passwords.Add(word);
                            Console.WriteLine(word);
                        }

                       

                    }

                    Console.WriteLine(Passwords.Count);
                    Console.ReadLine();
                }
            }
        }
    }
}