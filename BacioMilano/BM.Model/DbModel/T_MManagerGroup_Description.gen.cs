using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MManagerGroup_Description {
/// <summary>
/// ManagerId
/// </summary>
public const string ManagerId = "ManagerId";
/// <summary>
/// GroupId
/// </summary>
public const string GroupId = "GroupId";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_MManagerGroup_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(ManagerId, "ManagerId");
propertyField_Dictionary.Add(GroupId, "GroupId");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("ManagerId", ManagerId);
fieldProperty_Dictionary.Add("GroupId", GroupId);
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
return "T_MManagerGroup";}
public static string[] GetPrimaryProperties()
{
return new string[] {ManagerId,GroupId};}
public static string GetTableName()
{
return "T_MManagerGroup";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
