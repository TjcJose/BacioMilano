using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MFunOper : IModel {

/// <summary>
/// FunctionId
/// </summary>
public Int32? FunctionId { get;set;}
/// <summary>
/// OperationId
/// </summary>
public Int32? OperationId { get;set;} }
}
