using ControlzEx.Theming;
using GKsWpfLib.Config;
using GKsWpfLib.Mvvm;
using GKsWpfLib.Plugins;
using GKsWpfLib.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GKsWpfPrismLib.Module.Base.ViewModels
{
	public class BaseWindowOptionDetailViewModel : RegionViewModelBase
	{
		public BaseWindowOptionDetailViewModel(IRegionManager regionManager, IContainerExtension container, IVisualPlugins plugins) : base(regionManager)
		{

			AccentColors = AccentColorData.ColorList();

			AppThemes = ApplicationThemeData.ColorList();
			var nowTheme = AppThemes.First(x => x.Name == GKsWpfLibSettings.Instance.ApplicationTheme);
			var inde = AppThemes.IndexOf(nowTheme);
			SelectedTheme = new ReactiveProperty<int>(inde).AddTo(Disposable);
			SelectedTheme.Subscribe(ChangeTheme);
			//SelectedAccount = new ReactiveProperty<int>(0).AddTo(Disposable);
			//SelectedAccount.Subscribe(ChangeAccount);
			//FontSize = new ReactiveProperty<double>(14).AddTo(Disposable);
			//FontSize.Subscribe(ChangeFontSize);



			Swatches = new SwatchesProvider().Swatches;

			PaletteHelper paletteHelper = new PaletteHelper();
			ITheme theme = paletteHelper.GetTheme();

		}
		public IList<AccentColorData> AccentColors { get; set; }

		public IList<ApplicationThemeData> AppThemes { get; set; }

		public ReactiveProperty<int> SelectedTheme { get; set; }
		public ReactiveProperty<int> SelectedAccount { get; set; }
		public ReactiveProperty<double> FontSize { get; set; }

		private void ChangeTheme(int index)
		{
			if (index == -1) return;
			ThemeManager.Current.ChangeThemeBaseColor(Application.Current, AppThemes[index].Name);
			ModifyTheme(theme => theme.SetBaseTheme(index == 1 ? MaterialDesignThemes.Wpf.Theme.Dark : MaterialDesignThemes.Wpf.Theme.Light));
			GKsWpfLibSettings.Instance.ApplicationTheme = AppThemes[index].Name;
			GKsWpfLibSettings.Instance.Save();
		}
		private void ChangeAccount(int index)
		{
			if (index == -1) return;
			ThemeManager.Current.ChangeThemeColorScheme(Application.Current, AccentColors[index].Name);
		}
		private void ChangeFontSize(double size)
		{
		}
		private static void ModifyTheme(Action<ITheme> modificationAction)
		{
			var paletteHelper = new PaletteHelper();
			ITheme theme = paletteHelper.GetTheme();

			modificationAction?.Invoke(theme);

			paletteHelper.SetTheme(theme);
		}

		public IEnumerable<Swatch> Swatches { get; }

		//public ICommand ApplyPrimaryCommand { get; } = new AnotherCommandImplementation(o => ApplyPrimary((Swatch)o));

		//private static void ApplyPrimary(Swatch swatch) => ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));

		//public ICommand ApplyAccentCommand { get; } = new AnotherCommandImplementation(o => ApplyAccent((Swatch)o));

		//private static void ApplyAccent(Swatch swatch) => ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));

	}
}
