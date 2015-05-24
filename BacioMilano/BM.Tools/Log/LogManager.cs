using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace BM.Log
{
    public static class LogManager
    {
       public static ILog Log(string name)
       {
           return new Log(name);
       }

       public static ILog Log(Type type)
       {
           return new Log(type);
       }

       public static ILog Log(string repository, string name)
       {
           return new Log(repository, name);
       }

       public static ILog Log(string repository, Type type)
       {
           return new Log(repository, type);
       }

       public static ILog Log(Assembly repositoryAssembly, Type type)
       {
           return new Log(repositoryAssembly, type);
       }

       public static ILog Log(Assembly repositoryAssembly, string name)
       {
           return new Log(repositoryAssembly, name);
       }

       public static ILog Log<Type>()
       {
           return new Log(typeof(Type));
       }

       public static ILog Log<Type>(string repository)
       {
           return new Log(repository, typeof(Type));
       }

       public static void InitLogConfig()
       {
           log4net.Config.XmlConfigurator.Configure();
       }

       public static void InitLogConfig(string filePath)
       {
           FileInfo fi = new FileInfo(filePath);
           log4net.Config.XmlConfigurator.Configure(fi);
       }
    }
}
