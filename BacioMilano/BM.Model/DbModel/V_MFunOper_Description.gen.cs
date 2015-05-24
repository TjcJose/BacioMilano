using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class V_MFunOper_Description {
/// <summary>
/// OperationId
/// </summary>
public const string OperationId = "OperationId";
/// <summary>
/// FunctionId
/// </summary>
public const string FunctionId = "FunctionId";
/// <summary>
/// FunctionName
/// </summary>
public const string FunctionName = "FunctionName";
/// <summary>
/// OperationName
/// </summary>
public const string OperationName = "OperationName";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static V_MFunOper_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(OperationId, "OperationId");
propertyField_Dictionary.Add(FunctionId, "FunctionId");
propertyField_Dictionary.Add(FunctionName, "FunctionName");
propertyField_Dictionary.Add(OperationName, "OperationName");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("OperationId", OperationId);
fieldProperty_Dictionary.Add("FunctionId", FunctionId);
fieldProperty_Dictionary.Add("FunctionName", FunctionName);
fieldProperty_Dictionary.Add("OperationName", OperationName);
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
return "V_MFunOper";}
public static string[] GetPrimaryProperties()
{
return new string[] {OperationId,FunctionId};}
public static string GetTableName()
{
return "V_MFunOper";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
