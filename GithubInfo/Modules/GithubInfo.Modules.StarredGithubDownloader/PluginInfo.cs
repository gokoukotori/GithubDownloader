using GithubInfo.Modules.StarredGithubDownloader.Views;
using GKsWpfLib.Plugins;
using System;

namespace GithubInfo.Modules.StarredGithubDownloader
{
	public class PluginInfo : IVisualPlugin
	{
		public string Name => "Downloader";

		public string MainViewName => nameof(GithubLoginUsePersonalAccessToken);

		public string SettingViewName => nameof(GithubStarredOption);

		public string PluginThemeColor => "#32CD32";

		public string Icon => "FileDownload";

		public Type SettingViewType => typeof(GithubStarredOption);
	}
}
