using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MFunOper_Description {
/// <summary>
/// FunctionId
/// </summary>
public const string FunctionId = "FunctionId";
/// <summary>
/// OperationId
/// </summary>
public const string OperationId = "OperationId";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_MFunOper_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(FunctionId, "FunctionId");
propertyField_Dictionary.Add(OperationId, "OperationId");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("FunctionId", FunctionId);
fieldProperty_Dictionary.Add("OperationId", OperationId);
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
return "T_MFunOper";}
public static string[] GetPrimaryProperties()
{
return new string[] {OperationId,FunctionId};}
public static string GetTableName()
{
return "T_MFunOper";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
