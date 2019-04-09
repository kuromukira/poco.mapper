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

    ///<summary>Static helper class to convert list to arrays</summary>
    internal static class ListExtensions
    {
        public static T[] ConvertToArray<T>(IList list)
        {
            return list.Cast<T>().ToArray();
        }

        public static object[] ConvertToArrayRuntime(IList list, Type elementType)
        {
            var convertMethod = typeof(ListExtensions).GetMethod("ConvertToArray", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(IList) }, null);
            var genericMethod = convertMethod.MakeGenericMethod(elementType);
            return (object[])genericMethod.Invoke(null, new object[] { list });
        }
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
                        // * Validation for type mismatch except for collections
                        if (!_outputProp.PropertyType.IsAssignableFrom(_convertProp.PropertyType) && !isCustomType(_outputProp.PropertyType)
                            && !typeof(IEnumerable).IsAssignableFrom(_outputProp.PropertyType))
                            throw new IMapperException(string.Format("The source type ({0}) could not be converted to the target type ({1}).", _convertProp.PropertyType.Name, _outputProp.PropertyType.Name));

                        // * Check if Enum
                        else if (_outputProp.PropertyType.IsEnum)
                            _outputProp.SetValue(_output, Enum.ToObject(_outputProp.PropertyType, _convertProp.GetValue(toConvert)));

                        // * Check if IEnumerable (eg IList, List and Arrays)
                        else if (_outputProp.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(_outputProp.PropertyType))
                        {
                            var _collection = (IEnumerable)_convertProp.GetValue(toConvert, null);
                            if (!(_collection is null))
                            {
                                // * Define and check the target output list
                                var _constructedListType = typeof(List<>).MakeGenericType(
                                    !_outputProp.PropertyType.IsArray ?
                                    _outputProp.PropertyType.GetGenericArguments()[0] :
                                    _outputProp.PropertyType.GetElementType()
                                );
                                if (_constructedListType is null)
                                    throw new IMapperException("POCO.Mapper encountered an error with " + _outputProp.PropertyType.Name);
                                var _finalList = (IList)Activator.CreateInstance(_constructedListType);
                                bool isInnerElementCustom = !_outputProp.PropertyType.IsArray ?
                                    isCustomType(_outputProp.PropertyType.GetGenericArguments()[0]) :
                                    isCustomType(_outputProp.PropertyType.GetElementType());

                                // * Loop through the objects to be mapped
                                foreach (object _obj in _collection)
                                {
                                    // * For custom types
                                    if (isInnerElementCustom)
                                    {
                                        // * Call method again to map list objects
                                        object _result = map(_obj, !_outputProp.PropertyType.IsArray ?
                                            _outputProp.PropertyType.GetGenericArguments()[0] : _outputProp.PropertyType.GetElementType()
                                        );
                                        _finalList.Add(_result);
                                    }
                                    // * For native types
                                    else _finalList.Add(_obj);
                                }

                                // * Assign to target property
                                if (_outputProp.PropertyType.IsArray)
                                {
                                    var _arrayList = Array.CreateInstance(_outputProp.PropertyType.GetElementType(), _finalList.Count);
                                    for (int i = 0; i < _finalList.Count; i++)
                                        _arrayList.SetValue(Convert.ChangeType(_finalList[i], _outputProp.PropertyType.GetElementType()), i);
                                    _outputProp.SetValue(_output, _arrayList);
                                }
                                else _outputProp.SetValue(_output, _finalList);
                            }
                        }

                        // * Check if a custom type
                        else if (isCustomType(_outputProp.PropertyType))
                            _outputProp.SetValue(_output, map(_convertProp.GetValue(toConvert), _outputProp.PropertyType));

                        // * Default
                        else _outputProp.SetValue(_output, _convertProp.GetValue(toConvert));
                        break;
                    }
                }
            }
            return _output;

            bool isCustomType(Type outputType)
            {
                return (!outputType.IsPrimitive && outputType.IsClass && !outputType.IsAbstract && outputType != typeof(string));
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