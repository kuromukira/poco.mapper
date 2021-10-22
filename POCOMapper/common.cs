using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace POCO.Mapper.Common
{
    internal class ModelMapperCommon
    {
        public object Map(object toConvert, Type targetType)
        {
            if (toConvert is null)
                return null;
            object output = Activator.CreateInstance(targetType);
            foreach (PropertyInfo convertProp in toConvert.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // * Get ToString format attribute
                object[] formatAttribute = convertProp.GetCustomAttributes(typeof(UseFormat), true);
                UseFormat useFormat = ((UseFormat) (formatAttribute?.FirstOrDefault() ?? new UseFormat(string.Empty)));

                // * Get custom attribute name
                object[] mappedToAttribute = convertProp.GetCustomAttributes(typeof(MappedTo), true);
                MappedTo[] mappedToNames = ((MappedTo[]) (mappedToAttribute ?? new MappedTo[] { }));
                foreach (MappedTo mappedName in mappedToNames)
                {
                    // * Loop only through all properties that matched the target mapped name
                    foreach (PropertyInfo outputProp in output.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.Name.Equals(mappedName.Name)).ToList())
                    {
                        // * Validation for type mismatch except for collections
                        if (!outputProp.PropertyType.IsAssignableFrom(convertProp.PropertyType)
                            && !IsGuidMapping(convertProp.PropertyType, outputProp.PropertyType)
                            && !IsCustomType(outputProp.PropertyType)
                            && !IsCustomValueType(outputProp.PropertyType)
                            && !typeof(IEnumerable).IsAssignableFrom(outputProp.PropertyType))
                            throw new IMapperException($"The source type ({convertProp.PropertyType.Name}) could not be converted to the target type ({outputProp.PropertyType.Name}).");

                        // * Check if Guid to String mapping or vice versa
                        else if (IsGuidMapping(convertProp.PropertyType, outputProp.PropertyType))
                        {
                            // * Check source if Guid then convert to string
                            if (convertProp.PropertyType == typeof(Guid) && outputProp.PropertyType == typeof(string))
                            {
                                Guid sourceValue = (Guid) convertProp.GetValue(toConvert);
                                outputProp.SetValue(output, sourceValue == Guid.Empty ? string.Empty : sourceValue.ToString());
                            }
                            // * Check source if string then  convert to Guid
                            else if (convertProp.PropertyType == typeof(string) && outputProp.PropertyType == typeof(Guid))
                            {
                                string sourceValue = convertProp.GetValue(toConvert)?.ToString() ?? string.Empty;
                                Guid.TryParse(sourceValue, out Guid guidValue);
                                outputProp.SetValue(output, guidValue);
                            }
                        }

                        // * Check if Enum
                        else if (outputProp.PropertyType.IsEnum)
                            outputProp.SetValue(output, Enum.ToObject(outputProp.PropertyType, convertProp.GetValue(toConvert)));

                        // * Check if IEnumerable (eg IList, List and Arrays)
                        else if (outputProp.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(outputProp.PropertyType))
                        {
                            IEnumerable collection = (IEnumerable) convertProp.GetValue(toConvert, null);
                            if (!(collection is null))
                            {
                                // * Define and check the target output list
                                Type constructedListType = typeof(List<>).MakeGenericType(
                                    !outputProp.PropertyType.IsArray ? outputProp.PropertyType.GetGenericArguments()[0] : outputProp.PropertyType.GetElementType()
                                );
                                if (constructedListType is null)
                                    throw new IMapperException("POCO.Mapper encountered an error with " + outputProp.PropertyType.Name);
                                IList finalList = (IList) Activator.CreateInstance(constructedListType);
                                bool isInnerElementCustom = !outputProp.PropertyType.IsArray ? IsCustomType(outputProp.PropertyType.GetGenericArguments()[0]) : IsCustomType(outputProp.PropertyType.GetElementType());

                                // * Loop through the objects to be mapped
                                foreach (object obj in collection)
                                {
                                    // * For custom types
                                    if (isInnerElementCustom)
                                    {
                                        // * Call method again to map list objects
                                        object result = Map(obj, !outputProp.PropertyType.IsArray ? outputProp.PropertyType.GetGenericArguments()[0] : outputProp.PropertyType.GetElementType()
                                        );
                                        finalList.Add(result);
                                    }
                                    // * For native types
                                    else finalList.Add(obj);
                                }

                                // * Assign to target property
                                if (outputProp.PropertyType.IsArray)
                                {
                                    Array arrayList = Array.CreateInstance(outputProp.PropertyType.GetElementType(), finalList.Count);
                                    for (int i = 0; i < finalList.Count; i++)
                                        arrayList.SetValue(Convert.ChangeType(finalList[i], outputProp.PropertyType.GetElementType()), i);
                                    outputProp.SetValue(output, arrayList);
                                }
                                else outputProp.SetValue(output, finalList);
                            }
                        }

                        // * Check if a custom type
                        else if (IsCustomType(outputProp.PropertyType))
                            outputProp.SetValue(output, Map(convertProp.GetValue(toConvert), outputProp.PropertyType));

                        // * Check if a custom value type
                        else if (IsCustomValueType(outputProp.PropertyType))
                            outputProp.SetValue(output, Map(convertProp.GetValue(toConvert), outputProp.PropertyType));

                        // * Default
                        else
                        {
                            if (convertProp.PropertyType != typeof(string) && outputProp.PropertyType == typeof(string))
                            {
                                if (string.IsNullOrWhiteSpace(useFormat.Format))
                                    outputProp.SetValue(output, convertProp.GetValue(toConvert).ToString());
                                else
                                {
                                    if (Regex.IsMatch(useFormat.Format, @"\{0:(.*?)\}"))
                                        outputProp.SetValue(output, string.Format(useFormat.Format, convertProp.GetValue(toConvert)));
                                    else
                                    {
                                        string formatUsed = $"{{0:{useFormat.Format}}}";
                                        outputProp.SetValue(output, string.Format(formatUsed, convertProp.GetValue(toConvert)));
                                    }
                                }
                            }
                            else outputProp.SetValue(output, convertProp.GetValue(toConvert));
                        }

                        break;
                    }
                }
            }

            return output;

            bool IsCustomType(Type outputType) => !outputType.IsPrimitive && outputType.IsClass && !outputType.IsAbstract && outputType != typeof(string) && outputType != typeof(object);

            bool IsCustomValueType(Type outputType) => outputType.IsValueType && !outputType.IsPrimitive && outputType.Namespace != null;

            bool IsGuidMapping(Type sourceType, Type converType) => ((sourceType == typeof(string) && converType == typeof(Guid)) ||
                (converType == typeof(string) && sourceType == typeof(Guid)));
        }
    }
}