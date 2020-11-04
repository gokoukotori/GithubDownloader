using GKsLib.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GithubInfo.Modules.StarredGithubDownloader
{
	[ConfigPath(DirectoryName = @".\conf\", FileName = nameof(StarredGithubDownloaderSetteings) + ".conf")]
	[ConfigFormatType(ConfigType = ConfigType.JSON)]
	[DataContract]
	public class StarredGithubDownloaderSetteings : ConfigBase<StarredGithubDownloaderSetteings>
	{
		public static readonly StarredGithubDownloaderSetteings Instance = Load();

		protected override void OnPropertyChanging(PropertyChangingEventArgs args)
		{
			Trace.WriteLine(args.Name);
			base.OnPropertyChanging(args);
		}

		protected override void OnLoaded(EventArgs args)
		{
			// ファイルがない初期状態のときに null が嫌ならこんな感じで適当に
			UserName ??= "";
			Token ??= "";
			base.OnLoaded(args);
		}

		[DataMember]
		public string UserName
		{
			get { return Get(a => a.UserName); }
			set { Set(a => a.UserName, value); }
		}

		[DataMember]
		public string Token
		{
			get { return Get(a => a.Token); }
			set { Set(a => a.Token, value); }
		}
	}
}
