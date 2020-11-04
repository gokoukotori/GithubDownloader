using GithubInfo.Services.Github.Interface;
using GKsWpfLib.Mvvm;
using Octokit;
using Prism.Commands;
using Prism.Mvvm;
using LibGit2Sharp;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Reactive.Bindings.Extensions;

namespace GithubInfo.Modules.StarredGithubDownloader.ViewModels
{
	public class GithubStarredInfoViewModel : RegionViewModelBase
	{
		public ReactiveCollection<TargetDownload> TargetDownloads { get; set; }
		public AsyncReactiveCommand DownloadCommand { get; }
		public AsyncReactiveCommand CreateListCommand { get; }

		public ReactiveProperty<string> TargetDirectory { get; set; }
		public ReactiveProperty<string> Log { get; set; }
		public ReactiveProperty<string> DownloadingDialogText { get; set; }
		public ReactiveProperty<bool> IsDownloading { get; set; }
		private readonly IGithubClientFactoryService _GithubClientFactoryService;
		private IGitHubClient _GitHubClient;

		public GithubStarredInfoViewModel(IRegionManager regionManager, IGithubClientFactoryService githubClientFactoryService) : base(regionManager)
		{
			_GithubClientFactoryService = githubClientFactoryService;
			TargetDownloads = new ReactiveCollection<TargetDownload>().AddTo(Disposable);
			CreateListCommand = new AsyncReactiveCommand().AddTo(Disposable);
			CreateListCommand.Subscribe(CreateList);
			DownloadCommand = new AsyncReactiveCommand().AddTo(Disposable);
			DownloadCommand.Subscribe(Download);
			TargetDirectory = new ReactiveProperty<string>
			{
				Value = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName
			}.AddTo(Disposable);
			Log = new ReactiveProperty<string>
			{
				Value = ""
			}.AddTo(Disposable);
			DownloadingDialogText = new ReactiveProperty<string>
			{
				Value = ""
			}.AddTo(Disposable);
			IsDownloading = new ReactiveProperty<bool>().AddTo(Disposable);
		}

		private async Task Download()
		{
			Log.Value = "";
			IsDownloading.Value = true;
			DownloadingDialogText.Value = "ダウンロード中です。";
			foreach (var item in TargetDownloads)
			{
				if (!item.IsSelected) continue;
				Log.Value = item.Repository.CloneUrl;
				await Task.Run(() => {
					LibGit2Sharp.Repository.Clone(item.Repository.CloneUrl, Path.Combine(TargetDirectory.Value, item.Repository.FullName));
					return Task.CompletedTask;
				});
			}
			IsDownloading.Value = false;
		}

		private async Task CreateList()
		{
			IsDownloading.Value = true;
			Log.Value = "";
			if (TargetDownloads.Count == 0)
			{
				DownloadingDialogText.Value = "データダウンロード中です。";
				var test = await _GitHubClient.Activity.Starring.GetAllForCurrent();
				DownloadingDialogText.Value = "リスト生成中です。";
				for (int i = 0; i < test.Count; i++)
				{
					//Log.Value = "aaa";
					Log.Value = (i + 1) + "/" + test.Count;
					TargetDownloads.AddOnScheduler(new TargetDownload(false, test[i]));
				}
			}
			IsDownloading.Value = false;
		}

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			if (navigationContext.Parameters["Token"] is string token )
			{

				_GitHubClient = _GithubClientFactoryService.CreateGithubClient(token);
			}
			CreateListCommand.Execute();
		}

		public class TargetDownload
		{
			public TargetDownload(bool isSelect, Octokit.Repository repository)
			{
				IsSelected = isSelect;
				Repository = repository;
			}

			public bool IsSelected { get; set; }
			public Octokit.Repository Repository { get; set; }
		}
	}
}
