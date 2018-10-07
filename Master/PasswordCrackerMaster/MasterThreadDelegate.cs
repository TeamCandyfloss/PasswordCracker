using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerMaster
{
    class MasterThreadDelegate
    {

        private TcpClient _connectionSocket;
        public MasterThreadDelegate(TcpClient connectionSocket)
        {

            _connectionSocket = connectionSocket;

        }

        public void Start()
        {
            
            //Socket connectionSocket = serverSocket.AcceptSocket();
           
            Stream ns = _connectionSocket.GetStream();
            // Stream ns = new NetworkStream(connectionSocket);

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = " ";
            while (message != null && message != "")
            {
                message = sr.ReadLine();
                switch (message)
                {
                    case "1":
                        Console.WriteLine("case 1");
                        sw.WriteLine(FileChunkBalancer.GetChunk(10000));
                        break;
                    case "2":
                        Console.WriteLine("case 2");
                        sw.WriteLine(FileChunkBalancer.GetChunk(10000));
                        break;
                    case "3":
                        Console.WriteLine("case 3");
                        DataToSend data = new DataToSend(ns);
                        data.SendData(new PasswordFileHandler("passwords.txt").GetHashes());
                        break;
                }

                Console.WriteLine("Client: " + message);
                message = "99999";
                


            }
            ns.Close();
            _connectionSocket.Close();
        }


    }

}
