using POCO.Mapper.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POCO.Mapper
{
    /// <summary>Mapper Attribute Target</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MappedTo : Attribute
    {
        /// <summary></summary>
        public string Name { get; }

        /// <summary>Map to field</summary>
        /// <param name="name">Field Name</param>
        public MappedTo(string name) => Name = name;
    }

    /// <summary>String formatting</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UseFormat : Attribute
    {
        /// <summary></summary>
        public string Format { get; }

        /// <summary>Format to be used when mapping to a string</summary>
        /// <param name="format">String format</param>
        public UseFormat(string format) => Format = format;
    }

    /// <summary></summary>
    public class IMapperException : Exception
    {
        /// <summary></summary>
        public IMapperException()
        {
        }

        /// <summary></summary>
        public IMapperException(string message) : base(message)
        {
        }

        /// <summary></summary>
        public IMapperException(string message, Exception inner) : base(message, inner)
        {
        }
    }

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
        readonly ModelMapperCommon Common = new ModelMapperCommon();

        T IMapper<T, S>.From(S source) => (T) Common.Map(source, typeof(T));

        S IMapper<T, S>.From(T target) => (S) Common.Map(target, typeof(S));

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