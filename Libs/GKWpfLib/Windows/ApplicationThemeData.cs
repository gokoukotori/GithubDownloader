using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKsWpfLib.Windows
{
	public class ApplicationThemeData : AccentColorData
	{
		public Brush BorderColorBrush { get; set; }

		public ApplicationThemeData(string name, Brush colorBrush, Brush borderColorBrush) : base(name, colorBrush)
		{
			BorderColorBrush = borderColorBrush ?? throw new ArgumentNullException(nameof(borderColorBrush));
		}

		public ApplicationThemeData() : base()
		{
		}

		new public static IList<ApplicationThemeData> ColorList()
		{
			return ThemeManager.Current.Themes
										 .GroupBy(x => x.BaseColorScheme)
										 .Select(x => x.First())
										.Select(a => new ApplicationThemeData(a.BaseColorScheme, a.Resources["MahApps.Brushes.ThemeForeground"] as Brush, a.Resources["MahApps.Brushes.ThemeBackground"] as Brush))
										.ToList();
		}

	}
}
