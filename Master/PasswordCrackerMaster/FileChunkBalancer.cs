using System;
using System.Collections.Generic;
using System.IO;

namespace PasswordCrackerMaster
{
    public class FileChunkBalancer
    {
        private List<string> PasswordsWithUsers = new List<string>();
        public FileChunkBalancer(string dictionaryPath)
        {
            using (FileStream fs = new FileStream(dictionaryPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader ReadingStream = new StreamReader(fs))
                {
                    while (!ReadingStream.EndOfStream)
                    {
                        string Entry = ReadingStream.ReadLine();
                        
                      
                            PasswordsWithUsers.Add(Entry);




                    }
                    Console.WriteLine(PasswordsWithUsers[1]);
                    Console.WriteLine(PasswordsWithUsers[2]);
                    Console.WriteLine(PasswordsWithUsers[3]);

                    Console.WriteLine(PasswordsWithUsers.Count);
                    Console.ReadLine();
                }
            }

          
        }
    }
}