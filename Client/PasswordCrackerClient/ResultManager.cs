using System.Collections.Generic;

namespace PasswordCrackerClient
{
    public  class ResultManager
    {
        private  Dictionary<string, string> _partialResult = new Dictionary<string, string>();
        static ResultManager()
        {
            
        }

        public  void AddResultStrings(string user, string password)
        {
            _partialResult.Add(user, password);
        }

        public  Dictionary<string, string> GetPartialResult()
        {
            return _partialResult;
        }

        public void AddResultDic(Dictionary<string, string> dic)
        {
            foreach (var kvp in dic)
            {
            _partialResult.Add(kvp.Key, kvp.Value);

            }
        }


    }
}