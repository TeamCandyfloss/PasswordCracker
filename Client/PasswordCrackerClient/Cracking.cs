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
        private int _startDigitCounter = 0;
        private int _endDigitCounter = 0;
        private int _startEndDigitCounter = 0;
        private string _wordHack2;
        private string _wordHack3;

        // Streamwriter + path, til at oprette vores DebugLog.txt, som vil have de fleste beskeder i loggen. 
        private StreamWriter _logWriter;
        private string _path = @"DebugLog.txt";

        object _CompareLock = new object();
        private List<string> _wordList;

        public Cracking(List<string> WordList)
        {
            _wordList = WordList;
            _resultManager = new ResultManager();

            // DebugLog writer bliver instantieret. 
            _logWriter = new StreamWriter(_path, true);

            _logWriter.WriteLine("--------------CRACKING STARTED--------------");
            Console.WriteLine("--------------CRACKING STARTED--------------");

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

            Console.WriteLine("Requesting more work..");

            return _resultManager.GetPartialResult();

        }

        private async Task CheckNormalWord(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking for normal words.");
            Console.WriteLine($"Log {DateTime.Now} - Checking for normal words.");

            foreach (var word in WordList)
            {
                await Task.Run(() => CheckSingleWord(users, word));
            }


            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking for normal words.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking for normal words.");
        }

        private async Task CheckUpperWord(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking for Uppercased words.");
            Console.WriteLine($"Log {DateTime.Now} - Checking for Uppercased words.");

            foreach (var word in WordList)
            {
                await Task.Run(() => CheckSingleWord(users, word.ToUpper()));
            }

            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking for uppercased words.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking for uppercased words.");
        }

        private async Task CheckCapitalWord(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking for capitalized words.");
            Console.WriteLine($"Log {DateTime.Now} - Checking for capitalized words.");

            foreach (var word in WordList)
            {
                string capatalizedEntry = StringUtil.Capitalize(word);
                await Task.Run(() => CheckSingleWord(users, capatalizedEntry));
            }

            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking for capitalized words.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking for capitalized words.");
        }

        private async Task CheckReverseWord(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking reverse words.");
            Console.WriteLine($"Log {DateTime.Now} - Checking reverse words.");

            foreach (var word in WordList)
            {
                string reversedEntry = StringUtil.Reverse(word);
                await Task.Run(() => CheckSingleWord(users, reversedEntry));
            }

            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking reverse words.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking reverse words.");
        }

        private async Task CheckEndDigitWord(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking for end digits.");
            Console.WriteLine($"Log {DateTime.Now} - Checking for end digits.");

            foreach (var word in WordList)
            {
                _wordHack3 = word;
                Parallel.For(0, 100, EndDigitWordLoop);
            }

            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking for end digits.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking for end digits.");
            _endDigitCounter = 0;

        }

        private async void EndDigitWordLoop(int x)
        {

            string possiblePasswordEndDigit = _wordHack3 + x;

            await Task.Run(() => CheckSingleWord(_UsersToCrack, possiblePasswordEndDigit));

            _endDigitCounter++;
            switch (_endDigitCounter)
            {
                case 250000:
                    Console.WriteLine($"Log {DateTime.Now} - 25% Done End Digits");
                    break;
                case 500000:
                    Console.WriteLine($"Log {DateTime.Now} - 50% Done End Digits");
                    break;
                case 750000:
                    Console.WriteLine($"Log {DateTime.Now} - 75% Done End Digits");
                    break;
            }



        }
        private async Task CheckStartDigit(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking for start digits.");
            Console.WriteLine($"Log {DateTime.Now} - Checking for start digits.");

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

            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking for start digits.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking for start digits.");
            _startDigitCounter = 0;

        }

        private async void LoopStartDigit(int x)
        {
            //super hacky cowboy kode.

            string possiblePasswordStartDigit = x + _wordHack;
            await Task.Run(() => CheckSingleWord(_UsersToCrack, possiblePasswordStartDigit));
            _startDigitCounter++;
            switch (_startDigitCounter)
            {
                case 250000:
                    Console.WriteLine($"Log {DateTime.Now} - 25% Done Start Digits");
                    break;
                case 500000:
                    Console.WriteLine($"Log {DateTime.Now} - 50% Done Start Digits");
                    break;
                case 750000:
                    Console.WriteLine($"Log {DateTime.Now} - 75% Done Start Digits");
                    break;
            }



        }
        private async Task CheckStartEndDigit(List<string> WordList, Dictionary<string, string> users)
        {

            _logWriter.WriteLine($"Log {DateTime.Now} - Checking for start AND end digits.");
            Console.WriteLine($"Log {DateTime.Now} - Checking for start AND end digits.");

            foreach (var word in WordList)
            {
                _wordHack2 = word;
                for (int i = 0; i < 10; i++)
                {
                    _hack1 = i;
                    Parallel.For(0, 10, EndStartDigitLoop);
                }
            }

            _logWriter.WriteLine($"Log {DateTime.Now} - Done checking for start and end digits.");
            Console.WriteLine($"Log {DateTime.Now} - Done checking for start and end digits.");
            _startEndDigitCounter = 0;
        }

        private async void EndStartDigitLoop(int x)
        {
            string possiblePasswordStartEndDigit = _hack1 + _wordHack2 + x;
            await Task.Run(() => CheckSingleWord(_UsersToCrack, possiblePasswordStartEndDigit));
            _startEndDigitCounter++;
            switch (_startEndDigitCounter)
            {
                case 250000:
                    Console.WriteLine($"Log {DateTime.Now} - 25% Done End Start Digits");
                    break;
                case 500000:
                    Console.WriteLine($"Log {DateTime.Now} - 50% Done End Start Digits");
                    break;
                case 750000:
                    Console.WriteLine($"Log {DateTime.Now} - 75% Done End Start Digits");
                    break;
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

                    _resultManager.AddResultStrings(user.Key, possiblePassword);
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