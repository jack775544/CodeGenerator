using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Core.Utility
{
	public class ServiceFactory
	{
		private readonly List<Func<IServiceProvider, IEnumerable<object>>> _factories = new();

		public void AddFactory(Func<IServiceProvider, IEnumerable<object>> factory)
		{
			_factories.Add(factory);
		}

		public IEnumerable<IEnumerable<object>> Invoke(IServiceProvider serviceProvider)
		{
			return _factories.Select(x => x(serviceProvider));
		}
	}
}