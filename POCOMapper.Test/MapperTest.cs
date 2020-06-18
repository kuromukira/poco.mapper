using System;
using POCO.Mapper.Extension;
using System.Collections.Generic;
using Xunit;

namespace POCO.Mapper.Test
{
    public class MapperTest
    {
        private DataGenerator DataGenerator { get; } = new DataGenerator();
        private IMapper<TargetModel, SourceModel> lMapper { get; } = new ModelMapper<TargetModel, SourceModel>();
        private IMapper<TargetToString, SourceToString> lStringMapper { get; } = new ModelMapper<TargetToString, SourceToString>();
        private IMapper<MultipleMapTargetModel, MultipleMapSourceModel> lMultiFieldMapper { get; } = new ModelMapper<MultipleMapTargetModel, MultipleMapSourceModel>();

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Values_Using_IMapper(int sizes)
        {
            IList<SourceModel> source = DataGenerator.GenerateSourceModels(sizes);
            IList<TargetModel> target = lMapper.From(source);

            Assert.NotNull(target);
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
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_MultiField_Values_Using_IMapper(int sizes)
        {
            IList<MultipleMapSourceModel> source = DataGenerator.GenerateMultiFieldSourceModels(sizes);
            IList<MultipleMapTargetModel> target = lMultiFieldMapper.From(source);

            Assert.NotNull(target);
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
        }

        [Fact]
        public void Can_Map_DataString_Values_Using_IMapper()
        {
            SourceToString source = DataGenerator.GenerateSourceToString();
            TargetToString target = lStringMapper.From(source);

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