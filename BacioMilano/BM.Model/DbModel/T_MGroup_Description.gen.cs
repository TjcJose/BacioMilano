using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MGroup_Description {
/// <summary>
/// GroupId
/// </summary>
public const string GroupId = "GroupId";
/// <summary>
/// GroupName
/// </summary>
public const string GroupName = "GroupName";
/// <summary>
/// GroupMemo
/// </summary>
public const string GroupMemo = "GroupMemo";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_MGroup_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(GroupId, "GroupId");
propertyField_Dictionary.Add(GroupName, "GroupName");
propertyField_Dictionary.Add(GroupMemo, "GroupMemo");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("GroupId", GroupId);
fieldProperty_Dictionary.Add("GroupName", GroupName);
fieldProperty_Dictionary.Add("GroupMemo", GroupMemo);
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
return "T_MGroup";}
public static string[] GetPrimaryProperties()
{
return new string[] {GroupId};}
public static string GetTableName()
{
return "T_MGroup";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
