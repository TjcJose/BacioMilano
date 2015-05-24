using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_Manager_Description {
/// <summary>
/// ManagerId
/// </summary>
public const string ManagerId = "ManagerId";
/// <summary>
/// UserName
/// </summary>
public const string UserName = "UserName";
/// <summary>
/// TrueName
/// </summary>
public const string TrueName = "TrueName";
/// <summary>
/// UserPwd
/// </summary>
public const string UserPwd = "UserPwd";
/// <summary>
/// IsUse
/// </summary>
public const string IsUse = "IsUse";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_Manager_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(ManagerId, "ManagerId");
propertyField_Dictionary.Add(UserName, "UserName");
propertyField_Dictionary.Add(TrueName, "TrueName");
propertyField_Dictionary.Add(UserPwd, "UserPwd");
propertyField_Dictionary.Add(IsUse, "IsUse");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("ManagerId", ManagerId);
fieldProperty_Dictionary.Add("UserName", UserName);
fieldProperty_Dictionary.Add("TrueName", TrueName);
fieldProperty_Dictionary.Add("UserPwd", UserPwd);
fieldProperty_Dictionary.Add("IsUse", IsUse);
}
public static Dictionary<string, string> GetPropertyField_Dictionary()
{
     return propertyField_Dictionary;
}
public static Dictionary<string, string> GetFieldProperty_Dictionary()
{
 	return fieldProperty_Dictionary;
}
public static string GetEntityName()
{
return "T_Manager";}
public static string[] GetPrimaryProperties()
{
return new string[] {ManagerId};}
public static string GetTableName()
{
return "T_Manager";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
