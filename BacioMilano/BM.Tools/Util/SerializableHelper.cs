using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace BM.Util
{
    public class SerializableHelper
    {
        private static BinaryFormatter _binaryFormatter = new BinaryFormatter();

        public static void Serialize<T>(T obj, IFormatter formatter, string path) 
        {
            try
            {
                using (Stream stream = new FileStream(path, FileMode.Create,
                FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, obj);
                    stream.Close();
                }
            }
            catch (FieldAccessException ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
            catch (SerializationException ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
        }

        public static byte[] Serialize<T>(T obj, IFormatter formatter)
        {
            try
            {
                byte[] datas = null;
                using (Stream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, obj);
                    datas = new byte[stream.Length];
                    stream.Write(datas, 0, datas.Length);
                    stream.Close();
                }
                return datas;
            }
            catch (SerializationException ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
        }

        public static byte[] BinarySerialize<T>(T obj)
        {
            return Serialize(obj, _binaryFormatter);
        }

        public static void BinarySerialize<T>(T obj, string path)
        {
            Serialize(obj, _binaryFormatter, path);
        }

        public static T Deserialize<T>(string path, IFormatter formatter)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return (T)formatter.Deserialize(fs);
                }
            }
            catch (FieldAccessException ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
            catch (SerializationException ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
        }

        public static T Deserialize<T>(byte[] datas, IFormatter formatter)
        {
            try
            {
                using (MemoryStream fs = new MemoryStream(datas))
                {
                    return (T)(formatter.Deserialize(fs));
                }
            }
            catch (SerializationException ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.LogHelper<SerializableHelper>.GetLogger().Error(ex.Message, ex);
                throw ex;
            }
        }

        public static T BinaryDeserialize<T>(byte[] datas)
        {
            return Deserialize<T>(datas, _binaryFormatter);
        }

        public static T BinaryDeserialize<T>(string path)
        {
            return Deserialize<T>(path, _binaryFormatter);
        }



        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }
        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }
        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }
        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }
    }
}
