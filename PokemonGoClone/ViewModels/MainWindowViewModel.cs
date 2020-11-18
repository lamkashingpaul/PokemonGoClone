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

        // Default constructor
        public MainWindowViewModel()
        {
            // Create instance for all views and viewmodels
            StartView = new StartView();
            TrainerCreationView = new TrainerCreationView();
            MapView = new MapView();

            StartViewModel = new StartViewModel() { MainWindowViewModel = this};
            TrainerCreationViewModel = new TrainerCreationViewModel() { MainWindowViewModel = this};
            MapViewModel = new MapViewModel() { MainWindowViewModel = this};

            // Set up the startup view and viewmodels
            CurrentViewModel = StartViewModel;
            CurrentView = StartView;
        }

        // All properties of views and viewmodels
        public object StartView
        {
            get { return _startView; }
            set
            {
                _startView = value;
                OnPropertyChanged(nameof(StartView));
            }
        }
        public object StartViewModel
        {
            get { return _startViewModel; }
            set
            {
                _startViewModel = value;
                OnPropertyChanged(nameof(StartViewModel));
            }
        }
        public object TrainerCreationView
        {
            get { return _trainerCreationView; }
            set
            {
                _trainerCreationView = value;
                OnPropertyChanged(nameof(TrainerCreationView));
            }
        }
        public object TrainerCreationViewModel
        {
            get { return _trainerCreationViewModel; }
            set
            {
                _trainerCreationViewModel = value;
                OnPropertyChanged(nameof(TrainerCreationViewModel));
            }
        }

        public object MapView
        {
            get { return _mapView; }
            set
            {
                _mapView = value;
                OnPropertyChanged(nameof(MapView));
            }
        }
        public object MapViewModel
        {
            get { return _mapViewModel; }
            set
            {
                _mapViewModel = value;
                OnPropertyChanged(nameof(MapViewModel));
            }
        }

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
        public void GotoStartViewModel()
        {
            CurrentViewModel = StartViewModel;
            CurrentView = StartView;
        }
        public void GotoTrainerCreationViewModel()
        {
            CurrentViewModel = TrainerCreationViewModel;
            CurrentView = TrainerCreationView;
        }
        public void GoToMapViewModel()
        {
            CurrentViewModel = MapViewModel;
            CurrentView = MapView;
        }
        private void CloseWindow(object Windows)
        {
            (Windows as System.Windows.Window)?.Close();
        }
    }
}
