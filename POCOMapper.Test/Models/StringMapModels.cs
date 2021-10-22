using System;
using POCO.Mapper;
using POCO.Mapper.Extension;

namespace POCOMapper.Test
{
    internal class SourceToString : ModelMap
    {
        [MappedTo("DATE_TO_STRING")]
        [UseFormat("yyyy-MM-dd")]
        public DateTime DateTimeForString { get; set; }

        [MappedTo("INT_TO_STRING")]
        [UseFormat("#,###")]
        public int IntForString { get; set; }

        [MappedTo("DECIMAL_TO_STRING")]
        [UseFormat("#,###.##")]
        public decimal DecimalForString { get; set; }
    }

    internal class TargetToString : ModelMap
    {
        public string DATE_TO_STRING { get; set; }
        public string INT_TO_STRING { get; set; }
        public string DECIMAL_TO_STRING { get; set; }
    }
}
