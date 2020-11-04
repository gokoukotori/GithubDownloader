using GKsWpfLib.Constants;
using GKsWpfLib.Mvvm;
using GKsWpfLib.Plugins;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace GKsWpfPrismLib.Module.Base.ViewModels
{
	public class BaseOptionsTabViewModel : RegionViewModelBase
	{
		public ReactivePropertySlim<string> PluginOptionsViewRegion { get; set; }
		public BaseOptionsTabViewModel(IRegionManager regionManager, IContainerExtension container, IVisualPlugins plugins) : base(regionManager)
		{
			PluginOptionsViewRegion = new ReactivePropertySlim<string>(RegionNames.PluginTab).AddTo(Disposable);
			TabItemList = new ObservableCollection<TabItem>();
			foreach (var item in plugins)
			{
				if (item.SettingViewName?.Length == 0) continue;
				TabItemList.Add(new TabItem() { Content = container.Resolve(item.SettingViewType), Header = item.Name });
			}
		}
		public ObservableCollection<TabItem> TabItemList { get; set; }
	}
}
