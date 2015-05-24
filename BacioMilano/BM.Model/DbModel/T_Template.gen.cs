using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_Template : IModel {

/// <summary>
/// TemplateId
/// </summary>
public Int32? TemplateId { get;set;}
/// <summary>
/// TemplateName
/// </summary>
public String TemplateName { get;set;}
/// <summary>
/// TemplateContent
/// </summary>
public String TemplateContent { get;set;} }
}
