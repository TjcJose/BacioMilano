using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BM.Net
{
   public static class NetTools
    {
       public static IPAddress[] GetLocalIP()
       {
           string strHostName = Dns.GetHostName();   //得到本机的主机名
           IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
           return ipEntry.AddressList;
       }

       public static bool CheckIP(string ipAddress)
       {
           if(String.IsNullOrEmpty(ipAddress))
           {
               return false;
           }

           string[] arr = ipAddress.Split('.');
           if (arr.Length != 4)
           {
               return false;
           }

           foreach (string s in arr)
           {
               int i;
               if (int.TryParse(s, out i))
               {
                   if (i < 0 || i > 255)
                   {
                       return false;
                   }

               }
               else
               {
                   return false;
               }
           }

           return true;

       }
    }
}
