using System.Collections;
using ikvm.lang;
using java.lang;
using java.util;

namespace Generator.Emf.Utility;

public static class IteratorExtensions
{
	/// <summary>
	/// Gets an enumerator for the provided enumerator. This enumerator is untyped.
	/// </summary>
	/// <param name="iterator">The iterator to get the enumerator for.</param>
	/// <returns>An enumerator that loops over the provided iterator.</returns>
	public static IEnumerator GetEnumerator(this Iterator iterator)
	{
		return new IteratorEnumerator(iterator);
	}

	/// <summary>
	/// Gets an enumerable containing containing elemes of type <typeparamref name="T"/>, throwing an exception if when
	/// encountering an element of a different type.
	/// </summary>
	/// <param name="iterator">The iterator to get an enumerable for.</param>
	/// <typeparam name="T">The type of the desired elements.</typeparam>
	/// <returns>An enumerable that only contains elements of the specified type.</returns>
	public static IEnumerable<T> GetEnumerable<T>(this Iterator iterator)
	{
		foreach (var item in iterator)
		{
			yield return (T)item;
		}
	}

	/// <summary>
	/// Gets an enumerable containing elements of type <typeparamref name=""/> name="T"/>, filtering out elements of
	/// different types.
	/// </summary>
	/// <param name="iterator">The iterator to get an enumerable for.</param>
	/// <typeparam name="T">The type of the desired elements.</typeparam>
	/// <returns>An enumerable that only contains elements of the specified type.</returns>
	public static IEnumerable<T> GetEnumerableOfType<T>(this Iterator iterator)
	{
		foreach (var item in iterator)
		{
			if (item is T typedItem)
			{
				yield return typedItem;
			}
		}
	}

	public static IEnumerator GetEnumerator(this Iterable iterable)
	{
		return new IterableEnumerator(iterable);
	}
	
	public static IEnumerable<T> GetEnumerable<T>(this Iterable iterable)
	{
		foreach (var item in iterable)
		{
			yield return (T)item;
		}
	}

	public static IEnumerable<T> GetEnumerableOfType<T>(this Iterable iterable)
	{
		foreach (var item in iterable)
		{
			if (item is T typedItem)
			{
				yield return typedItem;
			}
		}
	}
}