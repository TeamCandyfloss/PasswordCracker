using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

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
        public void SendData(Dictionary<string, string> data)
        {
            BinaryFormatter v = new BinaryFormatter();
            v.Serialize(_stream, data);
            

        }


    }
}