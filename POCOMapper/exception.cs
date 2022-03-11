using System;

namespace POCO.Mapper
{
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
}