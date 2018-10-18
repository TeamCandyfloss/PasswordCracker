using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerClient
{
    public class Client
    {
        private bool _moreWork = true;
        private bool _isWorking = false;
        private TcpClient clientSocket;
        private Stream networkStream;
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private static string _dictionaryPath = "webster.txt";
        private static List<string> _wordList = new List<string>();
        private ResultManager _resultManager = new ResultManager();
        BinaryFormatter _formater = new BinaryFormatter();


        public Client()
        {
            // læser Ordbogen og gemmer den i en local variable
            using (FileStream fs = new FileStream(_dictionaryPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader readingStream = new StreamReader(fs))
                {
                    while (!readingStream.EndOfStream)
                    {
                        string Entry = readingStream.ReadLine();

                        _wordList.Add(Entry);
                    }
                }
            }
        }
        

        public void Create(string ip, int port)
        {
            clientSocket = new TcpClient(ip, port);

            networkStream = clientSocket.GetStream();
            streamWriter = new StreamWriter(networkStream);

            streamWriter.AutoFlush = true;
            streamReader = new StreamReader(networkStream);

            _isWorking = true;

            Handshake();
            
        }

        public void Handshake()
        {
            streamWriter.WriteLine("1");
            
            string serverResponse = streamReader.ReadLine();

            if (serverResponse == "666")
            {
                streamWriter.WriteLine("2");
                _isWorking = false;
                networkStream.Close();
                clientSocket.Close();
            }

            string[] unsplitString = serverResponse.Split(' ');

            Dictionary<string, string> UsersToCrack = (Dictionary<string, string>)_formater.Deserialize(networkStream);
            

            string fromRange = unsplitString[0];
            string toRange = unsplitString[1];
            Console.WriteLine($"Hello Agent, your range is from {fromRange} to {toRange}");

            Cracking cracker = new Cracking(_wordList);
            Dictionary<string,string> partialResult = cracker.StartCrack($"{fromRange} {toRange}", UsersToCrack);
            _resultManager.AddResultDic(partialResult);
            if (_resultManager.GetPartialResult().Keys.Count != 0)
            {
                 SendResult();
            }

            RequestWork(cracker);

            if (serverResponse == "666")
            {
                streamWriter.WriteLine("2");
                _isWorking = false;
                networkStream.Close();
                clientSocket.Close();
            }
        }

        void RequestWork(Cracking cracker)
        {
            
            // sender kode 3 for mere arbejde
            streamWriter.WriteLine("3");

            string serverResponse = streamReader.ReadLine();
            if (serverResponse == "666")
            {
                streamWriter.WriteLine("2");
                _isWorking = false;
                networkStream.Close();
                clientSocket.Close();
            }
            string[] unsplitString = serverResponse.Split(' ');
            string fromRange = unsplitString[0];
            string toRange = unsplitString[1];

            

            Console.WriteLine($"Hello Agent, your range is from {fromRange} to {toRange}");

            Dictionary<string, string> UsersToCrack = (Dictionary<string, string>)_formater.Deserialize(networkStream);

            Dictionary<string, string> partialResult = cracker.StartCrack($"{fromRange} {toRange}", UsersToCrack);
            _resultManager.AddResultDic(partialResult);

            if (partialResult.Keys.Count != 0)
            {
                SendResult();
                partialResult.Clear();
            }
            RequestWork(cracker);
        }

        void SendResult()
        {
            
            streamWriter.WriteLine("200");
           DataToSend data = new DataToSend(networkStream);
            data.SendData(_resultManager.GetPartialResult());
        }

    }
}
