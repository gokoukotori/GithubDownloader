using GKsWpfLib.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GithubInfo.Modules.StarredGithubDownloader.ViewModels
{
	public class ConfirmPersonalAccessTokenDialogViewModel : DialogViewModelBase
	{
		public ConfirmPersonalAccessTokenDialogViewModel(IRegionManager regionManager) : base(regionManager)
		{
			Message = new ReactivePropertySlim<string>("");

			YesCommand = new ReactiveCommand().AddTo(Disposable);
			YesCommand.Subscribe(() => RequestClose?.Invoke(new DialogResult(ButtonResult.OK)));
		}
		public override event Action<IDialogResult> RequestClose;
		/// <summary>メッセージボックスのタイトルを取得します。</summary>
		public string Title => "警告";

		/// <summary>メッセージボックスへ表示する文字列を取得します。</summary>
		public ReactivePropertySlim<string> Message { get; }

		/// <summary>はいボタンのCommandを取得します。</summary>
		public ReactiveCommand YesCommand { get; }


		/// <summary>ダイアログがClose可能かを取得します。</summary>
		/// <returns></returns>
		public override bool CanCloseDialog() { return true; }

		/// <summary>ダイアログClose時のイベントハンドラ。</summary>
		public override void OnDialogClosed()
		{
			// Method intentionally left empty.
		}

		/// <summary>ダイアログOpen時のイベントハンドラ。</summary>
		/// <param name="parameters">IDialogServiceに設定されたパラメータを表すIDialogParameters。</param>
		public override void OnDialogOpened(IDialogParameters parameters)
		{
			Message.Value = parameters.GetValue<string>("message");
		}
	}
}