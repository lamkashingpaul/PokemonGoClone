using PokemonGoClone.Views;
using PokemonGoClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PokemonGoClone.Utilities;
using System.Windows.Controls;

namespace PokemonGoClone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // All models

        // All available views
        private object _startView;
        private object _trainerCreationView;
        private object _mapView;
        private object _currentView;

        // All availabe viewmodels
        private object _startViewModel;
        private object _trainerCreationViewModel;
        private object _mapViewModel;
        private object _currentViewModel;

        // All ICommands to navigate between views and viewmodels
        private ICommand _goTtoStartViewModelCommand;
        private ICommand _goToTrainerCreationViewModelCommand;
        private ICommand _goToMapViewModelCommand;

        private ICommand _closeWindowCommand;

        // All properties of ICommands for views and viewmodels navigation
        public ICommand GoToStartViewModelCommand
        {
            get { return _goTtoStartViewModelCommand ?? (_goTtoStartViewModelCommand = new RelayCommand(x => { GotoStartViewModel(); })); }
        }
        public ICommand GoToTrainerCreationViewModelCommand
        {
            get { return _goToTrainerCreationViewModelCommand ?? (_goToTrainerCreationViewModelCommand = new RelayCommand(x => { GotoTrainerCreationViewModel(); })); }
        }
        public ICommand GoToMapViewModelCommand
        {
            get { return _goToMapViewModelCommand ?? (_goToMapViewModelCommand = new RelayCommand(x => { GoToMapViewModel(); })); }
        }
        public ICommand CloseWindowCommand
        {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(x => { CloseWindow(x); })); }
        }

        // More ICommands for initializing the game
        private ICommand _trainerCreationCommand;

        // All properties of ICommands for initializing the game
        public ICommand TrainerCreationCommand
        {
            get { return _trainerCreationCommand ?? (_trainerCreationCommand = new RelayCommand(x => { TrainerCreation(x); })); }
        }

        // Default constructor
        public MainWindowViewModel()
        {
            // Create instance for all views and viewmodels
            _startViewModel = new StartViewModel();
            _startView = new StartView();

            _trainerCreationViewModel = new TrainerCreationViewModel();
            _trainerCreationView = new TrainerCreationView();

            // Set up the startup view and viewmodels
            CurrentViewModel = _startViewModel;
            CurrentView = _startView;
        }

        // All properties of views and viewmodels
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        // All RelayCommands for views and viewmodels navigation
        private void GotoStartViewModel()
        {
            CurrentViewModel = _startViewModel;
            CurrentView = _startView;
        }
        private void GotoTrainerCreationViewModel()
        {
            CurrentViewModel = _trainerCreationViewModel;
            CurrentView = _trainerCreationView;
        }
        private void GoToMapViewModel()
        {
            CurrentViewModel = _mapViewModel;
            CurrentView = _mapView;
        }
        private void CloseWindow(object Windows)
        {
            (Windows as System.Windows.Window)?.Close();
        }

        // All RelayCommands for initializing the game
        private void TrainerCreation(object values)
        {
            var list = (object[])values;
            var textBox = list[0] as TextBox;
            var choice = (int)list[1];

            string name = textBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("You must enter your name.");
                return;
            }

            _mapView = new MapView();
            _mapViewModel = new MapViewModel(name, choice);
        }
    }
}
