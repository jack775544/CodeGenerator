using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generator.Core.Utility;

public static class TypeHelpers
{
	private static readonly MethodInfo EnumerableCast = typeof(Enumerable).GetMethod("Cast")!;

	public static IEnumerable<Type> GetBaseTypes(Type type)
	{
		var types = new List<Type>();
		types.AddRange(type.GetInterfaces());

		var t = type;
		while (t.BaseType != null)
		{
			types.Add(t.BaseType);
			t = t.BaseType;
		}

		return types;
	}

	public static object CastRuntime(IEnumerable self, Type type)
	{
		return EnumerableCast.MakeGenericMethod(type).Invoke(null, new object[]{ self })!;
	}
}