using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_MMenu_Description {
/// <summary>
/// MenuId
/// </summary>
public const string MenuId = "MenuId";
/// <summary>
/// ParentId
/// </summary>
public const string ParentId = "ParentId";
/// <summary>
/// MenuName
/// </summary>
public const string MenuName = "MenuName";
/// <summary>
/// MenuSort
/// </summary>
public const string MenuSort = "MenuSort";
/// <summary>
/// IsUse
/// </summary>
public const string IsUse = "IsUse";
/// <summary>
/// MenuType
/// </summary>
public const string MenuType = "MenuType";
/// <summary>
/// ActionName
/// </summary>
public const string ActionName = "ActionName";
/// <summary>
/// ControllerName
/// </summary>
public const string ControllerName = "ControllerName";
/// <summary>
/// Params
/// </summary>
public const string Params = "Params";
/// <summary>
/// Icon
/// </summary>
public const string Icon = "Icon";
/// <summary>
/// IsActive
/// </summary>
public const string IsActive = "IsActive";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_MMenu_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(MenuId, "MenuId");
propertyField_Dictionary.Add(ParentId, "ParentId");
propertyField_Dictionary.Add(MenuName, "MenuName");
propertyField_Dictionary.Add(MenuSort, "MenuSort");
propertyField_Dictionary.Add(IsUse, "IsUse");
propertyField_Dictionary.Add(MenuType, "MenuType");
propertyField_Dictionary.Add(ActionName, "ActionName");
propertyField_Dictionary.Add(ControllerName, "ControllerName");
propertyField_Dictionary.Add(Params, "Params");
propertyField_Dictionary.Add(Icon, "Icon");
propertyField_Dictionary.Add(IsActive, "IsActive");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("MenuId", MenuId);
fieldProperty_Dictionary.Add("ParentId", ParentId);
fieldProperty_Dictionary.Add("MenuName", MenuName);
fieldProperty_Dictionary.Add("MenuSort", MenuSort);
fieldProperty_Dictionary.Add("IsUse", IsUse);
fieldProperty_Dictionary.Add("MenuType", MenuType);
fieldProperty_Dictionary.Add("ActionName", ActionName);
fieldProperty_Dictionary.Add("ControllerName", ControllerName);
fieldProperty_Dictionary.Add("Params", Params);
fieldProperty_Dictionary.Add("Icon", Icon);
fieldProperty_Dictionary.Add("IsActive", IsActive);
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
return "T_MMenu";}
public static string[] GetPrimaryProperties()
{
return new string[] {MenuId};}
public static string GetTableName()
{
return "T_MMenu";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
