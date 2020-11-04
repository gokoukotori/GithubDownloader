using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKsWpfLib.Mvvm
{
	public class DialogViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest, IDialogAware
	{
		protected IRegionManager RegionManager { get; }

		public virtual string Title => "";

		public DialogViewModelBase(IRegionManager regionManager)
		{
			RegionManager = regionManager;
		}

		public virtual event Action<IDialogResult> RequestClose;

		public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
		{
			continuationCallback(true);
		}

		public virtual bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public virtual void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public virtual void OnNavigatedTo(NavigationContext navigationContext)
		{

		}

		public virtual bool CanCloseDialog()
		{
			return true;
		}
		public virtual void OnDialogClosed()
		{

		}
		public virtual void OnDialogOpened(IDialogParameters parameters)
		{

		}
	}
}
