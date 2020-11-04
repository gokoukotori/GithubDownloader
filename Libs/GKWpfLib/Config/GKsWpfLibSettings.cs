using GKsLib.Configuration;
using GKsWpfLib.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKsWpfLib.Config
{
	[ConfigPath(DirectoryName = @".\conf\", FileName = nameof(GKsWpfLibSettings) + ".conf")]
	[ConfigFormatType(ConfigType = ConfigType.JSON)]
	[DataContract]
	public class GKsWpfLibSettings : ConfigBase<GKsWpfLibSettings>
	{
		public static readonly GKsWpfLibSettings Instance = Load();

		protected override void OnPropertyChanging(PropertyChangingEventArgs args)
		{
			Trace.WriteLine(args.Name);
			base.OnPropertyChanging(args);
		}

		protected override void OnLoaded(EventArgs args)
		{
			// ファイルがない初期状態のときに null が嫌ならこんな感じで適当に
			ApplicationTheme ??= ApplicationThemeData.ColorList().Last().Name;
			base.OnLoaded(args);
		}

		[DataMember]
		public string ApplicationTheme
		{
			get { return Get(a => a.ApplicationTheme); }
			set { Set(a => a.ApplicationTheme, value); }
		}
	}


}
