using GKsLib.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace GithubInfo
{
	[ConfigPath(DirectoryName = @".\conf\", FileName = nameof(AppSettings) + ".conf")]
	[ConfigFormatType(ConfigType = ConfigType.JSON)]
	[DataContract]
	public class AppSettings : ConfigBase<AppSettings>
	{
		public static readonly AppSettings Instance = Load();

		protected override void OnPropertyChanging(PropertyChangingEventArgs args)
		{
			Trace.WriteLine(args.Name);
			base.OnPropertyChanging(args);
		}

		protected override void OnLoaded(EventArgs args)
		{
			// ファイルがない初期状態のときに null が嫌ならこんな感じで適当に
			ModulesDirectory ??= Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			ModulesNames ??= new List<string> { "GKsWpfPrismLib.Module.Base.dll" };
			base.OnLoaded(args);
		}

		[DataMember]
		public string ModulesDirectory
		{
			get { return Get(a => a.ModulesDirectory); }
			set { Set(a => a.ModulesDirectory, value); }
		}

		[DataMember]
		public List<string> ModulesNames
		{
			get { return Get(a => a.ModulesNames); }
			set { Set(a => a.ModulesNames, value); }
		}
	}
}
