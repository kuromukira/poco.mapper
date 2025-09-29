using POCO.Mapper.Common;
using System.Collections.Generic;
using System.Linq;

namespace POCO.Mapper;

///<summary>Map values from S to T and/or vice versa</summary>
///<typeparam name="T">Target Type</typeparam>
///<typeparam name="S">Source Type</typeparam>
public interface IMapper<T, S>
{
	///<summary>Map values from S to T</summary>
	T? From(S source);

	///<summary>Map values from T to S</summary>
	S? From(T target);

	///<summary>Map list from S to T</summary>
	IList<T?> From(IList<S> source);

	///<summary>Map list from T to S</summary>
	IList<S?> From(IList<T> target);
}

///<summary>Map values from S to T and/or vice versa</summary>
///<typeparam name="T">Target Type</typeparam>
///<typeparam name="S">Source Type</typeparam>
public class ModelMapper<T, S> : IMapper<T, S>
{
	private readonly ModelMapperCore _core = new();

	T? IMapper<T, S>.From(S? source) => (T?)_core.Map(source, typeof(T));

	S? IMapper<T, S>.From(T? target) => (S?)_core.Map(target, typeof(S));

	IList<T?> IMapper<T, S>.From(IList<S> source)
	{
		if (source.Count == 0)
			return [];

		IMapper<T, S> mapper = this;

		List<T?> result = new(source.Count);
		result.AddRange(source.Select(t => mapper.From(t)));

		return result;
	}

	IList<S?> IMapper<T, S>.From(IList<T> target)
	{
		if (target.Count == 0)
			return [];

		IMapper<T, S> mapper = (IMapper<T, S>)this;

		List<S?> result = new(target.Count);
		result.AddRange(target.Select(t => mapper.From(t)));

		return result;
	}
}