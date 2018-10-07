using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PasswordCrackerMaster
{
    
    public class DataToSend
    {
        
        private Stream _stream;
        public DataToSend(Stream stream)
        {
            
            _stream = stream;
        }
        //public Dictionary<string,string> Dictionary { get => _dictionaryToSend; }

        /// <summary>
        /// Data der bliver sendt skal have et [Serializable] TAG før klassen hvis det en custom class for at kunne sendes.  Næsten alle .Net klasser kan sendes uden ændringer.
        /// </summary>
        public void SendData(object data)
        {
            var formatter = new BinaryFormatter();

            formatter.Serialize(_stream, data);
        }


    }
}