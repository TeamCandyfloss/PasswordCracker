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
        private string _wordHack;
        private int _hack1;
        private string _wordHack2;
        private string _wordHack3;

        object _CompareLock = new object();
        private List<string> _wordList;

        public Cracking( List<string> WordList)
        {
            
            _wordList = WordList;
            _resultManager = new ResultManager();
        }

        public Dictionary<string, string> StartCrack(string interval, Dictionary<string, string> UsersToCrack)
        {
            _resultManager.ClearEntrys();
            _UsersToCrack = UsersToCrack;
            _interval = interval.Split(' ');
           

            // nød til at lave en ny interval variable til GetRange metoden på listen, Se metode beskrivelse.
           
            int AmountOfElements = Int32.Parse(_interval[1]) - Int32.Parse(_interval[0]);
            
            //variable får hvor vi skal starte at hente ord i ordlisten til den ordliste vi bruger til at cracke med.
            int startRange = Int32.Parse(_interval[0]);
            if (startRange > Int32.Parse(_interval[1]))
            {
                AmountOfElements = Int32.Parse(_interval[1]);
            }
            
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

            Console.WriteLine("checknormal");
            foreach (var word in WordList)
            {
                Console.WriteLine(test);
                test++;
                await Task.Run(() => CheckSingleWord(users, word));
            }

            Console.WriteLine("normal Done");


        }

        private async Task CheckUpperWord(List<string> WordList, Dictionary<string, string> users)
        {
            Console.WriteLine("checkupper");
            foreach (var word in WordList)
            {
               // Console.WriteLine($"check upper");
                await Task.Run(() => CheckSingleWord(users, word.ToUpper()));
            }
            Console.WriteLine("upper Done");
        }

        private async Task CheckCapitalWord(List<string> WordList, Dictionary<string, string> users)
        {
            Console.WriteLine("checkcap");
            foreach (var word in WordList)
            {
                //Console.WriteLine($"check cap");
                string capatalizedEntry = StringUtil.Capitalize(word);
                await Task.Run(() => CheckSingleWord(users, capatalizedEntry));
            }
            Console.WriteLine("cap Done");
        }

        private async Task CheckReverseWord(List<string> WordList, Dictionary<string, string> users)
        {
            Console.WriteLine("checkrev");
            foreach (var word in WordList)
            {
                // Console.WriteLine($"check rev");
                string reversedEntry = StringUtil.Reverse(word);
                await Task.Run(() => CheckSingleWord(users, reversedEntry));
            }
            Console.WriteLine("rev Done");
        }

        private async Task CheckEndDigitWord(List<string> WordList, Dictionary<string, string> users)
        {
            Console.WriteLine("checkendigit");
            int test = 0;
            foreach (var word in WordList)
            {
                _wordHack3 = word;
                Parallel.For(0, 100, EndDigitWordLoop);
            }
            Console.WriteLine("enddigit Done");
        }

        private async void EndDigitWordLoop(int x)
        {
            
                string possiblePasswordEndDigit = _wordHack3 + x;
                Console.WriteLine(possiblePasswordEndDigit);
                
                await Task.Run(() => CheckSingleWord(_UsersToCrack, possiblePasswordEndDigit));
            
        }
        private async Task CheckStartDigit(List<string> WordList, Dictionary<string, string> users)
        {
            Console.WriteLine("checkstartdigit");
            int test = 0;
            foreach (var word in WordList)
            {
                //for (int i = 0; i < 100; i++)
                //{
                //    Console.WriteLine(test);
                //    test++;
                //    string possiblePasswordStartDigit = i + word;
                //    await Task.Run(() => CheckSingleWord(users, possiblePasswordStartDigit));
                //}
                _wordHack = word;
                Parallel.For(0, 100, LoopStartDigit);
            }
            Console.WriteLine("startDigit Done");
        }

        private async void LoopStartDigit(int x)
        {
            //super hacky cowboy kode.
            int test = 0;
           
                string possiblePasswordStartDigit = x + _wordHack;
                await Task.Run(() => CheckSingleWord(_UsersToCrack, possiblePasswordStartDigit));
            test++;



        }
        private async Task CheckStartEndDigit(List<string> WordList, Dictionary<string, string> users)
        {
            Console.WriteLine("checkstartEnd");
            int test = 0;
            foreach (var word in WordList)
            {
                _wordHack2 = word;
                for (int i = 0; i < 10; i++)
                {
                    _hack1 = i;
                    Parallel.For(0, 10, EndStartDigitLoop);
                }
            }
            Console.WriteLine("StartEnd Done");
        }

        private async void EndStartDigitLoop(int x)
        {
            string possiblePasswordStartEndDigit = _hack1 + _wordHack2 + x;
            Console.WriteLine(possiblePasswordStartEndDigit);
            await Task.Run(() => CheckSingleWord(_UsersToCrack, possiblePasswordStartEndDigit));
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

        private bool CompareBytes(byte[] userValue, byte[] toByte)
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