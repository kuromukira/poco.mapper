using System;
using POCO.Mapper;
using POCO.Mapper.Extension;

namespace POCOMapper.Test
{
    internal class SourceWithIgnoreModel : ModelMap
    {
        [MappedTo("GUID")]
        public Guid Id { get; set; }

        [MappedTo("GUID_STRING")]
        public Guid ToStringValue { get; set; }

        [MappedTo("STRING_GUID")]
        public string ToGuidValue { get; set; }

        [IgnoreIf(typeof(TargetWithIgnoreModel2))]
        [MappedTo("STRING")]
        public string Name { get; set; }

        [IgnoreIf(typeof(TargetWithIgnoreModel1))]
        [IgnoreIf(typeof(TargetWithIgnoreModel2))]
        [MappedTo("INT")]
        public int Number { get; set; }

        [IgnoreIf(typeof(TargetWithIgnoreModel1))]
        [IgnoreIf(typeof(TargetWithIgnoreModel2))]
        [MappedTo("LONG")]
        public long LongNumber { get; set; }

        [IgnoreIf(typeof(TargetWithIgnoreModel1))]
        [MappedTo("DECIMAL")]
        public decimal Money { get; set; }

        [IgnoreIf(typeof(TargetWithIgnoreModel2))]
        [MappedTo("DOUBLE")]
        public double Currency { get; set; }

        [IgnoreIf(typeof(TargetWithIgnoreModel1))]
        [MappedTo("FLOAT")]
        public float Percentage { get; set; }
    }
}
