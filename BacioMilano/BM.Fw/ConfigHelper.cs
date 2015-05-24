using BM.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Fw
{
    public class ConfigHelper
    {
        private static string config_path;
        static ConfigHelper()
        {
            string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            config_path = init<Config>(folderPath);
        }

        static string init<T>(string folderPath) where T : IConfigInfo, new() 
        {
            string path = Path.Combine(folderPath, typeof(T).Name + ".xml"); 
            if (!File.Exists(path))
            {
                ConfigFileManager.SaveConfig(path, new T());
            }
            return path;
        }


        public static Config Config_Instance
        {
            get
            {
                return ConfigFileManager.Instance<Config>(config_path);
            }
        }
    }
}
