using System;
using POCO.Mapper.Extension;

namespace POCOMapper.Test.Models;

public class TargetWithIgnoreModel1 : ModelMap
{
	public Guid GUID { get; set; }
	public string GUID_STRING { get; set; }
	public Guid STRING_GUID { get; set; }
	public string STRING { get; set; }
	public int INT { get; set; }
	public long LONG { get; set; }
	public decimal DECIMAL { get; set; }
	public double DOUBLE { get; set; }
	public float FLOAT { get; set; }
}

public class TargetWithIgnoreModel2 : ModelMap
{
	public Guid GUID { get; set; }
	public string GUID_STRING { get; set; }
	public Guid STRING_GUID { get; set; }
	public string STRING { get; set; }
	public int INT { get; set; }
	public long LONG { get; set; }
	public decimal DECIMAL { get; set; }
	public double DOUBLE { get; set; }
	public float FLOAT { get; set; }
}

public class TargetWithoutIgnoreModel : ModelMap
{
	public Guid GUID { get; set; }
	public string GUID_STRING { get; set; }
	public Guid STRING_GUID { get; set; }
	public string STRING { get; set; }
	public int INT { get; set; }
	public long LONG { get; set; }
	public decimal DECIMAL { get; set; }
	public double DOUBLE { get; set; }
	public float FLOAT { get; set; }
}