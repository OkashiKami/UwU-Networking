using System;
using System.Collections.Generic;
using System.Reflection;

namespace UServer
{
    public class UMethodInfo
    {
        private Type target;
        private MethodInfo method;
        private object[] args;

        public UMethodInfo(Type program, MethodInfo main, params object[] args)
        {
            this.target = program;
            this.method = main;
            this.args = args;
        }

        internal object Invoke()
        {

            var classStart = Activator.CreateInstance(target);
            var result = method.Invoke(classStart, args);
            return result;
        }
    }
}