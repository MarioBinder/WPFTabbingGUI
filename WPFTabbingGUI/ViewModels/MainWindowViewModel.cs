using WPFTabbingGUI.Common;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using WPFTabbingGUI.Models;

namespace WPFTabbingGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
    {
        #region ViewModelProperty Pages
        private ObservableCollection<TabItem> _pages = new ObservableCollection<TabItem>();
        public ObservableCollection<TabItem> Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                _pages = value;
                RaisePropertyChanged(p => p.Pages);
            }
        }
        #endregion

        #region ViewModelProperty TabReplacement
        private Dock _tabReplacement = new Dock();
        public Dock TabReplacement
        {
            get
            {
                return _tabReplacement;
            }
            set
            {
                _tabReplacement = value;
                RaisePropertyChanged(t => t.TabReplacement);
            }
        }
        #endregion

        #region ViewModelProperty Message
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged(m => m.Message);
            }
        }
        #endregion

        #region ViewModelProperty ApplicationHeight
        private double _applicationHeight;
        public double ApplicationHeight
        {
            get
            {
                return _applicationHeight;
            }
            set
            {
                _applicationHeight = value;
                RaisePropertyChanged(m => m.ApplicationHeight);
            }
        }
        #endregion

        #region ViewModelProperty ApplicationWidth
        private double _applicationWidth;
        public double ApplicationWidth
        {
            get
            {
                return _applicationWidth;
            }
            set
            {
                _applicationWidth = value;
                RaisePropertyChanged(m => m.ApplicationWidth);
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            var mainViewModel = new MainWindowModel();

            Pages = new ObservableCollection<TabItem>(mainViewModel.GetTabItems());
            TabReplacement = mainViewModel.SetConfiguration();

            ApplicationHeight = mainViewModel.ApplicationHeight;
            ApplicationWidth = mainViewModel.ApplicationWidth;

            Message = mainViewModel.Message;
        }
    }
}
