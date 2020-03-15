using POCO.Mapper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCO.Mapper.Test
{
    internal class CommonModel
    {
        public enum DataState { New, Updated, Removed }
    }

    internal class SourceModel : ModelMap
    {
        [MappedTo("GUID")]
        public Guid Id { get; set; }
        [MappedTo("GUID_STRING")]
        public Guid ToStringValue { get; set; }
        [MappedTo("STRING_GUID")]
        public string ToGuidValue { get; set; }
        [MappedTo("STRING")]
        public string Name { get; set; }
        [MappedTo("CHAR")]
        public char Character { get; set; }
        [MappedTo("INT")]
        public int Number { get; set; }
        [MappedTo("LONG")]
        public long LongNumber { get; set; }
        [MappedTo("DECIMAL")]
        public decimal Money { get; set; }
        [MappedTo("DOUBLE")]
        public double Currency { get; set; }
        [MappedTo("FLOAT")]
        public float Percentage { get; set; }
        [MappedTo("ENUM")]
        public CommonModel.DataState State { get; set; } = CommonModel.DataState.New;

        [MappedTo("STRING_ARRAY")]
        public string[] NameArray { get; set; }
        [MappedTo("STRIN_LIST")]
        public IList<string> NameList { get; set; }

        [MappedTo("INT_ARRAY")]
        public int[] NumberArray { get; set; }
        [MappedTo("INT_LIST")]
        public IList<int> NumberList { get; set; }

        [MappedTo("LONG_ARRAY")]
        public long[] LongArray { get; set; }
        [MappedTo("LONG_LIST")]
        public IList<long> LongList { get; set; }

        [MappedTo("DECIMAL_ARRAY")]
        public decimal[] DecimalArray { get; set; }
        [MappedTo("DECIMAL_LIST")]
        public IList<decimal> DecimalList { get; set; }

        [MappedTo("DOUBLE_ARRAY")]
        public double[] DoubleArray { get; set; }
        [MappedTo("DOUBLE_LIST")]
        public IList<double> DoubleList { get; set; }

        [MappedTo("FLOAT_ARRAY")]
        public float[] FloatArray { get; set; }
        [MappedTo("FLOAT_LIST")]
        public IList<float> FloatList { get; set; }

        [MappedTo("OUTER")]
        public InnerSourceModel InnerSource { get; set; }
        [MappedTo("OUTER_LIST")]
        public IList<InnerSourceModel> InnerSourceList { get; set; }
        [MappedTo("OUTER_ARRAY")]
        public InnerSourceModel[] InnerSourceArray { get; set; }

        [MappedTo("NULL_OUTER")]
        public InnerSourceModel JustNullInner { get; set; }
        [MappedTo("NULL_OUTER_LIST")]
        public IList<InnerSourceModel> JustNullInnerList { get; set; }
        [MappedTo("NULL_OUTER_ARRAY")]
        public InnerSourceModel[] JustNullInnerArray { get; set; }
    }

    internal class InnerSourceModel
    {
        [MappedTo("OBJECT")]
        public object Object { get; set; }
        [MappedTo("OBJECT_ARRAY")]
        public object[] ObjectArray { get; set; }
        [MappedTo("OBJECT_LIST")]
        public IList<object> ObjectList { get; set; }
    }

    internal class TargetModel
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

        public InnerTargetModel OUTER { get; set; }
        public IList<InnerTargetModel> OUTER_LIST { get; set; }
        public InnerTargetModel[] OUTER_ARRAY { get; set; }

        public InnerTargetModel NULL_OUTER { get; set; }
        public IList<InnerTargetModel> NULL_OUTER_LIST { get; set; }
        public InnerTargetModel[] NULL_OUTER_ARRAY { get; set; }
    }

    internal class InnerTargetModel
    {
        public object OBJECT { get; set; }
        public object[] OBJECT_ARRAY { get; set; }
        public IList<object> OBJECT_LIST { get; set; }
    }

    internal class MultipleMapSourceModel : ModelMap
    {
        [MappedTo("STR")]
        [MappedTo("STRING")]
        public string Text { get; set; }

        [MappedTo("INT")]
        [MappedTo("INTEGER")]
        public int Number { get; set; }
    }

    internal class MultipleMapTargetModel
    {
        public string STRING { get; set; }
        public string STR { get; set; }

        public int INTEGER { get; set; }
        public int INT { get; set; }
    }

    static class ValueGenerator
    {
        private static readonly Random Random = new Random();

        internal static int RandomNumber() => Random.Next(1, 50);

        internal static int Number(int max = 9) => Random.Next(0, max);

        internal static string Word()
        {
            int size = Random.Next(3, 5);
            StringBuilder builder = new StringBuilder(string.Empty);
            for (int i = 0; i < size; i++)
                builder.Append((char)Random.Next(65, 90));
            return builder.ToString();
        }

        internal static char Character() => (char)Random.Next(65, 90);
    }

    internal class DataGenerator
    {
        private InnerSourceModel GenerateInnerSource() =>
            new InnerSourceModel
            {
                Object = ValueGenerator.Word(),
                ObjectArray = new List<object> { ValueGenerator.Word(),
                    ValueGenerator.Number(),
                    ValueGenerator.Character() }.ToArray(),
                ObjectList = new List<object> { ValueGenerator.Word(),
                    ValueGenerator.Number(),
                    ValueGenerator.Character() }
            };

        private SourceModel GenerateSourceModel()
        {
            int listSize = ValueGenerator.RandomNumber();
            return new SourceModel
            {
                Id = Guid.NewGuid(),
                ToGuidValue = Guid.NewGuid().ToString(),
                ToStringValue = Guid.NewGuid(),
                Name = ValueGenerator.Word(),
                Character = ValueGenerator.Character(),
                Number = ValueGenerator.Number(),
                LongNumber = ValueGenerator.Number(1000),
                Money = ValueGenerator.Number(10000),
                Currency = ValueGenerator.Number(10000),
                Percentage = ValueGenerator.Number(100),
                State = CommonModel.DataState.New,

                NameArray = Enumerable.Range(1, listSize).Select(i => ValueGenerator.Word()).ToArray(),
                NameList = Enumerable.Range(1, listSize).Select(i => ValueGenerator.Word()).ToList(),

                NumberArray = Enumerable.Range(1, listSize).Select(i => ValueGenerator.Number()).ToArray(),
                NumberList = Enumerable.Range(1, listSize).Select(i => ValueGenerator.Number()).ToList(),

                LongArray = Enumerable.Range(1, listSize).Select(i => (long)ValueGenerator.Number()).ToArray(),
                LongList = Enumerable.Range(1, listSize).Select(i => (long)ValueGenerator.Number()).ToList(),

                DecimalArray = Enumerable.Range(1, listSize).Select(i => (decimal)ValueGenerator.Number()).ToArray(),
                DecimalList = Enumerable.Range(1, listSize).Select(i => (decimal)ValueGenerator.Number()).ToList(),

                DoubleArray = Enumerable.Range(1, listSize).Select(i => (double)ValueGenerator.Number()).ToArray(),
                DoubleList = Enumerable.Range(1, listSize).Select(i => (double)ValueGenerator.Number()).ToList(),

                FloatArray = Enumerable.Range(1, listSize).Select(i => (float)ValueGenerator.Number()).ToArray(),
                FloatList = Enumerable.Range(1, listSize).Select(i => (float)ValueGenerator.Number()).ToList(),

                InnerSource = GenerateInnerSource(),
                InnerSourceArray = Enumerable.Range(1, listSize).Select(i => GenerateInnerSource()).ToArray(),
                InnerSourceList = Enumerable.Range(1, listSize).Select(i => GenerateInnerSource()).ToList()
            };
        }

        internal IList<SourceModel> GenerateSourceModels(int limit) => Enumerable.Range(1, limit).Select(i => GenerateSourceModel()).ToList();

        public MultipleMapSourceModel GenerateMultiFieldSourceModel() =>
            new MultipleMapSourceModel
            {
                Number = ValueGenerator.Number(),
                Text = ValueGenerator.Word()
            };

        internal IList<MultipleMapSourceModel> GenerateMultiFieldSourceModels(int limit) => Enumerable.Range(1, limit).Select(i => GenerateMultiFieldSourceModel()).ToList();
    }
}
