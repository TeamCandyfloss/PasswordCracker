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
        private int _interval;
        private PasswordFileHandler _passwordFileHandler = new PasswordFileHandler("passwords.txt");
        public MasterThreadDelegate(TcpClient connectionSocket, int interval)
        {

            _connectionSocket = connectionSocket;
            _interval = interval;

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
                    // start case når en client først connecter
                    case "1":
                        LogHandler.ConnectedUser();
                        Console.WriteLine("case 1");
                        string interval = FileChunkBalancer.GetChunk(_interval);
                        sw.WriteLine(interval);
                        DataToSend data = new DataToSend(ns);
                        data.SendData(_passwordFileHandler.GetHashes());
                        LogHandler.SetGivenValue(interval);
                        Console.WriteLine($"{Thread.CurrentThread.Name} is cracking interval {LogHandler.GetGivenValue()} currently {LogHandler.GetUsers()} users cracking");
                        // TODO Mulighvis ændre så clienter sender et navn i stedet for masteren tildeler tråde navne.
                        break;
                    // når en client disconnecter
                    case "2":
                        Console.WriteLine("case 2");
                        LogHandler.DisconnectedUser();
                        Console.WriteLine($"{Thread.CurrentThread.Name} Disconnected {LogHandler.GetUsers()} left cracking");
                        break;
                    // Når en client skal havde mere arbejde.
                    case "3":
                        Console.WriteLine("case 1");
                        string interval1 = FileChunkBalancer.GetChunk(_interval);
                        sw.WriteLine(interval1);
                        DataToSend data1 = new DataToSend(ns);
                        data1.SendData(_passwordFileHandler.GetHashes());
                        LogHandler.SetGivenValue(interval1);
                        Console.WriteLine($"{Thread.CurrentThread.Name} is cracking interval {LogHandler.GetGivenValue()} currently {LogHandler.GetUsers()} users cracking");

                        break;
                    case "200":
                        //bruges hvis en client har fundet et password, hvorefter det blive tilføjet til passwordlisten.
                        BinaryFormatter formater = new BinaryFormatter();
                        Dictionary<string, string> partialResult = (Dictionary<string, string>) formater.Deserialize(ns);
                        ResultManager.AddResult(partialResult);
                        ResultManager.ShowResult();
                        break;
                }

               
                message = "99999";
               


            }
            ns.Close();
            _connectionSocket.Close();
        }


    }

}
