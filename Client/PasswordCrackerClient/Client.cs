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

        void RequestWork(int resultCode)
        {

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
            streamWriter.WriteLine("3");

            string serverResponse = streamReader.ReadLine();
            string[] unsplitString = serverResponse.Split(' ');

            Dictionary<string, string> testdic = (Dictionary<string, string>) formater.Deserialize(networkStream);
            Cracking test = new Cracking("0 10000", testdic);
            test.StartCrack();

            string fromRange = unsplitString[0];
            string toRange = unsplitString[1];
            Console.WriteLine($"Hello Agent, your range is from {fromRange} to {toRange}");

            if (!_moreWork)
            {
                _isWorking = false;
                networkStream.Close();
                clientSocket.Close();
            }
        }

        void CheckHash(string[] wordList, string[] passwords)
        {
            //TODO: Implement code here. 
        }

        void ReconstructHash(Byte[] hash)
        {
            //TODO: Implement code here.
        }

        void SendResult()
        {
            //TODO: Implement code here.
        }

    }
}
