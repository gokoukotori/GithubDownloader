using ControlzEx.Theming;
using GithubInfo.Services;
using GithubInfo.Services.Interfaces;
using GithubInfo.Views;
using GKsWpfLib.Config;
using GKsWpfLib.Modules;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Services.Dialogs;
using System.IO;
using System.Linq;
using System.Windows;

namespace GithubInfo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IMessageService, MessageService>();
			containerRegistry.Register<IDialogWindow, GKsWpfLib.Windows.Dialogs.DialogWindow>(); //default dialog host

			var plugins = ModuleLoader.LoadVisualPlugins(AppSettings.Instance.ModulesNames.Select(x => Path.Combine(AppSettings.Instance.ModulesDirectory, x)).ToArray());
			containerRegistry.RegisterInstance(plugins);
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			var modules = ModuleLoader.LoadModules(AppSettings.Instance.ModulesNames.Select(x => Path.Combine(AppSettings.Instance.ModulesDirectory, x)).ToArray());
			modules.ForEach(x => moduleCatalog.AddModule(x));

		}

		protected override void Initialize()
		{
			base.Initialize();
			AppSettings.Instance.Save();
			ThemeManager.Current.ChangeThemeBaseColor(Current, GKsWpfLibSettings.Instance.ApplicationTheme);
			var paletteHelper = new PaletteHelper();
			ITheme theme = paletteHelper.GetTheme();
			theme.SetBaseTheme(GKsWpfLibSettings.Instance.ApplicationTheme == "Dark" ? MaterialDesignThemes.Wpf.Theme.Dark : MaterialDesignThemes.Wpf.Theme.Light);

			paletteHelper.SetTheme(theme);
		}
	}
}