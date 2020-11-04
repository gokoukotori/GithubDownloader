using GKsWpfLib.Plugins;
using GKsWpfPrismLib.Module.Base.ViewModels;
using GKsWpfPrismLib.Module.Base.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKsWpfPrismLib.Module.Base
{
	public class PluginInfo : IVisualPlugin
	{
		public string Name => "ベースモジュール";

		public string MainViewName => nameof(BaseStartUpTileMenu);

		public string SettingViewName => nameof(BaseWindowOptionDetail);
		public string PluginThemeColor => "#32CD32";
		public string Icon => "Home";

		public Type SettingViewType => typeof(BaseWindowOptionDetail);
	}
}
