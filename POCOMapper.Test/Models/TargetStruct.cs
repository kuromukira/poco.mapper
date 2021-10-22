using System;
using System.Collections.Generic;

namespace POCOMapper.Test
{
    internal struct TargetStruct
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

        public InnerTargetStruct OUTER_ST { get; set; }
        public IList<InnerTargetStruct> OUTER_LIST_ST { get; set; }
        public InnerTargetStruct[] OUTER_ARRAY_ST { get; set; }

        public InnerTargetModel OUTER { get; set; }
        public IList<InnerTargetModel> OUTER_LIST { get; set; }
        public InnerTargetModel[] OUTER_ARRAY { get; set; }

        public InnerTargetModel NULL_OUTER { get; set; }
        public IList<InnerTargetModel> NULL_OUTER_LIST { get; set; }
        public InnerTargetModel[] NULL_OUTER_ARRAY { get; set; }
    }

    internal struct InnerTargetStruct
    {
        public object OBJECT { get; set; }
        public object[] OBJECT_ARRAY { get; set; }
        public IList<object> OBJECT_LIST { get; set; }
    }
}
