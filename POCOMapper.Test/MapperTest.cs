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
            IList<SourceModel> _source = DataGenerator.GenerateSourceModels(sizes);
            IList<TargetModel> _target = lMapper.from(_source);

            Assert.NotNull(_target);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_Values_Using_Extension_Methods(int sizes)
        {
            IList<SourceModel> _source = DataGenerator.GenerateSourceModels(sizes);
            TargetModel[] _target = _source.MapToArray<TargetModel>();

            Assert.NotNull(_target);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_MultiField_Values_Using_IMapper(int sizes)
        {
            IList<MultipleMapSourceModel> _source = DataGenerator.GenerateMultiFieldSourceModels(sizes);
            IList<MultipleMapTargetModel> _target = lMultiFieldMapper.from(_source);

            Assert.NotNull(_target);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void Can_Map_MultiField_Values_Using_Extension_Methods(int sizes)
        {
            IList<MultipleMapSourceModel> _source = DataGenerator.GenerateMultiFieldSourceModels(sizes);
            MultipleMapTargetModel[] _target = _source.MapToArray<MultipleMapTargetModel>();

            Assert.NotNull(_target);
        }
    }
}