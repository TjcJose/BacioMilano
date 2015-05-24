using System;
using BM.DA;
namespace BM.Model.DbModel{
[Serializable]
 public partial class T_Image : IModel {

/// <summary>
/// ImageId
/// </summary>
public Int64? ImageId { get;set;}
/// <summary>
/// UserId
/// </summary>
public Int64? UserId { get;set;}
/// <summary>
/// ImageName
/// </summary>
public String ImageName { get;set;}

/// <summary>
/// ImageExt
/// </summary>
public String ImageExt { get;set;}
/// <summary>
/// ImageSize
/// </summary>
public Int32? ImageSize { get;set;}
/// <summary>
/// ImageWidth
/// </summary>
public Int32? ImageWidth { get;set;}
/// <summary>
/// ImageHeight
/// </summary>
public Int32? ImageHeight { get;set;} }
}
