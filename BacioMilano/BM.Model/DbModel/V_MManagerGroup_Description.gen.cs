using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class V_MManagerGroup_Description {
/// <summary>
/// OperationId
/// </summary>
public const string OperationId = "OperationId";
/// <summary>
/// FunctionId
/// </summary>
public const string FunctionId = "FunctionId";
/// <summary>
/// GroupId
/// </summary>
public const string GroupId = "GroupId";
/// <summary>
/// ManagerId
/// </summary>
public const string ManagerId = "ManagerId";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static V_MManagerGroup_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(OperationId, "OperationId");
propertyField_Dictionary.Add(FunctionId, "FunctionId");
propertyField_Dictionary.Add(GroupId, "GroupId");
propertyField_Dictionary.Add(ManagerId, "ManagerId");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("OperationId", OperationId);
fieldProperty_Dictionary.Add("FunctionId", FunctionId);
fieldProperty_Dictionary.Add("GroupId", GroupId);
fieldProperty_Dictionary.Add("ManagerId", ManagerId);
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
return "V_MManagerGroup";}
public static string[] GetPrimaryProperties()
{
return new string[] {OperationId,FunctionId,GroupId,ManagerId};}
public static string GetTableName()
{
return "V_MManagerGroup";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
