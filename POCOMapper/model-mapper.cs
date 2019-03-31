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
        /// <summary></summary>
        public string Name { get; }
        /// <summary>Map to field</summary>
        /// <param name="name">Field Name</param>
        public MappedTo(string name) => Name = name;
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
                    // * Get custom attribute name
                    var _mappedTo = _convertProp.GetCustomAttributes(typeof(MappedTo), true).FirstOrDefault();
                    string _mappedToName = string.Empty;
                    if (_mappedTo != null)
                        _mappedToName = ((MappedTo)_mappedTo).Name;
                    if (!string.IsNullOrEmpty(_mappedToName) && _outputProp.Name.Equals(_mappedToName))
                    {
                        // * Validation for type mismatch
                        if (!_outputProp.PropertyType.IsAssignableFrom(_convertProp.PropertyType) && !isCustomeType(_outputProp))
                            throw new IMapperException(string.Format("The source type ({0}) could not be converted to the target type ({1}).", _convertProp.PropertyType.Name, _outputProp.PropertyType.Name));

                        // * Check if Enum
                        else if (_outputProp.PropertyType.IsEnum)
                            _outputProp.SetValue(_output, Enum.ToObject(_outputProp.GetType(), _convertProp.GetValue(toConvert)));

                        // * Check if IEnumerable (eg IList, List)
                        else if (_outputProp.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(_outputProp.PropertyType))
                        {
                            var _collection = (IEnumerable)_convertProp.GetValue(toConvert, null);
                            var _listType = typeof(List<>);

                            // * Define and check the target output list
                            var _constructedListType = _listType.MakeGenericType(_outputProp.PropertyType.GetGenericArguments());
                            if (_constructedListType is null)
                                throw new IMapperException("POCO.Mapper encountered an error with " + _outputProp.PropertyType.Name);
                            var _finalList = (IList)Activator.CreateInstance(_constructedListType);

                            // * Loop through the objects to be mapped
                            foreach (object _obj in _collection)
                            {
                                // * Call method again to map list objects
                                object _result = map(_obj, _outputProp.PropertyType.GetGenericArguments()[0]);
                                _finalList.Add(_result);
                            }
                            _outputProp.SetValue(_output, _finalList);
                        }

                        // * Check if a custom type
                        else if (isCustomeType(_outputProp))
                            _outputProp.SetValue(_output, map(_convertProp.GetValue(toConvert), _outputProp.PropertyType));

                        // * Default
                        else _outputProp.SetValue(_output, _convertProp.GetValue(toConvert));
                        break;
                    }
                }
            }
            return _output;

            bool isCustomeType(PropertyInfo outputProp)
            {
                return (!outputProp.PropertyType.IsPrimitive && outputProp.PropertyType.IsClass && !outputProp.PropertyType.IsAbstract && outputProp.PropertyType != typeof(string));
            }
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