using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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

            string message = sr.ReadLine();
            string answer = "";
            while (message != null && message != "")
            {

                if (message == "1")
                {
                    sw.WriteLine(FileChunkBalancer.GetChunk(10000)); 
                }
                Console.WriteLine("Client: " + message);
                message = "99999";
                message = sr.ReadLine();


            }
            ns.Close();
            _connectionSocket.Close();
        }


    }

}
