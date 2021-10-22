using System;
using POCO.Mapper;
using POCO.Mapper.Extension;

namespace POCOMapper.Test
{
    internal class MultipleMapSourceModel : ModelMap
    {
        [MappedTo("GUID")] public Guid Id { get; set; }

        [MappedTo("STR")]
        [MappedTo("STRING")]
        public string Text { get; set; }

        [MappedTo("INT")]
        [MappedTo("INTEGER")]
        public int Number { get; set; }
    }

    internal class MultipleMapTargetModel
    {
        public Guid GUID { get; set; }

        public string STRING { get; set; }
        public string STR { get; set; }

        public int INTEGER { get; set; }
        public int INT { get; set; }
    }
}
