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
        private Dictionary<string, string> _UsersToCrack;
        private string[] _interval;
        private ResultManager _resultManager;


        object _CompareLock = new object();
        private List<string> _wordList;

        public Cracking( List<string> WordList)
        {
            
            _wordList = WordList;
            _resultManager = new ResultManager();
        }

        public Dictionary<string, string> StartCrack(string interval, Dictionary<string, string> UsersToCrack)
        {
            _UsersToCrack = UsersToCrack;
            _interval = interval.Split(' ');
            // nød til at lave en ny interval variable til GetRange metoden på listen, Se metode beskrivelse.
            int AmountOfElements = Int32.Parse(_interval[1]) - Int32.Parse(_interval[0]);
            //variable får hvor vi skal starte at hente ord i ordlisten til den ordliste vi bruger til at cracke med.
            int startRange = Int32.Parse(_interval[0]);
            // extracte de ord inden for det interval der er givet og kommer det i en ny liste.
            List<string> listToCrack = _wordList.GetRange(startRange, AmountOfElements);

            //checker hver ord i listen igennem forskellige "filter"
            Task doneNormal = CheckNormalWord(listToCrack, _UsersToCrack);
            Task doneUpper = CheckUpperWord(listToCrack, _UsersToCrack);
            Task doneCap = CheckCapitalWord(listToCrack, _UsersToCrack);
            Task doneReverse = CheckReverseWord(listToCrack, _UsersToCrack);
            Task doneStartDigit = CheckStartDigit(listToCrack, _UsersToCrack);
            Task doneEndDigit = CheckEndDigitWord(listToCrack, _UsersToCrack);
            Task doneStartEndDigit = CheckStartEndDigit(listToCrack, _UsersToCrack);

            Task.WaitAll(doneNormal, doneUpper, doneCap, doneReverse, doneStartDigit, doneEndDigit, doneStartEndDigit);

            Console.WriteLine("er vi færdige?");

            return _resultManager.GetPartialResult();

        }

        private async Task CheckNormalWord(List<string> WordList, Dictionary<string, string> users)
        {
            int test = 0;
           

            foreach (var word in WordList)
            {
                test++;
                await Task.Run(() => CheckSingleWord(users, word));
                Console.WriteLine($"check normal {test}");
            }


        }

        private async Task CheckUpperWord(List<string> WordList, Dictionary<string, string> users)
        {
            foreach (var word in WordList)
            {
               // Console.WriteLine($"check upper");
                await Task.Run(() => CheckSingleWord(users, word.ToUpper()));
            }

        }

        private async Task CheckCapitalWord(List<string> WordList, Dictionary<string, string> users)
        {
            foreach (var word in WordList)
            {
                //Console.WriteLine($"check cap");
                string capatalizedEntry = StringUtil.Capitalize(word);
                await Task.Run(() => CheckSingleWord(users, capatalizedEntry));
            }
           
        }

        private async Task CheckReverseWord(List<string> WordList, Dictionary<string, string> users)
        {
            foreach (var word in WordList)
            {
                // Console.WriteLine($"check rev");
                string reversedEntry = StringUtil.Reverse(word);
                await Task.Run(() => CheckSingleWord(users, reversedEntry));
            }
           
        }

        private async Task CheckEndDigitWord(List<string> WordList, Dictionary<string, string> users)
        {
            foreach (var word in WordList)
            {
                for (int i = 0; i < 100; i++)
                {
                    string possiblePasswordEndDigit = word + i;
                    await Task.Run(() => CheckSingleWord(users, possiblePasswordEndDigit));
                }
            }
           
        }
        private async Task CheckStartDigit(List<string> WordList, Dictionary<string, string> users)
        {
            foreach (var word in WordList)
            {
                for (int i = 0; i < 100; i++)
                {
                    string possiblePasswordStartDigit = i + word;
                    await Task.Run(() => CheckSingleWord(users, possiblePasswordStartDigit));
                }
            }

        }
        private async Task CheckStartEndDigit(List<string> WordList, Dictionary<string, string> users)
        {
            int test = 0;
            foreach (var word in WordList)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        string possiblePasswordStartEndDigit = i + word + j;
                        Console.WriteLine($"check tal {test}");
                        test++;
                        await Task.Run(() => CheckSingleWord(users, possiblePasswordStartEndDigit));
                    }
                }
            }

        }





        private void CheckSingleWord(Dictionary<string, string> users, string possiblePassword)
        {
            Converter converter = new Converter();
            foreach (var user in users)
            {
                byte[] userShaByte = converter.Base64Decrypter(user.Value);
                byte[] possiblePasswordShaByte = converter.ToByte(possiblePassword);
                byte[] passwordSha = converter.ConvertSha1(possiblePasswordShaByte);

                // der kan skabes endnu en tråd her, så den kører comparen i en ny tråd hvergang istedet for at låse comparen. lad os teste det senere.
                bool compared = CompareBytes(userShaByte, passwordSha);
                if (compared)
                {
                    
                    _resultManager.AddResultStrings(user.Key,possiblePassword);
                    Console.WriteLine($"added {user.Value}");
                   // return true;

                }
            }

            // return false; ;
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