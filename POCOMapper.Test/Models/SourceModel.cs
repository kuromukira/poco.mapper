using System;
using System.Collections.Generic;
using POCO.Mapper;
using POCO.Mapper.Extension;
using POCO.Mapper.Test;

namespace POCOMapper.Test
{
    internal class SourceModel : ModelMap
    {
        [MappedTo("GUID")] public Guid Id { get; set; }
        [MappedTo("GUID_STRING")] public Guid ToStringValue { get; set; }
        [MappedTo("STRING_GUID")] public string ToGuidValue { get; set; }
        [MappedTo("STRING")] public string Name { get; set; }
        [MappedTo("CHAR")] public char Character { get; set; }
        [MappedTo("INT")] public int Number { get; set; }
        [MappedTo("LONG")] public long LongNumber { get; set; }
        [MappedTo("DECIMAL")] public decimal Money { get; set; }
        [MappedTo("DOUBLE")] public double Currency { get; set; }
        [MappedTo("FLOAT")] public float Percentage { get; set; }
        [MappedTo("ENUM")] public CommonModel.DataState State { get; set; } = CommonModel.DataState.New;

        [MappedTo("STRING_ARRAY")] public string[] NameArray { get; set; }
        [MappedTo("STRIN_LIST")] public IList<string> NameList { get; set; }

        [MappedTo("INT_ARRAY")] public int[] NumberArray { get; set; }
        [MappedTo("INT_LIST")] public IList<int> NumberList { get; set; }

        [MappedTo("LONG_ARRAY")] public long[] LongArray { get; set; }
        [MappedTo("LONG_LIST")] public IList<long> LongList { get; set; }

        [MappedTo("DECIMAL_ARRAY")] public decimal[] DecimalArray { get; set; }
        [MappedTo("DECIMAL_LIST")] public IList<decimal> DecimalList { get; set; }

        [MappedTo("DOUBLE_ARRAY")] public double[] DoubleArray { get; set; }
        [MappedTo("DOUBLE_LIST")] public IList<double> DoubleList { get; set; }

        [MappedTo("FLOAT_ARRAY")] public float[] FloatArray { get; set; }
        [MappedTo("FLOAT_LIST")] public IList<float> FloatList { get; set; }

        [MappedTo("OUTER")] public InnerSourceModel InnerSource { get; set; }
        [MappedTo("OUTER_LIST")] public IList<InnerSourceModel> InnerSourceList { get; set; }
        [MappedTo("OUTER_ARRAY")] public InnerSourceModel[] InnerSourceArray { get; set; }

        [MappedTo("NULL_OUTER")] public InnerSourceModel JustNullInner { get; set; }
        [MappedTo("NULL_OUTER_LIST")] public IList<InnerSourceModel> JustNullInnerList { get; set; }
        [MappedTo("NULL_OUTER_ARRAY")] public InnerSourceModel[] JustNullInnerArray { get; set; }

        [MappedTo("RECORD")] public InnerSourceRecord RecordProperty { get; set; }
        [MappedTo("STRUCT")] public InnerSourceStruct StructProperty { get; set; }
    }

    internal class InnerSourceModel
    {
        [MappedTo("OBJECT")] public object Object { get; set; }
        [MappedTo("OBJECT_ARRAY")] public object[] ObjectArray { get; set; }
        [MappedTo("OBJECT_LIST")] public IList<object> ObjectList { get; set; }
    }

}
