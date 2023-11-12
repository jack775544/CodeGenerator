#nullable disable
using System.Collections;
using java.util;

namespace Generator.Emf.Utility;

public class IteratorEnumerator : IEnumerator
{
	private readonly Iterator _iterator;

	public object Current { get; private set; }

	public IteratorEnumerator(Iterator iterator)
	{
		_iterator = iterator;
	}
	
	public bool MoveNext()
	{
		if (_iterator.hasNext())
		{
			Current = _iterator.next();
			return true;
		}

		Current = default;
		return false;
	}

	[Obsolete("Not compatible with Java iterators")]
	public void Reset()
	{
		throw new NotImplementedException("Not compatible with Java iterators");
	}

	public void Dispose()
	{
	}
}