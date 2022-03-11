using System;

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

    /// <summary>Mapper Attribute Target</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreIf : Attribute
    {
        /// <summary></summary>
        public Type TargetType { get; }

        /// <summary>Ignore this property when mapping to the given target type</summary>
        /// <param name="type">Target Type</param>
        public IgnoreIf(Type type) => TargetType = type;
    }
}