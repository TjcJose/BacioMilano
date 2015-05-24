using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MGroup : IModel {

/// <summary>
/// GroupId
/// </summary>
public Int32? GroupId { get;set;}
/// <summary>
/// GroupName
/// </summary>
public String GroupName { get;set;}
/// <summary>
/// GroupMemo
/// </summary>
public String GroupMemo { get;set;} }
}
