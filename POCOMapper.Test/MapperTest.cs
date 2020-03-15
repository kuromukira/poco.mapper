using POCO.Mapper.Extension;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace POCO.Mapper.Test
{
    public class MapperTest
    {
        private DataGenerator DataGenerator { get; } = new DataGenerator();
        private IMapper<TargetModel, SourceModel> lMapper { get; } = new ModelMapper<TargetModel, SourceModel>();
        private IMapper<MultipleMapTargetModel, MultipleMapSourceModel> lMultiFieldMapper { get; } = new ModelMapper<MultipleMapTargetModel, MultipleMapSourceModel>();

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Values_Using_IMapper(int sizes)
        {
            IList<SourceModel> source = DataGenerator.GenerateSourceModels(sizes);
            IList<TargetModel> target = lMapper.from(source);

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
            IList<MultipleMapTargetModel> target = lMultiFieldMapper.from(source);

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
    }
}