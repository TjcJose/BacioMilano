using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MMenuFunOper : IModel {

/// <summary>
/// MenuId
/// </summary>
public Int32? MenuId { get;set;}
/// <summary>
/// OperationId
/// </summary>
public Int32? OperationId { get;set;}
/// <summary>
/// FunctionId
/// </summary>
public Int32? FunctionId { get;set;} }
}
