using System;
using System.Collections.Generic;
using System.Text;

namespace UServer
{
    [Serializable]
    public class Command
    {
        /// <summary>
        /// The name of the script that the command will be called on
        /// </summary>
        internal string name;

        /// <summary>
        /// The short name of the action 
        /// </summary>
        public string prefix { get; set; }
        /// <summary>
        /// The methods that are in the class
        /// </summary>
        internal List<Flag> flags { get; set; } = new List<Flag>();
        /// <summary>
        /// The path to the cs fiel that will be executed
        /// </summary>
        public string pathToCode { get; set; }
        public Command() { }

        internal object[] Args(string cmd, List<string> parts)
        {
            List<KVP> keyValuePairs = new List<KVP>();

            foreach (var item in parts)
            {
                if (item.Contains(":"))
                {
                    var part1 = item.Split(':')[0];
                    var part2 = item.Split(':')[1];

                    var flag = flags.Find(x => x.name.ToLower() == cmd.ToLower());

                    foreach (var item2 in flag.args)
                    {
                        if(item2.StartsWith(part1))
                            keyValuePairs.Add(new KVP(part1, part2));
                    }
                }
                else
                {
                    var flag = flags.Find(x => x.name.ToLower() == cmd.ToLower());
                    foreach (var item2 in flag.args)
                    {
                        if(item2.StartsWith(item))
                            keyValuePairs.Add(new KVP(item));
                    }
                }
            }

            return keyValuePairs.ToArray();
        }
    }


    public class KVP
    {
        private string _key;
        private string _value;

        public string Key
        {
            set
            {
                _key = value;
                var firstletter = _key.ToCharArray()[0].ToString().ToUpper();
                var sb = new StringBuilder();
                for (int i = 1; i < _key.ToCharArray().Length; i++)
                    sb.Append(_key.ToCharArray()[i]);
                _key = $"{firstletter }{sb.ToString()}";
            }
            get
            {
                var key = _key != null ? _key : _value;
                var firstletter = key.ToCharArray()[0].ToString().ToUpper();
                var sb = new StringBuilder();
                for (int i = 1; i < key.ToCharArray().Length; i++)
                    sb.Append(key.ToCharArray()[i]);
                key = $"{firstletter }{sb}";
                return key;
            }
        }
        public string Value
        {
            set
            {
                _value = value;
            }
            get
            {
               return _value != null ? _value : null;
            }
        }


        public KVP(string value)
        {
            this._key = null;
            this._value = value;
        }

        public KVP(string key, string value)
        {
            this._key = key;
            this._value = value;
        }
    }
}