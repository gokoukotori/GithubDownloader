
using GKsWpfLib.Constants;
using GKsWpfLib.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace GithubInfo.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly IRegionManager _regionManager;
		public ReactivePropertySlim<string> Title { get; }
		public ReactivePropertySlim<string> MainRegion { get; }
		public ReactiveCommand ShowMainMenuViewCommand { get; }

		public MainWindowViewModel(IRegionManager regionManager)
		{
			Title = new ReactivePropertySlim<string>("Prism Application").AddTo(Disposable);
			MainRegion = new ReactivePropertySlim<string>(RegionNames.Main).AddTo(Disposable);

			_regionManager = regionManager;
			ShowMainMenuViewCommand = new ReactiveCommand().WithSubscribe(ExecuteMainMenuView).AddTo(Disposable);
		}
		private void ExecuteMainMenuView()
		{
			_regionManager.RequestNavigate(MainRegion.Value, "BaseStartUpTileMenu");
		}
	}
}
