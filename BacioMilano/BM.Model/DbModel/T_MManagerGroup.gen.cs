using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MManagerGroup : IModel {

/// <summary>
/// ManagerId
/// </summary>
public Int64? ManagerId { get;set;}
/// <summary>
/// GroupId
/// </summary>
public Int32? GroupId { get;set;} }
}
