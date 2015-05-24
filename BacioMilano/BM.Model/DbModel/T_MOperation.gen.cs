using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MOperation : IModel {

/// <summary>
/// OperationId
/// </summary>
public Int32? OperationId { get;set;}
/// <summary>
/// OperationName
/// </summary>
public String OperationName { get;set;} }
}
