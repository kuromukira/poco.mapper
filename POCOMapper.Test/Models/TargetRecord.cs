using System;
using System.Collections.Generic;
using POCO.Mapper.Test;

namespace POCOMapper.Test
{
    internal record TargetRecord
    {
        public Guid GUID { get; set; }
        public string GUID_STRING { get; set; }
        public Guid STRING_GUID { get; set; }
        public string STRING { get; set; }
        public char CHAR { get; set; }
        public int INT { get; set; }
        public long LONG { get; set; }
        public decimal DECIMAL { get; set; }
        public double DOUBLE { get; set; }
        public float FLOAT { get; set; }
        public CommonModel.DataState ENUM { get; set; }

        public string[] STRING_ARRAY { get; set; }
        public IList<string> STRIN_LIST { get; set; }

        public int[] INT_ARRAY { get; set; }
        public IList<int> INT_LIST { get; set; }

        public long[] LONG_ARRAY { get; set; }
        public IList<long> LONG_LIST { get; set; }

        public decimal[] DECIMAL_ARRAY { get; set; }
        public IList<decimal> DECIMAL_LIST { get; set; }

        public double[] DOUBLE_ARRAY { get; set; }
        public IList<double> DOUBLE_LIST { get; set; }

        public float[] FLOAT_ARRAY { get; set; }
        public IList<float> FLOAT_LIST { get; set; }

        public InnerTargetRecord OUTER { get; set; }
        public IList<InnerTargetRecord> OUTER_LIST { get; set; }
        public InnerTargetRecord[] OUTER_ARRAY { get; set; }

        public InnerTargetRecord NULL_OUTER { get; set; }
        public IList<InnerTargetRecord> NULL_OUTER_LIST { get; set; }
        public InnerTargetRecord[] NULL_OUTER_ARRAY { get; set; }

        public InnerTargetModel CLASS { get; set; }
        public InnerTargetStruct STRUCT { get; set; }
    }

    internal record InnerTargetRecord
    {
        public object OBJECT { get; set; }
        public object[] OBJECT_ARRAY { get; set; }
        public IList<object> OBJECT_LIST { get; set; }
    }
}
