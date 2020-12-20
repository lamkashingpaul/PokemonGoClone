using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;

        private ICommand _newCommand;
        private ICommand _loadCommand;
        private ICommand _saveCommand;
        private ICommand _continueCommand;
        private ICommand _closeWindowCommand;
        public ICommand NewCommand
        {
            get { return _newCommand ?? (_newCommand = new RelayCommand(x => { New(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand ContinueCommand
        {
            get { return _continueCommand ?? (_continueCommand = new RelayCommand(x => { Continue(x); }, x => (!DialogViewModel.IsVisible && IsContinue))); }
        }
        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new RelayCommand(x => { Load(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(x => { Save(x); }, x => !DialogViewModel.IsVisible && IsContinue)); }
        }
        public ICommand CloseWindowCommand
        {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(x => { CloseWindow(x); }, x => !DialogViewModel.IsVisible)); }
        }


        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewMode; }
            set
            {
                _mainWindowViewMode = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            set
            {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }
        public StartViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            DialogViewModel.DefaultDelegates();
        }
        public bool IsContinue
        {
            get { return MainWindowViewModel.Player != null; }
        }
        private void New(object x)
        {
            MainWindowViewModel.GoToTrainerCreationViewModel(null);
        }
        private void Continue(object x)
        {
            MainWindowViewModel.GoToMapViewModel(null);
        }
        private void Load(object x)
        {
            MainWindowViewModel.GoToLoadViewModel(null);
        }
        private void Save(object x)
        {
            MainWindowViewModel.GoToSaveViewModel(null);
        }
        private void CloseWindow(object Windows)
        {
            (Windows as System.Windows.Window)?.Close();
        }
    }
}
