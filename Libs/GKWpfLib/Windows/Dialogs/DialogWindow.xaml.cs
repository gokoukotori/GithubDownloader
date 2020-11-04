using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace GKsWpfLib.Windows.Dialogs
{
	/// <summary>
	/// DialogWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class DialogWindow : MetroWindow, IDialogWindow
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DialogWindow"/> class.
		/// </summary>
		public DialogWindow()
		{
			InitializeComponent();
		}
		/// <summary>
		/// The <see cref="IDialogResult"/> of the dialog.
		/// </summary>
		public IDialogResult Result { get; set; }

	}
}
