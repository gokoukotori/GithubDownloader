using System.Windows.Controls;
using System.Windows.Forms;

namespace GithubInfo.Modules.StarredGithubDownloader.Views
{
	/// <summary>
	/// Interaction logic for GithubStarredInfo
	/// </summary>
	public partial class GithubStarredInfo : System.Windows.Controls.UserControl
	{
		public GithubStarredInfo()
		{
			InitializeComponent();
		}
		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			using (FolderBrowserDialog dialog = new FolderBrowserDialog())
			{
				dialog.ShowNewFolderButton = true;
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK)
				{
					this.FolderPath.Text = dialog.SelectedPath;
				}
			}
		}
	}
}
