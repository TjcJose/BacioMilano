using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using BM.Cache;
using BM.Util;
using BM.Log;
using System.Xml;

namespace BM.Config
{
    /// <summary>
    /// 文件配置管理基类
    /// </summary>
    public static class ConfigFileManager
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object m_lockHelper = new object();

        /// <summary>
        /// 获得配置信息类
        /// </summary>
        /// <param name="configFilePath">配置文件所在路径</param>
        /// <param name="isCache">是否应用缓存</param>
        /// <returns>配置信息类</returns>
        public static T Instance<T>(string configFilePath, bool isCache = true) where T : IConfigInfo, new()
        {
            return (T)(LoadConfig<T>(configFilePath, isCache));
        }

        /// <summary>
        /// 移除配置信息
        /// </summary>
        /// <param name="configFilePath">配置文件所在路径</param>
        public static void RemoveConfig(string configFilePath)
        {
            string key = configFilePath.ToLower();
            var service = CacheServiceProviderHelper<EnterpriseLibraryCacheServiceProvider>.Instance.GetCacheService();
            if (service.Contains(key))
            {
                lock (m_lockHelper)
                {
                    if (service.Contains(key))
                    {
                        service.Remove(key);
                    }
                }
            }
        }
      
        /// <summary>
        /// 加载(反序列化)指定对象类型的配置对象
        /// </summary>
        /// <param name="configFilePath">配置文件所在路径(包括文件名)</param>
        /// <param name="configinfo">相应的变量 注:该参数主要用于设置m_configinfo变量 和 获取类型.GetType()</param>
        /// <param name="isCache">是否应用缓存</param>
        /// <returns></returns>
        static T LoadConfig<T>(string configFilePath, bool isCache = true) where T : IConfigInfo, new()
        {
            T configinfo;
            string key = configFilePath.ToLower();
            if (isCache)
            {
                configinfo = CacheCallHelper<T, EnterpriseLibraryCacheServiceProvider>.CacheFunRun(configFilePath.ToLower(),
                 delegate
                 {
                   
                     return SerializableHelper.XmlDeserializeFromFile<T>(configFilePath, System.Text.Encoding.UTF8);
                 }, CachingExpirationTypes.UsualSingleObject, m_lockHelper);
            }
            else
            {
                lock (m_lockHelper)
                {
                    var service = CacheServiceProviderHelper<EnterpriseLibraryCacheServiceProvider>.Instance.GetCacheService();
                    configinfo = SerializableHelper.XmlDeserializeFromFile<T>(configFilePath, System.Text.Encoding.UTF8);
                    service[key] = configinfo;
                }
            }
            return configinfo;
        }

        /// <summary>
        /// 反序列化指定的类
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="configFilePath">指定的配置文件所在的路径(包括文件名)</param>
        /// <returns></returns>
        static T DeserializeInfo<T>(string configFilePath) where T : IConfigInfo, new()
        {
            return SerializableHelper.XmlDeserializeFromFile<T>(configFilePath, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 保存(序列化)指定路径下的配置文件
        /// </summary>
        /// <param name="configFilePath">指定的配置文件所在的路径(包括文件名)</param>
        /// <param name="configinfo">被保存(序列化)的对象</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveConfig<T>(string configFilePath, T configinfo) where T : IConfigInfo, new()
        {
            bool sucess = false;
            try
            {
                SerializableHelper.XmlSerializeToFile(configinfo, configFilePath, System.Text.Encoding.UTF8);
                sucess = true;
            }
            catch (Exception ex)
            {
                LogHelper<T>.GetLogger().Error(ex.Message, ex);
            }

            return sucess;
        }
    }
}
