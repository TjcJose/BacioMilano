using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MOperation_Description {
/// <summary>
/// OperationId
/// </summary>
public const string OperationId = "OperationId";
/// <summary>
/// OperationName
/// </summary>
public const string OperationName = "OperationName";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_MOperation_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(OperationId, "OperationId");
propertyField_Dictionary.Add(OperationName, "OperationName");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("OperationId", OperationId);
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
return "T_MOperation";}
public static string[] GetPrimaryProperties()
{
return new string[] {OperationId};}
public static string GetTableName()
{
return "T_MOperation";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
