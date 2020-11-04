using GKsWpfLib.Constants;
using GKsWpfLib.Mvvm;
using GKsWpfLib.Plugins;
using GKsWpfPrismLib.Module.Base.Views;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace GKsWpfPrismLib.Module.Base.ViewModels
{
	public class BaseStartUpTileMenuViewModel : RegionViewModelBase
	{
		public BaseStartUpTileMenuViewModel(IRegionManager regionManager, IVisualPlugins plugins) : base(regionManager)
		{
			ShowPluginViewCommand = new DelegateCommand<IVisualPlugin>(ExecuteShowPluginView);
			PluginList = new ObservableCollection<IVisualPlugin>(plugins);
			PluginList.Remove(plugins.First(x => x.MainViewName == nameof(BaseStartUpTileMenu)));

		}

		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			//do something
		}
		private void ExecuteShowPluginView(IVisualPlugin parameter)
		{
			var parameters = new NavigationParameters
			{
				{ nameof(IVisualPlugin), parameter }
			};
			RegionManager.RequestNavigate(RegionNames.Main, nameof(BaseContentHamburgerMenu), parameters);
		}

		public DelegateCommand<IVisualPlugin> ShowPluginViewCommand { get; }

		public ObservableCollection<IVisualPlugin> PluginList { get; set; }
	}
}
