using GKsWpfLib.Constants;
using Prism.Ioc;
using Prism.Regions;
using System.Windows.Controls;

namespace GKsWpfPrismLib.Module.Base.Views
{
	/// <summary>
	/// Interaction logic for BaseContentHamburgerMenu
	/// </summary>
	public partial class BaseContentHamburgerMenu : UserControl
	{
		public BaseContentHamburgerMenu()
		{
			InitializeComponent();

			RegionManager.SetRegionName(this.PluginViewRegion, RegionNames.MainContent);

			var regionMan = ContainerLocator.Current.Resolve<IRegionManager>();
			RegionManager.SetRegionManager(this.PluginViewRegion, regionMan);
		}
	}
}
