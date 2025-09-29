using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace POCO.Mapper.Common;

internal class ModelMapperCore
{
	public object? Map(object? toConvert, Type? targetType)
	{
		if (toConvert is null)
			return null;

		if (targetType is null)
			return null;
		
		object output = Activator.CreateInstance(targetType);
		foreach (PropertyInfo convertProp in toConvert.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
		{
			object[] formatAttribute = convertProp.GetCustomAttributes(typeof(UseFormat), true);
			UseFormat useFormat = ((UseFormat)(formatAttribute.FirstOrDefault() ?? new UseFormat(string.Empty)));

			object[] ignoreIfAttribute = convertProp.GetCustomAttributes(typeof(IgnoreIf), true);
			IgnoreIf[] ignoreIfTypes = ignoreIfAttribute.OfType<IgnoreIf>().ToArray();

			object[] mappedToAttribute = convertProp.GetCustomAttributes(typeof(MappedTo), true);
			MappedTo[] mappedToNames = mappedToAttribute.OfType<MappedTo>().ToArray();

			foreach (MappedTo mappedName in mappedToNames.Distinct().ToList())
			{
				if (ignoreIfTypes.ToList().Any(ignore => ignore.TargetType == output.GetType()))
					continue;

				foreach (PropertyInfo outputProp in output.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.Name.Equals(mappedName.Name)).ToList())
				{
					if (!outputProp.PropertyType.IsAssignableFrom(convertProp.PropertyType)
						&& !IsGuidMapping(convertProp.PropertyType, outputProp.PropertyType)
						&& !IsCustomType(outputProp.PropertyType)
						&& !IsCustomValueType(outputProp.PropertyType)
						&& !typeof(IEnumerable).IsAssignableFrom(outputProp.PropertyType))
						throw new IMapperException($"The source type ({convertProp.PropertyType.Name}) could not be converted to the target type ({outputProp.PropertyType.Name}).");

					if (IsGuidMapping(convertProp.PropertyType, outputProp.PropertyType))
					{
						if (convertProp.PropertyType == typeof(Guid) && outputProp.PropertyType == typeof(string))
						{
							Guid sourceValue = (Guid)convertProp.GetValue(toConvert);
							outputProp.SetValue(output, sourceValue == Guid.Empty ? string.Empty : sourceValue.ToString());
						}
						else if (convertProp.PropertyType == typeof(string) && outputProp.PropertyType == typeof(Guid))
						{
							string sourceValue = convertProp.GetValue(toConvert)?.ToString() ?? string.Empty;
							Guid.TryParse(sourceValue, out Guid guidValue);
							outputProp.SetValue(output, guidValue);
						}
						else
						{
							Guid sourceValue = (Guid)convertProp.GetValue(toConvert);
							outputProp.SetValue(output, sourceValue);
						}
					}

					else if (outputProp.PropertyType.IsEnum || (Nullable.GetUnderlyingType(outputProp.PropertyType)?.IsEnum == true))
					{
						Type? targetEnumType = outputProp.PropertyType.IsEnum 
							? outputProp.PropertyType 
							: Nullable.GetUnderlyingType(outputProp.PropertyType);
						
						if (targetEnumType is null)
							continue;
						
						bool isNullable = !outputProp.PropertyType.IsEnum;
						
						object? sourceValue = convertProp.GetValue(toConvert);
						
						if (sourceValue is null)
						{
							if (isNullable)
								outputProp.SetValue(output, null);
							continue;
						}
						
						object? enumValue = null;
						bool hasValidValue = false;
						
						if (sourceValue is string stringValue)
						{
							if (Enum.TryParse(targetEnumType, stringValue, ignoreCase: true, out object? parsedValue))
							{
								enumValue = parsedValue;
								hasValidValue = true;
							}
						}
						else
						{
							try
							{
								Type underlyingType = Enum.GetUnderlyingType(targetEnumType);
								object convertedValue = Convert.ChangeType(sourceValue, underlyingType);
								enumValue = Enum.ToObject(targetEnumType, convertedValue);
								hasValidValue = true;
							}
							catch
							{
							}
						}
						
						if (hasValidValue && enumValue is not null)
						{
							outputProp.SetValue(output, enumValue);
						}
					}

					else if (outputProp.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(outputProp.PropertyType))
					{
						IEnumerable collection = (IEnumerable)convertProp.GetValue(toConvert, null);
						if (!(collection is null))
						{
							Type? elementType;
							if (outputProp.PropertyType.IsArray)
								elementType = outputProp.PropertyType.GetElementType();
							else if (outputProp.PropertyType.IsGenericType)
								elementType = outputProp.PropertyType.GetGenericArguments().FirstOrDefault();
							else
								elementType = typeof(object);

							if (elementType is null)
								throw new IMapperException($"POCO.Mapper could not determine element type for {outputProp.PropertyType.Name}");

							Type constructedListType = typeof(List<>).MakeGenericType(elementType);
							if (constructedListType is null)
								throw new IMapperException("POCO.Mapper encountered an error with " + outputProp.PropertyType.Name);
							IList finalList = (IList)Activator.CreateInstance(constructedListType);
							bool isInnerElementCustom = IsCustomType(elementType);

							foreach (object obj in collection)
							{
								if (isInnerElementCustom)
								{
									object? result = Map(obj, elementType);
									finalList.Add(result);
								}
								else
									finalList.Add(obj);
							}

							if (outputProp.PropertyType.IsArray)
							{
								Array arrayList = Array.CreateInstance(elementType, finalList.Count);
								for (int i = 0; i < finalList.Count; i++)
								{
									object value = finalList[i];
									if (value == null || elementType.IsInstanceOfType(value))
									{
										arrayList.SetValue(value, i);
									}
									else if (elementType.IsEnum)
									{
										if (value is string s)
											arrayList.SetValue(Enum.Parse(elementType, s), i);
										else
											arrayList.SetValue(Enum.ToObject(elementType, value), i);
									}
									else
									{
										arrayList.SetValue(Convert.ChangeType(value, elementType), i);
									}
								}
								outputProp.SetValue(output, arrayList);
							}
							else
								outputProp.SetValue(output, finalList);
						}
					}

					else if (IsCustomType(outputProp.PropertyType) || IsCustomValueType(outputProp.PropertyType))
						outputProp.SetValue(output, Map(convertProp.GetValue(toConvert), outputProp.PropertyType));

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
						else
							outputProp.SetValue(output, convertProp.GetValue(toConvert));
					}

					break;
				}
			}
		}

		return output;

		bool IsCustomType(Type outputType) => !outputType.IsPrimitive && outputType.IsClass && !outputType.IsAbstract && outputType != typeof(string) && outputType != typeof(object);

		bool IsCustomValueType(Type outputType)
		{
			if (!outputType.IsValueType || outputType.IsPrimitive || outputType.Namespace == null)
				return false;

			string asmName = outputType.Assembly.GetName().Name ?? string.Empty;

			if (asmName.StartsWith("System", StringComparison.Ordinal) || asmName.StartsWith("Microsoft", StringComparison.Ordinal))
				return false;

			if (outputType.Assembly == typeof(object).Assembly)
				return false;

			return true;
		}

		bool IsGuidMapping(Type sourceType, Type convertType) => (sourceType == typeof(string) && convertType == typeof(Guid)) ||
																 (convertType == typeof(string) && sourceType == typeof(Guid)) || (convertType == typeof(Guid) && sourceType == typeof(Guid));
	}
}