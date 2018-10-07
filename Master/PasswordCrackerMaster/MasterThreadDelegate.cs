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

            string message = sr.ReadLine();
            string answer = "";
            while (message != null && message != "")
            {
                message = sr.ReadLine();
                if (message == "1")
                {
                    sw.WriteLine(FileChunkBalancer.GetChunk(10000));
                }
                if (message == "2")
                {
                    sw.WriteLine(FileChunkBalancer.GetChunk(10000));
                    sw.WriteLine();
                }

                if (message == "3")
                {
                    DataToSend data = new DataToSend();
                    data.Dictionary = new Dictionary<string, string>();
                    var formatter = new BinaryFormatter();

                    formatter.Serialize(ns, data);
                }
                Console.WriteLine("Client: " + message);
                message = "99999";
                


            }
            ns.Close();
            _connectionSocket.Close();
        }


    }

}
