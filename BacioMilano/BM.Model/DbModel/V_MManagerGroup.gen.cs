using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class V_MManagerGroup : IModel {

/// <summary>
/// OperationId
/// </summary>
public Int32? OperationId { get;set;}
/// <summary>
/// FunctionId
/// </summary>
public Int32? FunctionId { get;set;}
/// <summary>
/// GroupId
/// </summary>
public Int32? GroupId { get;set;}
/// <summary>
/// ManagerId
/// </summary>
public Int64? ManagerId { get;set;} }
}
