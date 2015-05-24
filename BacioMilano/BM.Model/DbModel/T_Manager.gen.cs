using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_Manager : IModel {

/// <summary>
/// ManagerId
/// </summary>
public Int64? ManagerId { get;set;}
/// <summary>
/// UserName
/// </summary>
public String UserName { get;set;}
/// <summary>
/// TrueName
/// </summary>
public String TrueName { get;set;}
/// <summary>
/// UserPwd
/// </summary>
public String UserPwd { get;set;}
/// <summary>
/// IsUse
/// </summary>
public Boolean? IsUse { get;set;} }
}
