using GithubInfo.Core.PubSubEvents;
using GithubInfo.Modules.StarredGithubDownloader.Views;
using GithubInfo.Services.Github.Interface;
using GKsWpfLib.Constants;
using GKsWpfLib.Mvvm;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GithubInfo.Modules.StarredGithubDownloader.ViewModels
{
	public class GithubStarredOptionViewModel : RegionViewModelBase
	{
		public GithubStarredOptionViewModel(IRegionManager regionManager,IEventAggregator eventAggregator, IGithubClientFactoryService githubClientFactoryService, IDialogService dialogService) : base(regionManager)
		{
			_DialogService = dialogService;
			_EventAggregator = eventAggregator;
			_GithubClientFactoryService = githubClientFactoryService;
			Token = new ReactiveProperty<string>(StarredGithubDownloaderSetteings.Instance.Token).AddTo(Disposable);
			IsNotExcuteing = new ReactiveProperty<bool>(true).AddTo(Disposable);
			ExcuteAuthCommand = new AsyncReactiveCommand().AddTo(Disposable);
			ExcuteAuthCommand.Subscribe(ExcuteAuth);
			_EventAggregator.GetEvent<ChangePersonalAccessTokenEvent>().Subscribe(TokenReceived);
		}
		private readonly IDialogService _DialogService;

		public readonly IEventAggregator _EventAggregator;
		private readonly IGithubClientFactoryService _GithubClientFactoryService;
		public ReactiveProperty<string> UserName { get; }
		public ReactiveProperty<string> Token { get; }
		public ReactiveProperty<bool> IsNotExcuteing { get; }
		public AsyncReactiveCommand ExcuteAuthCommand { get; }
		private async Task ExcuteAuth()
		{
			IsNotExcuteing.Value = false;
			if (await _GithubClientFactoryService.IsEnableTokenAsync(Token.Value))
			{
				StarredGithubDownloaderSetteings.Instance.Token = Token.Value;
				StarredGithubDownloaderSetteings.Instance.Save();
				_EventAggregator.GetEvent<ChangePersonalAccessTokenEvent>().Publish(Token.Value);
				ShowInformationMessage("Personal Access Tokenの認証に成功しました。");
			}
			else
			{
				ShowInformationMessage("正しい Personal Access Tokenを入力してください。");
			}
			IsNotExcuteing.Value = true;
		}
		private void ShowInformationMessage(string message)
		{
			_DialogService.ShowDialog(nameof(ConfirmPersonalAccessTokenDialog), new DialogParameters($"message={message}"), _ => { });
		}

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			if (_GithubClientFactoryService.IsEnableToken(Token.Value))
			{
				_EventAggregator.GetEvent<ChangePersonalAccessTokenEvent>().Publish(Token.Value);
			}
		}
		private void TokenReceived(string token)
		{
			Token.Value = token;
		}
	}
}
