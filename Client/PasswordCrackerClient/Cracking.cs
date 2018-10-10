using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using PasswordCrackerClient.model;

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
            StartCrack();
        }

        public void StartCrack()
        {


            Console.WriteLine("vi skal hanse den");
            CheckNormalWord("flower", _wordDic);
            

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

        private Dictionary<string, string> CheckSingleWord(Dictionary<string, string> users, string possiblePassword)
        {
            Console.WriteLine("check single");
            Converter converter = new Converter();
            Dictionary<string, string> Result = new Dictionary<string, string>();
            foreach (var user in users)
            {
                Task<bool> compared = CompareBytes(converter.ToByte(user.Value), converter.Sha1Convert(possiblePassword));
                compared.Wait();
                if (compared.Result)
                {
                    Result.Add(user.Key,possiblePassword);
                    return Result;
                }
                
            }

            return Result;

        }

        private async Task<bool> CompareBytes(byte[] userValue, byte[] toByte)
        {
            Console.WriteLine("compare");
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