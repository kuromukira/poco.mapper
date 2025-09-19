using POCO.Mapper.Common;
using System.Collections.Generic;

namespace POCO.Mapper;

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
		if (source is null || source.Count == 0)
			return [];

		IMapper<T, S> mapper = (IMapper<T, S>)this;

		List<T> result = new(source.Count);
		for (int i = 0; i < source.Count; i++)
			result.Add(mapper.From(source[i]));

		return result;
	}

	IList<S> IMapper<T, S>.From(IList<T> target)
	{
		if (target is null || target.Count == 0)
			return [];

		IMapper<T, S> mapper = (IMapper<T, S>)this;

		List<S> result = new(target.Count);
		for (int i = 0; i < target.Count; i++)
			result.Add(mapper.From(target[i]));

		return result;
	}
}