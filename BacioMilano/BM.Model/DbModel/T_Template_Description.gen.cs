using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_Template_Description {
/// <summary>
/// TemplateId
/// </summary>
public const string TemplateId = "TemplateId";
/// <summary>
/// TemplateName
/// </summary>
public const string TemplateName = "TemplateName";
/// <summary>
/// TemplateContent
/// </summary>
public const string TemplateContent = "TemplateContent";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_Template_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(TemplateId, "TemplateId");
propertyField_Dictionary.Add(TemplateName, "TemplateName");
propertyField_Dictionary.Add(TemplateContent, "TemplateContent");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("TemplateId", TemplateId);
fieldProperty_Dictionary.Add("TemplateName", TemplateName);
fieldProperty_Dictionary.Add("TemplateContent", TemplateContent);
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
return "T_Template";}
public static string[] GetPrimaryProperties()
{
return new string[] {TemplateId};}
public static string GetTableName()
{
return "T_Template";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
