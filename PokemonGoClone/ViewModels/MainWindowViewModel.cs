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
using PokemonGoClone.Models;

namespace PokemonGoClone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // All models
        public string Name;
        public int Choice;
        public List<BeingModel> Beings { get; private set; }
        public List<TileModel> Map { get; private set; }

        // All available views
        private object _startView;
        private object _trainerCreationView;
        private object _mapView;
        private object _bagView;
        private object _battleView;
        private object _currentView;

        // All availabe viewmodels
        private object _startViewModel;
        private object _trainerCreationViewModel;
        private object _mapViewModel;
        private object _bagViewModel;
        private object _battleViewModel;
        private object _currentViewModel;

        // All ICommands to navigate between views and viewmodels
        private ICommand _goTtoStartViewModelCommand;
        private ICommand _goToTrainerCreationViewModelCommand;
        private ICommand _goToMapViewModelCommand;
        private ICommand _goToBagViewModelCommand;
        private ICommand _goToBattleViewModelCommand;

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
        public ICommand GoToBagViewModelCommand
        {
            get { return _goToBagViewModelCommand ?? (_goToBagViewModelCommand = new RelayCommand(x => { GoToBagViewModel(); })); }
        }
        public ICommand GoToBattleViewModelCommand
        {
            get { return _goToBattleViewModelCommand ?? (_goToBattleViewModelCommand = new RelayCommand(x => { GoToBattleViewModel(); })); }
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

            StartViewModel = new StartViewModel() { MainWindowViewModel = this};
            TrainerCreationViewModel = new TrainerCreationViewModel() { MainWindowViewModel = this };

            // Set up the startup view and viewmodels
            CurrentViewModel = StartViewModel;
            CurrentView = StartView;
        }

        // Methods for game control
        public void StartNewGame (string name, int choice)
        {
            Name = name;
            Choice = choice;

            MapView = new MapView();
            MapViewModel = new MapViewModel(name, choice);

            CurrentView = MapView;
            CurrentViewModel = MapViewModel;
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

        public object BagView
        {
            get { return _bagView; }
            set
            {
                _bagView = value;
                OnPropertyChanged(nameof(BagView));
            }
        }
        public object BagViewModel
        {
            get { return _bagViewModel; }
            set
            {
                _bagViewModel = value;
                OnPropertyChanged(nameof(BagViewModel));
            }
        }
        public object BattleView
        {
            get { return _battleView; }
            set
            {
                _battleView = value;
                OnPropertyChanged(nameof(BattleView));
            }
        }
        public object BattleViewModel
        {
            get { return _battleViewModel; }
            set
            {
                _battleViewModel = value;
                OnPropertyChanged(nameof(BattleViewModel));
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
        public void GoToBagViewModel()
        {
            CurrentViewModel = BagViewModel;
            CurrentView = BagView;
        }
        public void GoToBattleViewModel()
        {
            CurrentViewModel = BattleViewModel;
            CurrentView = BattleView;
        }
        private void CloseWindow(object Windows)
        {
            (Windows as System.Windows.Window)?.Close();
        }
    }
}
