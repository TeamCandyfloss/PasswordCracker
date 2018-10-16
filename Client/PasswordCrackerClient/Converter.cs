using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
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
            char[] charArray;
            charArray = ConvertMePlease.ToCharArray();
            byte[] bytearray = Array.ConvertAll(charArray, Convert.ToByte);
            return bytearray;
        }

        public byte[] ConvertSha1(byte[] password)
        {
            return sha1.ComputeHash(password);
        }



        public byte[] Base64Decrypter(string password)
        {
            return Convert.FromBase64String(password);
        }
    }
}