using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class V_MFunOper : IModel {

/// <summary>
/// OperationId
/// </summary>
public Int32? OperationId { get;set;}
/// <summary>
/// FunctionId
/// </summary>
public Int32? FunctionId { get;set;}
/// <summary>
/// FunctionName
/// </summary>
public String FunctionName { get;set;}
/// <summary>
/// OperationName
/// </summary>
public String OperationName { get;set;} }
}
