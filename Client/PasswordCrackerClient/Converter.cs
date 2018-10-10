using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PasswordCrackerClient
{
    public class Converter
    {
        private SHA1CryptoServiceProvider sha1;
        private Dictionary<string, byte[]> _convertedDic;
        public Converter()
        {
            sha1 = new SHA1CryptoServiceProvider();
        }

        public Dictionary<string, byte[]> ConvertToByte(Dictionary<string, string> preConvertedDic)
        {

            foreach (var s in preConvertedDic)
            {
                byte[] bytes = Convert.FromBase64String(s.Value);
                _convertedDic.Add(s.Key, bytes);
            }

            return _convertedDic;
        }

        public byte[] ToByte(string ConvertMePlease)
        {
             byte[] converted = Convert.FromBase64String(ConvertMePlease);
            return converted;
        }

        public byte[] Sha1Convert(string password)
        {
            return sha1.ComputeHash(ToByte(password));
        }
    }
}