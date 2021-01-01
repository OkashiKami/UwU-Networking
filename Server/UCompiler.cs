using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UServer
{
    internal class UCompiler
    {
        public static Assembly Compile(List<string> files)
        {

            for (int i = 0; i < files.Count; i++)
            {
                if (!File.Exists(files[i])) continue;

                files[i] = new FileInfo(files[i]).FullName;
            }
            


           

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("UServer.exe");
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;
            
            CompilerResults results = provider.CompileAssemblyFromFile(parameters, files.ToArray());

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }
                throw new InvalidOperationException(sb.ToString());
            }
            Assembly assembly = null;
            assembly = results.CompiledAssembly;
            if (assembly != null) return assembly;
            return default;
        }
        public  static Assembly Compile(string actionName)
        {
            var file = actionName;
            if (!File.Exists(file)) return default;
            file = new FileInfo(file).FullName;



            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("UServer.exe");
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;

            CompilerResults results = provider.CompileAssemblyFromFile(parameters, file);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }
                throw new InvalidOperationException(sb.ToString());
            }
            Assembly assembly = null;
            assembly = results.CompiledAssembly;
            if (assembly != null) return assembly;
            return default;
        }


        public static UMethodInfo GetMethod(Assembly assembly, Command command, string cmd, params object[] args)
        {
            if (assembly == null) return default;

            var name = command.name;
            Type program = assembly.GetType($"UServer.{name}");
            MethodInfo method = program.GetMethod(cmd);
            return new UMethodInfo(program, method, args) ?? default;
        }
    }
}