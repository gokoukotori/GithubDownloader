using GithubInfo.Modules.StarredGithubDownloader.Views;
using GithubInfo.Services.Github;
using GithubInfo.Services.Github.Interface;
using LibGit2Sharp;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GithubInfo.Modules.StarredGithubDownloader
{
	public class StarredGithubDownloaderModule : IModule
	{

		private readonly IRegionManager _regionManager;


		public StarredGithubDownloaderModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}
		public void OnInitialized(IContainerProvider containerProvider)
		{
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IGithubClientFactoryService, GithubClientFactoryService>();

			containerRegistry.RegisterForNavigation<GithubLoginUsePersonalAccessToken>();
			containerRegistry.RegisterForNavigation<GithubStarredInfo>();
			containerRegistry.RegisterForNavigation<ConfirmPersonalAccessTokenDialog>();

		}
	}
}