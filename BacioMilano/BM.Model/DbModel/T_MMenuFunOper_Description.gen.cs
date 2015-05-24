using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MMenuFunOper_Description {
/// <summary>
/// MenuId
/// </summary>
public const string MenuId = "MenuId";
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
static T_MMenuFunOper_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(MenuId, "MenuId");
propertyField_Dictionary.Add(OperationId, "OperationId");
propertyField_Dictionary.Add(FunctionId, "FunctionId");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("MenuId", MenuId);
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
return "T_MMenuFunOper";}
public static string[] GetPrimaryProperties()
{
return new string[] {MenuId,OperationId,FunctionId};}
public static string GetTableName()
{
return "T_MMenuFunOper";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
