using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MRight_Description {
/// <summary>
/// ManagerId
/// </summary>
public const string ManagerId = "ManagerId";
/// <summary>
/// OperationId
/// </summary>
public const string OperationId = "OperationId";
/// <summary>
/// FunctionId
/// </summary>
public const string FunctionId = "FunctionId";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_MRight_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(ManagerId, "ManagerId");
propertyField_Dictionary.Add(OperationId, "OperationId");
propertyField_Dictionary.Add(FunctionId, "FunctionId");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("ManagerId", ManagerId);
fieldProperty_Dictionary.Add("OperationId", OperationId);
fieldProperty_Dictionary.Add("FunctionId", FunctionId);
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
return "T_MRight";}
public static string[] GetPrimaryProperties()
{
return new string[] {ManagerId,OperationId,FunctionId};}
public static string GetTableName()
{
return "T_MRight";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
