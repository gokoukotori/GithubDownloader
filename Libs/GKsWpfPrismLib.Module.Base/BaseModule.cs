using GKsWpfLib.Constants;
using GKsWpfPrismLib.Module.Base.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GKsWpfPrismLib.Module.Base
{
	public class BaseModule : IModule
	{
		private readonly IRegionManager _regionManager;

		public BaseModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}
		public void OnInitialized(IContainerProvider containerProvider)
		{
			_regionManager.RequestNavigate(RegionNames.Main, nameof(BaseStartUpTileMenu));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<BaseContentHamburgerMenu>();
			containerRegistry.RegisterForNavigation<BaseOptionsTab>();
			containerRegistry.RegisterForNavigation<BaseStartUpTileMenu>();
			containerRegistry.RegisterForNavigation<BaseWindowOptionDetail>();
		}
	}
}