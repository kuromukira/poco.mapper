using POCO.Mapper.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POCO.Mapper
{
    ///<summary>Map values from S to T and/or vice versa</summary>
    ///<typeparam name="T">Target Type</typeparam>
    ///<typeparam name="S">Source Type</typeparam>
    public interface IMapper<T, S>
    {
        ///<summary>Map values from S to T</summary>
        T From(S source);

        ///<summary>Map values from T to S</summary>
        S From(T target);

        ///<summary>Map list from S to T</summary>
        IList<T> From(IList<S> source);

        ///<summary>Map list from T to S</summary>
        IList<S> From(IList<T> target);
    }

    ///<summary>Map values from S to T and/or vice versa</summary>
    ///<typeparam name="T">Target Type</typeparam>
    ///<typeparam name="S">Source Type</typeparam>
    public class ModelMapper<T, S> : IMapper<T, S>
    {
        readonly ModelMapperCore Core = new ModelMapperCore();

        T IMapper<T, S>.From(S source) => (T)Core.Map(source, typeof(T));

        S IMapper<T, S>.From(T target) => (S)Core.Map(target, typeof(S));

        IList<T> IMapper<T, S>.From(IList<S> source)
        {
            if (!source.Any())
                return new List<T>();
            else
            {
                IMapper<T, S> mapper = new ModelMapper<T, S>();
                return source.Select(s => mapper.From(s)).ToList();
            }
        }

        IList<S> IMapper<T, S>.From(IList<T> target)
        {
            if (!target.Any())
                return new List<S>();
            else
            {
                IMapper<T, S> mapper = new ModelMapper<T, S>();
                return target.Select(t => mapper.From(t)).ToList();
            }
        }
    }
}