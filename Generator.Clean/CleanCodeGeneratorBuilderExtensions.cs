using System;
using Generator.Core;
using Generator.Core.Metamodel;

namespace Generator.Clean
{
	public static class CleanCodeGeneratorBuilderExtensions
	{
		public static CodeGeneratorBuilder<T> AddFileCleanup<T>(
			this CodeGeneratorBuilder<T> builder,
			Action<CleanConfiguration> configureAction = null)
			where T : BaseModel
		{
			configureAction ??= _ => {};
			return builder
				.AddGenerationHook(new CleanHook())
				.AddScopedHelper<OldFileFinder>()
				.ConfigureOptions(configureAction);
		}
	}
}