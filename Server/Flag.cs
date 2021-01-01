using System;

namespace UServer
{
    [Serializable]
    public class Flag
    {
        /// <summary>
        /// The name of the command that will be called
        /// </summary>
        public string name;
        /// <summary>
        /// The params that will be pass to the method when it is called
        /// </summary>
        internal string[] args = new string[0];
    }
}