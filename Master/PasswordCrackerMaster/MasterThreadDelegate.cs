using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordCrackerMaster
{
    class MasterThreadDelegate
    {

        private TcpClient _connectionSocket;
        private int _interval = 10000;
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
                        sw.WriteLine(FileChunkBalancer.GetChunk(_interval));
                        LogHandler.SetGivenValue(_interval.ToString());
                        Console.WriteLine($"{Thread.CurrentThread.Name} is cracking interval {LogHandler.GetGivenValue()}");
                        break;
                    case "2":
                        Console.WriteLine("case 2");
                        LogHandler.DisconnectedUser();
                        Console.WriteLine($"{Thread.CurrentThread.Name} Disconnected");
                        break;
                    // case 3 sender in dictionary af hashes af hvem der ejer dem.
                    case "3":
                        Console.WriteLine("case 3");
                        sw.WriteLine("0 10000");
                        DataToSend data = new DataToSend(ns);
                        data.SendData(new PasswordFileHandler("passwords.txt").GetHashes());
                        break;
                    case "200":
                        //bruges hvis en client har fundet et password, hvorefter det blive tilføjet til passwordlisten.
                        BinaryFormatter formater = new BinaryFormatter();
                        Dictionary<string, string> partialResult = (Dictionary<string, string>) formater.Deserialize(ns);
                        ResultManager.AddResult(partialResult);
                        break;
                }

               
                message = "99999";
                


            }
            ns.Close();
            _connectionSocket.Close();
        }


    }

}
