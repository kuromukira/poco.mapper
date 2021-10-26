using POCO.Mapper.Extension;
using POCOMapper.Test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace POCO.Mapper.Test
{
    public class MapperTest
    {
        private IMapper<TargetModel, SourceModel> IMapper { get; } = new ModelMapper<TargetModel, SourceModel>();
        private IMapper<TargetStruct, SourceStruct> IStructMapper { get; } = new ModelMapper<TargetStruct, SourceStruct>();
        private IMapper<TargetRecord, SourceRecord> IRecordMapper { get; } = new ModelMapper<TargetRecord, SourceRecord>();
        private IMapper<TargetToString, SourceToString> StringIMapper { get; } = new ModelMapper<TargetToString, SourceToString>();
        private IMapper<MultipleMapTargetModel, MultipleMapSourceModel> MultiFieldIMapper { get; } = new ModelMapper<MultipleMapTargetModel, MultipleMapSourceModel>();

        static void AssertValues(SourceModel source, TargetModel target)
        {
            Assert.True(source.Id == target.GUID);
            Assert.True(source.Character == target.CHAR);
            Assert.True(source.Money == target.DECIMAL);
            Assert.Equal(source.DecimalArray, target.DECIMAL_ARRAY);
            Assert.Equal(source.DecimalList, target.DECIMAL_LIST);
            Assert.True(source.Currency == target.DOUBLE);
            Assert.Equal(source.DoubleArray, target.DOUBLE_ARRAY);
            Assert.Equal(source.DoubleList, target.DOUBLE_LIST);
            Assert.True(source.Percentage == target.FLOAT);
            Assert.Equal(source.FloatArray, target.FLOAT_ARRAY);
            Assert.Equal(source.FloatList, target.FLOAT_LIST);
            Assert.Same(source.InnerSource.Object, target.OUTER.OBJECT);
            Assert.Equal(source.InnerSource.ObjectArray, target.OUTER.OBJECT_ARRAY);
            Assert.Equal(source.InnerSource.ObjectList, target.OUTER.OBJECT_LIST);
            Assert.Same(source.JustNullInner, target.NULL_OUTER);
            Assert.Same(source.JustNullInnerArray, target.NULL_OUTER_ARRAY);
            Assert.Same(source.JustNullInnerList, target.NULL_OUTER_LIST);
            Assert.Equal(source.LongArray, target.LONG_ARRAY);
            Assert.Equal(source.LongList, target.LONG_LIST);
            Assert.True(source.LongNumber == target.LONG);
            Assert.True(source.Name == target.STRING);
            Assert.Equal(source.NameArray, target.STRING_ARRAY);
            Assert.Equal(source.NameList, target.STRIN_LIST);
            Assert.True(source.Number == target.INT);
            Assert.Equal(source.NumberArray, target.INT_ARRAY);
            Assert.Equal(source.NumberList, target.INT_LIST);
            Assert.True(source.State == target.ENUM);
            Assert.True(source.ToGuidValue == target.STRING_GUID.ToString());
            Assert.True(source.ToStringValue.ToString() == target.GUID_STRING);
            Assert.Same(source.RecordProperty.Object, target.RECORD.OBJECT);
            Assert.Equal(source.RecordProperty.ObjectArray, target.RECORD.OBJECT_ARRAY);
            Assert.Equal(source.RecordProperty.ObjectList, target.RECORD.OBJECT_LIST);
            Assert.Same(source.StructProperty.Object, target.STRUCT.OBJECT);
            Assert.Equal(source.StructProperty.ObjectArray, target.STRUCT.OBJECT_ARRAY);
            Assert.Equal(source.StructProperty.ObjectList, target.STRUCT.OBJECT_LIST);
        }

        static void AssertMultiValues(MultipleMapSourceModel source, MultipleMapTargetModel target)
        {
            Assert.True(source.Id == target.GUID);
            Assert.True(source.Text == target.STR);
            Assert.True(source.Text == target.STRING);
            Assert.True(source.Number == target.INT);
            Assert.True(source.Number == target.INTEGER);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Values_Using_IMapper(int sizes)
        {
            IList<SourceModel> source = DataGenerator.GenerateSourceModels(sizes);
            IList<TargetModel> target = IMapper.From(source);

            Assert.NotNull(target);

            foreach (SourceModel _source in source)
            {
                Assert.Contains(target, t => t.GUID == _source.Id);
                foreach (TargetModel _target in target.Where(t => t.GUID == _source.Id).ToList())
                    AssertValues(_source, _target);
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Values_Using_Extension_Methods(int sizes)
        {
            IList<SourceModel> source = DataGenerator.GenerateSourceModels(sizes);
            TargetModel[] target = source.MapToArray<TargetModel>();

            Assert.NotNull(target);

            foreach (SourceModel _source in source)
            {
                Assert.Contains(target, t => t.GUID == _source.Id);
                foreach (TargetModel _target in target.Where(t => t.GUID == _source.Id).ToList())
                    AssertValues(_source, _target);
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_MultiField_Values_Using_IMapper(int sizes)
        {
            IList<MultipleMapSourceModel> source = DataGenerator.GenerateMultiFieldSourceModels(sizes);
            IList<MultipleMapTargetModel> target = MultiFieldIMapper.From(source);

            Assert.NotNull(target);

            foreach (MultipleMapSourceModel _source in source)
            {
                Assert.Contains(target, t => t.GUID == _source.Id);
                foreach (MultipleMapTargetModel _target in target.Where(t => t.GUID == _source.Id).ToList())
                    AssertMultiValues(_source, _target);
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_MultiField_Values_Using_Extension_Methods(int sizes)
        {
            IList<MultipleMapSourceModel> source = DataGenerator.GenerateMultiFieldSourceModels(sizes);
            MultipleMapTargetModel[] target = source.MapToArray<MultipleMapTargetModel>();

            Assert.NotNull(target);

            foreach (MultipleMapSourceModel _source in source)
            {
                Assert.Contains(target, t => t.GUID == _source.Id);
                foreach (MultipleMapTargetModel _target in target.Where(t => t.GUID == _source.Id).ToList())
                    AssertMultiValues(_source, _target);
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Structs_Using_IMapper(int sizes)
        {
            IList<SourceStruct> source = DataGenerator.GenerateSourceStructs(sizes);
            IList<TargetStruct> target = IStructMapper.From(source);

            Assert.NotNull(target);

            foreach (SourceStruct _source in source)
            {
                Assert.Contains(target, t => t.GUID == _source.Id);
                foreach (TargetStruct _target in target.Where(t => t.GUID == _source.Id).ToList())
                {
                    Assert.True(_source.Id == _target.GUID);
                    Assert.True(_source.Character == _target.CHAR);
                    Assert.True(_source.Money == _target.DECIMAL);
                    Assert.Equal(_source.DecimalArray, _target.DECIMAL_ARRAY);
                    Assert.Equal(_source.DecimalList, _target.DECIMAL_LIST);
                    Assert.True(_source.Currency == _target.DOUBLE);
                    Assert.Equal(_source.DoubleArray, _target.DOUBLE_ARRAY);
                    Assert.Equal(_source.DoubleList, _target.DOUBLE_LIST);
                    Assert.True(_source.Percentage == _target.FLOAT);
                    Assert.Equal(_source.FloatArray, _target.FLOAT_ARRAY);
                    Assert.Equal(_source.FloatList, _target.FLOAT_LIST);
                    Assert.Same(_source.InnerSource.Object, _target.OUTER.OBJECT);
                    Assert.Equal(_source.InnerSource.ObjectArray, _target.OUTER.OBJECT_ARRAY);
                    Assert.Equal(_source.InnerSource.ObjectList, _target.OUTER.OBJECT_LIST);
                    Assert.Same(_source.JustNullInner, _target.NULL_OUTER);
                    Assert.Same(_source.JustNullInnerArray, _target.NULL_OUTER_ARRAY);
                    Assert.Same(_source.JustNullInnerList, _target.NULL_OUTER_LIST);
                    Assert.Same(_source.InnerSourceStruct.Object, _target.OUTER_ST.OBJECT);
                    Assert.Equal(_source.InnerSourceStruct.ObjectArray, _target.OUTER_ST.OBJECT_ARRAY);
                    Assert.Equal(_source.InnerSourceStruct.ObjectList, _target.OUTER_ST.OBJECT_LIST);
                    Assert.Equal(_source.LongArray, _target.LONG_ARRAY);
                    Assert.Equal(_source.LongList, _target.LONG_LIST);
                    Assert.True(_source.LongNumber == _target.LONG);
                    Assert.True(_source.Name == _target.STRING);
                    Assert.Equal(_source.NameArray, _target.STRING_ARRAY);
                    Assert.Equal(_source.NameList, _target.STRIN_LIST);
                    Assert.True(_source.Number == _target.INT);
                    Assert.Equal(_source.NumberArray, _target.INT_ARRAY);
                    Assert.Equal(_source.NumberList, _target.INT_LIST);
                    Assert.True(_source.ToGuidValue == _target.STRING_GUID.ToString());
                    Assert.True(_source.ToStringValue.ToString() == _target.GUID_STRING);
                    Assert.Same(_source.RecordProperty.Object, _target.RECORD.OBJECT);
                    Assert.Equal(_source.RecordProperty.ObjectArray, _target.RECORD.OBJECT_ARRAY);
                    Assert.Equal(_source.RecordProperty.ObjectList, _target.RECORD.OBJECT_LIST);
                }
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Records_Using_IMapper(int sizes)
        {
            IList<SourceRecord> source = DataGenerator.GenerateSourceRecords(sizes);
            IList<TargetRecord> target = IRecordMapper.From(source);

            Assert.NotNull(target);

            foreach (SourceRecord _source in source)
            {
                Assert.Contains(target, t => t.GUID == _source.Id);
                foreach (TargetRecord _target in target.Where(t => t.GUID == _source.Id).ToList())
                {
                    Assert.True(_source.Id == _target.GUID);
                    Assert.True(_source.Character == _target.CHAR);
                    Assert.True(_source.Money == _target.DECIMAL);
                    Assert.Equal(_source.DecimalArray, _target.DECIMAL_ARRAY);
                    Assert.Equal(_source.DecimalList, _target.DECIMAL_LIST);
                    Assert.True(_source.Currency == _target.DOUBLE);
                    Assert.Equal(_source.DoubleArray, _target.DOUBLE_ARRAY);
                    Assert.Equal(_source.DoubleList, _target.DOUBLE_LIST);
                    Assert.True(_source.Percentage == _target.FLOAT);
                    Assert.Equal(_source.FloatArray, _target.FLOAT_ARRAY);
                    Assert.Equal(_source.FloatList, _target.FLOAT_LIST);
                    Assert.Same(_source.InnerSource.Object, _target.OUTER.OBJECT);
                    Assert.Equal(_source.InnerSource.ObjectArray, _target.OUTER.OBJECT_ARRAY);
                    Assert.Equal(_source.InnerSource.ObjectList, _target.OUTER.OBJECT_LIST);
                    Assert.Same(_source.JustNullInner, _target.NULL_OUTER);
                    Assert.Same(_source.JustNullInnerArray, _target.NULL_OUTER_ARRAY);
                    Assert.Same(_source.JustNullInnerList, _target.NULL_OUTER_LIST);
                    Assert.Same(_source.InnerSource.Object, _target.OUTER.OBJECT);
                    Assert.Equal(_source.InnerSource.ObjectArray, _target.OUTER.OBJECT_ARRAY);
                    Assert.Equal(_source.InnerSource.ObjectList, _target.OUTER.OBJECT_LIST);
                    Assert.Equal(_source.LongArray, _target.LONG_ARRAY);
                    Assert.Equal(_source.LongList, _target.LONG_LIST);
                    Assert.True(_source.LongNumber == _target.LONG);
                    Assert.True(_source.Name == _target.STRING);
                    Assert.Equal(_source.NameArray, _target.STRING_ARRAY);
                    Assert.Equal(_source.NameList, _target.STRIN_LIST);
                    Assert.True(_source.Number == _target.INT);
                    Assert.Equal(_source.NumberArray, _target.INT_ARRAY);
                    Assert.Equal(_source.NumberList, _target.INT_LIST);
                    Assert.True(_source.ToGuidValue == _target.STRING_GUID.ToString());
                    Assert.True(_source.ToStringValue.ToString() == _target.GUID_STRING);
                    Assert.Same(_source.ClassProperty.Object, _target.CLASS.OBJECT);
                    Assert.Equal(_source.ClassProperty.ObjectArray, _target.CLASS.OBJECT_ARRAY);
                    Assert.Equal(_source.ClassProperty.ObjectList, _target.CLASS.OBJECT_LIST);
                    Assert.Same(_source.StructProperty.Object, _target.STRUCT.OBJECT);
                    Assert.Equal(_source.StructProperty.ObjectArray, _target.STRUCT.OBJECT_ARRAY);
                    Assert.Equal(_source.StructProperty.ObjectList, _target.STRUCT.OBJECT_LIST);
                }
            }
        }

        [Fact]
        public void Can_Map_DataString_Values_Using_IMapper()
        {
            SourceToString source = DataGenerator.GenerateSourceToString();
            TargetToString target = StringIMapper.From(source);

            Assert.NotNull(target);
        }

        [Fact]
        public void Can_Map_DataString_Values_Using_Extension_Methods()
        {
            SourceToString source = DataGenerator.GenerateSourceToString();
            TargetToString target = source.MapTo<TargetToString>();

            Assert.NotNull(target);
        }
    }
}