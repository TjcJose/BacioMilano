using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BM.DA
{
    /// <summary>
    /// 实体描述帮助类
    /// </summary>
    public class ModelDescriptionHelper
    {
        private static object lockObj = new object();
        private static Dictionary<Type, ModelDescriptionHelper> dic = new Dictionary<Type, ModelDescriptionHelper>();
        public static ModelDescriptionHelper Get_ModelDescriptionHelper(Type type)
        {
            if (dic.ContainsKey(type))
            {
                return dic[type];
            }
            else
            {
                lock (lockObj)
                {
                    if (!dic.ContainsKey(type))
                    {
                        string className = GetClassName(type);
                        var typeUse = Type.GetType(className, true);

                        var m = new ModelDescriptionHelper();
                        m._EntityName = (string)(typeUse.InvokeMember("GetEntityName", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null));
                        m._PrimaryProperties = (string[])(typeUse.InvokeMember("GetPrimaryProperties", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null));
                        m._TableName = (string)(typeUse.InvokeMember("GetTableName", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null));
                        m._DataAccessString = (string)(typeUse.InvokeMember("GetDataAccessString", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null));
                        m._PropertyField_Dictionary = (Dictionary<string, string>)(typeUse.InvokeMember("GetPropertyField_Dictionary", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null));
                        m._FieldProperty_Dictionary = (Dictionary<string, string>)(typeUse.InvokeMember("GetFieldProperty_Dictionary", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null));

                        m._PropertyInfo_Dictionary = type.GetProperties().ToDictionary(k=>k.Name);
                        m._PrimaryFields = getPrimaryFields(m._PropertyField_Dictionary, m._PrimaryProperties);

                        dic.Add(type, m);
                    }
                }
            }
            return dic[type];
        }

        private static string[] getPrimaryFields(Dictionary<string, string> propertyField_Dictionary, string[] primaryProperties)
        {
            List<string> ls = new List<string>();
            foreach(string property in primaryProperties)
            {
                ls.Add(propertyField_Dictionary[property]);
            }
            return ls.ToArray();
        }

        private static string GetClassName(Type type)
        {
           return type.Namespace + "." + type.Name + "_Description, " + type.Assembly.FullName;
        }

        private ModelDescriptionHelper()
        {
        }

        private string _EntityName;
        private string[] _PrimaryProperties;
        private string[] _PrimaryFields;
        private string _TableName;
        private string _DataAccessString;
        private Dictionary<string, string> _PropertyField_Dictionary;
        private Dictionary<string, string> _FieldProperty_Dictionary;
        private Dictionary<string, PropertyInfo> _PropertyInfo_Dictionary;

        public string EntityName
        {
            get
            {
                return _EntityName;
            }
        }

        public string[] PrimaryProperties
        {
            get
            {
                return _PrimaryProperties;
            }
        }

        public string[] PrimaryFields
        {
            get
            {
                return _PrimaryFields;
            }
        }

        public string TableName
        {
            get
            {
                return _TableName;
            }
        }

        public string DataAccessString
        {
            get
            {
                return _DataAccessString;
            }
           
        }
        

        public Dictionary<string, string> PropertyField_Dictionary
        {
            get
            {
                return _PropertyField_Dictionary;
            }
        }

        public Dictionary<string, string> FieldProperty_Dictionary
        {
            get
            {
                return _FieldProperty_Dictionary;
            }
        }

        public Dictionary<string, PropertyInfo> PropertyInfo_Dictionary
        {
            get
            {
                return _PropertyInfo_Dictionary;
            }
        }
    }
}

