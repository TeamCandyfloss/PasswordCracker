using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordCrackerClient
{
    public class Cracking
    {
        private Dictionary<string, string> _wordDic;
        private string _interval = "";
        object _CompareLock = new object();
        public Cracking(string interval, Dictionary<string, string> dic)
        {
            _wordDic = dic;
            _interval = interval;
        }

        public async Task StartCrack()
        {


            Console.WriteLine("vi skal hanse den");
            await CheckNormalWord("Flower", _wordDic);


        }

        private async Task<Dictionary<string, string>> CheckNormalWord(string entry, Dictionary<string, string> users)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            Console.WriteLine("check normal");

            string possiblePassword = entry;
            results = CheckSingleWord(users, possiblePassword);
            
            //lasses kode.
            return results;

        }

        private  Dictionary<string, string> CheckSingleWord(Dictionary<string, string> users, string possiblePassword)
        {
            Converter converter = new Converter();
            Dictionary<string, string> Result = new Dictionary<string, string>();
            foreach (var user in users)
            {
                byte[] userShaByte = converter.ToByte(user.Value);
                byte[] possiblePasswordShaByte = converter.ToByte(possiblePassword);
                byte[] testshit = converter.ConvertSha1(possiblePasswordShaByte);

                bool compared = CompareBytes(userShaByte, testshit);
                if (compared)
                {
                    Result.Add(user.Key,possiblePassword);
                    Console.WriteLine($"added {user.Value}");
                    return Result;
                }

                
            }

            return Result;

        }

        private  bool CompareBytes(byte[] userValue, byte[] toByte)
        {
            lock (_CompareLock)
            {


                if (userValue.Length != toByte.Length)
                {
                    return false;
                }

                for (int i = 0; i < userValue.Length; i++)
                {
                    if (userValue[i] != toByte[i])
                        return false;
                }

                return true;
            }
        }
    }
}