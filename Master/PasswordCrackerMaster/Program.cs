using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PasswordCrackerMaster
{
    class Program
    {
        private static bool _StartTimer = true;
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(6789);

            //TcpListener serverSocket = new TcpListener(6789);
            serverSocket.Start();
            Console.WriteLine("Server activated now");
            


            while (true)
            {
                TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                
                Console.WriteLine($"{LogHandler.GetUsers()} connected");
                MasterThreadDelegate service = new MasterThreadDelegate(connectionSocket, 10000);

                Task.Factory.StartNew(() => service.Start());
                if (_StartTimer)
                {
                    LogHandler.StartStopWatch();
                    _StartTimer = false;
                }
              
            }


            
          
           

            
        }
    }
}
