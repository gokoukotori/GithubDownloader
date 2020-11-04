using GKsWpfLib.Plugins;
using Prism.Modularity;
using System.Reflection;
using GKsLib.Extensions;
using System.Windows.Documents;
using System.Collections.Generic;

namespace GKsWpfLib.Modules
{
	public static class ModuleLoader
	{
		public static List<ModuleInfo> LoadModules(params string[] modulePaths)
		{
			var moduleInfo = new List<ModuleInfo>();
			foreach (var item in modulePaths)
			{
				var asm = Assembly.LoadFrom(item);
				var prismModule = asm.GetInterfaces<IModule>()[0];
				moduleInfo.Add(new ModuleInfo()
				{
					ModuleName = prismModule.Name,
					ModuleType = prismModule.AssemblyQualifiedName,
					InitializationMode = InitializationMode.WhenAvailable
				});
			}
			return moduleInfo;
		}


		public static IVisualPlugins LoadVisualPlugins(params string[] modulePaths)
		{
			var list = new VisualPlugins();
			foreach (var item in modulePaths)
			{
				var asm = Assembly.LoadFrom(item);
				var prismModule = asm.CreateInterfaceInstances<IVisualPlugin>()[0];

				list.Add(prismModule);

			}
			return list;
		}
	}
}
