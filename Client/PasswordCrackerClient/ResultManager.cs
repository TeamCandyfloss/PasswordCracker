
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerClient
{
    static class ResultManager
    {
        private static Dictionary<string, string> result = new Dictionary<string, string>();
        private static object resultLock = new  object();
        

        public static void AddResult(string partialResult)
        {
            lock (resultLock)
            {
                string[] splitResult = partialResult.Split(':');
                result.Add(splitResult[0], splitResult[1]);
            }
        }

        public static Dictionary<string, string> ReturnResult()
        {
            return result;
        }


        


    }
}
