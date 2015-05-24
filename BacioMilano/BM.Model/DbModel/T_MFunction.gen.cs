using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_MFunction : IModel {

/// <summary>
/// FunctionId
/// </summary>
public Int32? FunctionId { get;set;}
/// <summary>
/// FunctionName
/// </summary>
public String FunctionName { get;set;} }
}
