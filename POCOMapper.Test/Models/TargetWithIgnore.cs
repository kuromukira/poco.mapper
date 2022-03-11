using System;
using POCO.Mapper;
using POCO.Mapper.Extension;

namespace POCOMapper.Test
{
    internal class TargetWithIgnoreModel1 : ModelMap
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

    internal class TargetWithIgnoreModel2 : ModelMap
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

    internal class TargetWithoutIgnoreModel : ModelMap
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
}
