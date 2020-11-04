using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKsWpfLib.Windows
{
	public class AccentColorData
	{
		public AccentColorData(string name, Brush colorBrush)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			ColorBrush = colorBrush ?? throw new ArgumentNullException(nameof(colorBrush));
		}

		public AccentColorData()
		{
		}

		public string Name { get; set; }


		public Brush ColorBrush { get; set; }

		public static IList<AccentColorData> ColorList()
		{
			return ThemeManager.Current.Themes.GroupBy(x => x.ColorScheme)
											.OrderBy(a => a.Key)
											.Select(a => new AccentColorData(a.Key, a.First().ShowcaseBrush))
											.ToList();
		}

	}
}
