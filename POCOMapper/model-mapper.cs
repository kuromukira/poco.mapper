using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace POCO.Mapper
{
    ///<summary>Map values from S to T and/or vice versa</summary>
    ///<typeparam name="T">Target Type</typeparam>
    ///<typeparam name="S">Source Type</typeparam>
    public interface IMapper<T, S>
    {
        ///<summary>Map values from S to T</summary>
        T from(S source);
        ///<summary>Map values from T to S</summary>
        S from(T target);
        ///<summary>Map list from S to T</summary>
        IList<T> from(IList<S> source);
        ///<summary>Map list from T to S</summary>
        IList<S> from(IList<T> target);
    }

    /// <summary>Mapper Attribute Target</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MappedTo : Attribute
    {
        private string lName { get; set; } = string.Empty;
        private Type lType { get; set; } = null;
        /// <summary>Map to field</summary>
        /// <param name="name">Field Name</param>
        public MappedTo(string name) { lName = name; }
        /// <summary>Map to a collection field</summary>
        /// <param name="name">Field Name</param>
        /// <param name="individualType">Individual Type for collection object</param>
        public MappedTo(string name, Type individualType) { lName = name; lType = individualType; }
        /// <summary></summary>
        public string Name { get { return lName; } }
        /// <summary></summary>
        public Type Type { get { return lType; } }
    }

    /// <summary></summary>
    public class IMapperException : Exception
    {
        /// <summary></summary>
        public IMapperException() { }
        /// <summary></summary>
        public IMapperException(string message) : base(message) { }
        /// <summary></summary>
        public IMapperException(string message, Exception inner) : base(message, inner) { }
    }

    ///<summary>Map values from S to T and/or vice versa</summary>
    ///<typeparam name="T">Target Type</typeparam>
    ///<typeparam name="S">Source Type</typeparam>
    public class ModelMapper<T, S> : IMapper<T, S>
    {
        private object map(object toConvert, Type targetType)
        {
            object _output = Activator.CreateInstance(targetType);
            foreach (PropertyInfo _convertProp in toConvert.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (PropertyInfo _outputProp in _output.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    string _propertyName = _convertProp.Name;
                    // * Get custom attribute name
                    var _mappedTo = _convertProp.GetCustomAttributes(typeof(MappedTo), true).FirstOrDefault();
                    if (_mappedTo != null)
                        _propertyName = ((MappedTo)_mappedTo).Name;
                    if (_outputProp.Name.Equals(_propertyName))
                    {
                        // * Check if Enum
                        if (_outputProp.GetType().IsEnum)
                            _outputProp.SetValue(_output, Enum.ToObject(_outputProp.GetType(), _convertProp.GetValue(toConvert)));
                        // * Check if IEnumerable (eg IList, List)
                        else if (_outputProp.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(_outputProp.PropertyType))
                        {
                            var _collection = (IEnumerable)_convertProp.GetValue(toConvert, null);
                            var _listType = typeof(List<>);
                            var _constructedListType = _listType.MakeGenericType(((MappedTo)_mappedTo).Type.GetGenericArguments());
                            if (_constructedListType == null)
                                throw new IMapperException("MappedTo.Type is required for collection property mapping. e.g MappedTo(\"Field\", typeof(List<MyClass>))");
                            else if (((MappedTo)_mappedTo).Type.IsGenericType && ((MappedTo)_mappedTo).Type.GetGenericTypeDefinition() == typeof(IList<>))
                                throw new IMapperException("IList<> is not supported for MappedTo.Type. Change it to List<> to avoid further errors.");
                            var _finalList = (IList)Activator.CreateInstance(_constructedListType);
                            foreach (object _obj in _collection)
                            {
                                // * Call method again to map list objects
                                object _result = map(_obj, ((MappedTo)_mappedTo).Type.GetGenericArguments()[0]);
                                _finalList.Add(_result);
                            }
                            _outputProp.SetValue(_output, _finalList);
                        }
                        // * Default
                        else _outputProp.SetValue(_output, _convertProp.GetValue(toConvert));
                        break;
                    }
                }
            }
            return _output;
        }

        T IMapper<T, S>.from(S source)
        {
            return (T)map(source, typeof(T));
        }

        S IMapper<T, S>.from(T target)
        {
            return (S)map(target, typeof(S));
        }

        IList<T> IMapper<T, S>.from(IList<S> source)
        {
            if (!source.Any())
                return new List<T>();
            else
            {
                IMapper<T, S> _mapper = new ModelMapper<T, S>();
                return source.Select(_b => _mapper.from(_b)).ToList();
            }
        }

        IList<S> IMapper<T, S>.from(IList<T> target)
        {
            if (!target.Any())
                return new List<S>();
            else
            {
                IMapper<T, S> _mapper = new ModelMapper<T, S>();
                return target.Select(_b => _mapper.from(_b)).ToList();
            }
        }
    }
}