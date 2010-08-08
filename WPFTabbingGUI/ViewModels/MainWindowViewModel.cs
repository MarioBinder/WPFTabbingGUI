/*
 * Copyright (C) 2010, Mario Priebe <mp@biggle.de>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the project nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
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
