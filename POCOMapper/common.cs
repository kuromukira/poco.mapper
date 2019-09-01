using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace POCO.Mapper.Common
{
    internal class ModelMapperCommon
    {
        public object Map(object toConvert, Type targetType)
        {
            if (toConvert is null)
                return toConvert;
            object _output = Activator.CreateInstance(targetType);
            foreach (PropertyInfo _convertProp in toConvert.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // * Get custom attribute name
                var _mappedTo = _convertProp.GetCustomAttributes(typeof(MappedTo), true);
                MappedTo[] _mappedToName = ((MappedTo[])(_mappedTo ?? new MappedTo[] { }));
                foreach (MappedTo _mappedName in _mappedToName)
                {
                    // * Loop only through all properties that matched the target mapped name
                    foreach (PropertyInfo _outputProp in _output.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.Name.Equals(_mappedName.Name)).ToList())
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
                                        object _result = Map(_obj, !_outputProp.PropertyType.IsArray ?
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
                            _outputProp.SetValue(_output, Map(_convertProp.GetValue(toConvert), _outputProp.PropertyType));

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
    }
}
