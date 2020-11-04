using System;
using System.Linq;
using System.Reflection;

namespace GKsLib.Extensions
{
	public static class AssemblyExtension
	{
		/// <summary>
		/// アセンブリ内の指定されたインターフェイスが実装されているすべての Type を返却
		/// </summary>

		public static Type[] GetInterfaces<T>(this Assembly asm)
		{
			return asm.GetTypes().Where(c => c.GetInterfaces().Any(t => t == typeof(T))).ToArray();
		}

		/// <summary>
		/// アセンブリ内の指定されたインターフェイスが実装されているすべての Type を返却
		/// </summary>
		public static Type[] GetInterfaces(this Assembly asm, string name)
		{
			return asm.GetTypes().Where(c => c.GetInterfaces().Any(t => nameof(t) == name)).ToArray();
		}

		/// <summary>
		/// アセンブリ内の指定されたインターフェイスが実装されているすべての Type を返却
		/// </summary>
		public static Type[] GetInterfaces(this Assembly asm, Type type)
		{
			return asm.GetTypes().Where(c => c.GetInterfaces().Any(t => t == type)).ToArray();
		}
		/// <summary>
		/// アセンブリ内の指定されたインターフェイスが実装されているすべての Type のインスタンスを作成して返却
		/// </summary>
		public static T[] CreateInterfaceInstances<T>(this Assembly asm) where T : class
		{
			return GetInterfaces<T>(asm).Select(c => Activator.CreateInstance(c) as T).ToArray();
		}
	}
}
