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
            BinaryFormatter formater = new BinaryFormatter();
            streamWriter.WriteLine("1");

            string serverResponse = streamReader.ReadLine();
            string[] unsplitString = serverResponse.Split(' ');

            Dictionary<string, string> UsersToCrack = (Dictionary<string, string>) formater.Deserialize(networkStream);
            

            string fromRange = unsplitString[0];
            string toRange = unsplitString[1];
            Console.WriteLine($"Hello Agent, your range is from {fromRange} to {toRange}");

            Cracking test = new Cracking($"{fromRange} {toRange}", UsersToCrack, _wordList);
            test.StartCrack();

            if (serverResponse == "666")
            {
                _isWorking = false;
                networkStream.Close();
                clientSocket.Close();
            }
        }

        void RequestWork(int resultCode)
        {
          

        }

        void SendResult()
        {
            //TODO: Implement code here.
        }

    }
}
