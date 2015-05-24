using System;
using System.Collections.Generic;
namespace BM.Model.DbModel{
 public class T_Image_Description {
/// <summary>
/// ImageId
/// </summary>
public const string ImageId = "ImageId";
/// <summary>
/// UserId
/// </summary>
public const string UserId = "UserId";
/// <summary>
/// ImageName
/// </summary>
public const string ImageName = "ImageName";

/// <summary>
/// ImageExt
/// </summary>
public const string ImageExt = "ImageExt";
/// <summary>
/// ImageSize
/// </summary>
public const string ImageSize = "ImageSize";
/// <summary>
/// ImageWidth
/// </summary>
public const string ImageWidth = "ImageWidth";
/// <summary>
/// ImageHeight
/// </summary>
public const string ImageHeight = "ImageHeight";
private readonly static Dictionary<string, string> propertyField_Dictionary;
private readonly static Dictionary<string, string> fieldProperty_Dictionary;
static T_Image_Description(){
propertyField_Dictionary = new Dictionary<string, string>();
propertyField_Dictionary.Add(ImageId, "ImageId");
propertyField_Dictionary.Add(UserId, "UserId");
propertyField_Dictionary.Add(ImageName, "ImageName");
propertyField_Dictionary.Add(ImageExt, "ImageExt");
propertyField_Dictionary.Add(ImageSize, "ImageSize");
propertyField_Dictionary.Add(ImageWidth, "ImageWidth");
propertyField_Dictionary.Add(ImageHeight, "ImageHeight");
fieldProperty_Dictionary = new Dictionary<string, string>();
fieldProperty_Dictionary.Add("ImageId", ImageId);
fieldProperty_Dictionary.Add("UserId", UserId);
fieldProperty_Dictionary.Add("ImageName", ImageName);
fieldProperty_Dictionary.Add("ImageExt", ImageExt);
fieldProperty_Dictionary.Add("ImageSize", ImageSize);
fieldProperty_Dictionary.Add("ImageWidth", ImageWidth);
fieldProperty_Dictionary.Add("ImageHeight", ImageHeight);
}
public static Dictionary<string, string> GetPropertyField_Dictionary()
{
     return propertyField_Dictionary;
}
public static Dictionary<string, string> GetFieldProperty_Dictionary()
{
 	return fieldProperty_Dictionary;
}
public static string GetEntityName()
{
return "T_Image";}
public static string[] GetPrimaryProperties()
{
return new string[] {ImageId};}
public static string GetTableName()
{
return "T_Image";}
public static string GetDataAccessString()
{
return Config.ConnectionString;
}
}
}
