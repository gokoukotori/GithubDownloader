using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GKsLib.Configuration
{
	/// <summary>そのクラスをシリアライズまたはデシリアライズするときのファイル名を指定します。</summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ConfigPathAttribute : Attribute
	{
		#region Members

		/// <summary>ConfigFilePathAttribute クラスの新しいインスタンスを作成します。</summary>
		public ConfigPathAttribute()
		{
			DirectoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			FileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName) + DefaultExtension;
		}

		/// <summary>設定ファイルのあるディレクトリ名。</summary>
		public virtual string DirectoryName { get; set; }

		/// <summary>設定ファイル名。</summary>
		public virtual string FileName { get; set; }

		/// <summary>既定の拡張子。</summary>
		public virtual string DefaultExtension { get { return ".conf"; } }

		/// <summary>設定ファイルのフルパス。</summary>
		public virtual string FullPath { get { return DirectoryName + "\\" + FileName; } }

		#endregion
	}
}
