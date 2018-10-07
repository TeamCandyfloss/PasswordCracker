using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerMaster
{
    class Program
    {
        static void Main(string[] args)
        {

            TcpListener serverSocket = new TcpListener(6789);

            //TcpListener serverSocket = new TcpListener(6789);
            serverSocket.Start();
            Console.WriteLine("Server activated now");


            while (true)
            {

                TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("Client Connected");
                MasterThreadDelegate service = new MasterThreadDelegate(connectionSocket);

                Task.Factory.StartNew(() => service.Start());
            }


            
          
           

            
        }
    }
}
