using POCO.Mapper.Extension;
using POCOMapper.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCO.Mapper.Test
{
    internal class CommonModel
    {
        public enum DataState
        {
            New,
            Updated,
            Removed
        }
    }

    static class ValueGenerator
    {
        private static readonly Random Random = new Random();

        internal static int RandomNumber() => Random.Next(1, 50);

        internal static int Number(int min = 1, int max = 9) => Random.Next(min, max);

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
        private static InnerSourceModel GenerateInnerSource()
            => new()
            {
                Object = ValueGenerator.Word(),
                ObjectArray = new List<object>
                {
                    ValueGenerator.Word(),
                    ValueGenerator.Number(),
                    ValueGenerator.Character()
                }.ToArray(),
                ObjectList = new List<object>
                {
                    ValueGenerator.Word(),
                    ValueGenerator.Number(),
                    ValueGenerator.Character()
                }
            };

        private static SourceModel GenerateSourceModel()
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
                LongNumber = ValueGenerator.Number(max: 1000),
                Money = ValueGenerator.Number(max: 10000),
                Currency = ValueGenerator.Number(max: 10000),
                Percentage = ValueGenerator.Number(max: 100),
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

        internal static IList<SourceModel> GenerateSourceModels(int limit) => Enumerable.Range(1, limit).Select(i => GenerateSourceModel()).ToList();

        public static MultipleMapSourceModel GenerateMultiFieldSourceModel()
            => new()
            {
                Id = Guid.NewGuid(),
                Number = ValueGenerator.Number(),
                Text = ValueGenerator.Word()
            };

        internal static IList<MultipleMapSourceModel> GenerateMultiFieldSourceModels(int limit) => Enumerable.Range(1, limit).Select(i => GenerateMultiFieldSourceModel()).ToList();

        public static SourceToString GenerateSourceToString()
            => new()
            {
                DecimalForString = Convert.ToDecimal($"{ValueGenerator.Number(min: 1000, max: 9999)}.{ValueGenerator.RandomNumber()}"),
                IntForString = ValueGenerator.Number(min: 1000, max: 9999),
                DateTimeForString = DateTime.Now
            };

        private static InnerSourceStruct GenerateInnerSourceStruct()
            => new()
            {
                Object = ValueGenerator.Word(),
                ObjectArray = new List<object>
                {
                    ValueGenerator.Word(),
                    ValueGenerator.Number(),
                    ValueGenerator.Character()
                }.ToArray(),
                ObjectList = new List<object>
                {
                    ValueGenerator.Word(),
                    ValueGenerator.Number(),
                    ValueGenerator.Character()
                }
            };

        private static SourceStruct GenerateSourceStruct()
        {
            int listSize = ValueGenerator.RandomNumber();
            return new SourceStruct
            {
                Id = Guid.NewGuid(),
                ToGuidValue = Guid.NewGuid().ToString(),
                ToStringValue = Guid.NewGuid(),
                Name = ValueGenerator.Word(),
                Character = ValueGenerator.Character(),
                Number = ValueGenerator.Number(),
                LongNumber = ValueGenerator.Number(max: 1000),
                Money = ValueGenerator.Number(max: 10000),
                Currency = ValueGenerator.Number(max: 10000),
                Percentage = ValueGenerator.Number(max: 100),

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
                InnerSourceList = Enumerable.Range(1, listSize).Select(i => GenerateInnerSource()).ToList(),

                InnerSourceStruct = GenerateInnerSourceStruct(),
                InnerSourceArrayStruct = Enumerable.Range(1, listSize).Select(i => GenerateInnerSourceStruct()).ToArray(),
                InnerSourceListStruct = Enumerable.Range(1, listSize).Select(i => GenerateInnerSourceStruct()).ToList()
            };
        }

        internal static IList<SourceStruct> GenerateSourceStructs(int limit) => Enumerable.Range(1, limit).Select(i => GenerateSourceStruct()).ToList();
    }
}