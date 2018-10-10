using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerClient
{
    class SendDataToMaster
    {
        private Stream _stream;

        public SendDataToMaster(Stream stream)
        {
            _stream = stream;
        }


        
        public void DataSendToMaster(Dictionary<string, string> dataDictionary)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(_stream,dataDictionary);
        }
    }
}
