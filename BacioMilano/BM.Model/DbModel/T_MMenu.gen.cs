using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MMenu : IModel {

/// <summary>
/// MenuId
/// </summary>
public Int32? MenuId { get;set;}
/// <summary>
/// ParentId
/// </summary>
public Int32? ParentId { get;set;}
/// <summary>
/// MenuName
/// </summary>
public String MenuName { get;set;}
/// <summary>
/// MenuSort
/// </summary>
public Int32? MenuSort { get;set;}
/// <summary>
/// IsUse
/// </summary>
public Boolean? IsUse { get;set;}
/// <summary>
/// MenuType
/// </summary>
public Int32? MenuType { get;set;}
/// <summary>
/// ActionName
/// </summary>
public String ActionName { get;set;}
/// <summary>
/// ControllerName
/// </summary>
public String ControllerName { get;set;}
/// <summary>
/// Params
/// </summary>
public String Params { get;set;}
/// <summary>
/// Icon
/// </summary>
public String Icon { get;set;}
/// <summary>
/// IsActive
/// </summary>
public Boolean? IsActive { get;set;} }
}
