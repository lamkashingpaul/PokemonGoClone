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
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Abilities;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PokemonGoClone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // All models
        public List<PokemonModel> Pokemons { get; private set; }   // Pokemon data
        public List<AbilityModel> Abilities { get; private set; }   // Abilities data
        public List<ItemModel> Items { get; private set; }   // Items data

        public string Name;   // Name of Player
        public int Choice;    // Choice of starting pokemon

        public TrainerModel Player;
        // All trainers inside the game, Trainers[0] is assigned to variable Player
        // Trainers is linked with Trainers inside MapViewModel;
        public List<TrainerModel> Trainers;

        // Map of the game, it is linked with Map inside MapViewModel
        public List<TileModel> Map;

        // All available views
        private object _startView;
        private object _trainerCreationView;
        private object _mapView;
        private object _bagView;
        private object _pokemonStatusView;
        private object _itemView;
        private object _battleView;
        private object _saveView;
        private object _loadView;
        private object _currentView;

        // All availabe viewmodels
        private object _startViewModel;
        private object _trainerCreationViewModel;
        private object _mapViewModel;
        private object _bagViewModel;
        private object _pokemonStatusViewModel;
        private object _itemViewModel;
        private object _battleViewModel;
        private object _saveViewModel;
        private object _loadViewModel;
        private object _currentViewModel;

        // All ICommands to navigate between views and viewmodels
        private ICommand _goToStartViewModelCommand;
        private ICommand _goToTrainerCreationViewModelCommand;
        private ICommand _goToMapViewModelCommand;
        private ICommand _goToBagViewModelCommand;
        private ICommand _goToBattleViewModelCommand;
        private ICommand _goToSaveViewModelCommand;
        private ICommand _goToLoadViewModelCommand;
        private ICommand _closeWindowCommand;

        // All properties of ICommands for views and viewmodels navigation
        public ICommand GoToStartViewModelCommand
        {
            get { return _goToStartViewModelCommand ?? (_goToStartViewModelCommand = new RelayCommand(x => { GoToStartViewModel(x); })); }
        }
        public ICommand GoToTrainerCreationViewModelCommand
        {
            get { return _goToTrainerCreationViewModelCommand ?? (_goToTrainerCreationViewModelCommand = new RelayCommand(x => { GoToTrainerCreationViewModel(x); })); }
        }
        public ICommand GoToLoadViewModelCommand
        {
            get { return _goToLoadViewModelCommand ?? (_goToLoadViewModelCommand = new RelayCommand(x => { GoToLoadViewModel(x); })); }
        }
        public ICommand GoToSaveViewModelCommand
        {
            get { return _goToSaveViewModelCommand ?? (_goToSaveViewModelCommand = new RelayCommand(x => { GoToSaveViewModel(x); })); }
        }
        public ICommand GoToMapViewModelCommand
        {
            get { return _goToMapViewModelCommand ?? (_goToMapViewModelCommand = new RelayCommand(x => { GoToMapViewModel(x); })); }
        }
        public ICommand GoToBagViewModelCommand
        {
            get { return _goToBagViewModelCommand ?? (_goToBagViewModelCommand = new RelayCommand(x => { GoToBagViewModel(x); })); }
        }
        public ICommand GoToBattleViewModelCommand
        {
            get { return _goToBattleViewModelCommand ?? (_goToBattleViewModelCommand = new RelayCommand(x => { GoToBattleViewModel(x); })); }
        }
        public ICommand CloseWindowCommand
        {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(x => { CloseWindow(x); })); }
        }



        // Default constructor
        public MainWindowViewModel()
        {
            // Create instance for views and viewmodels
            StartView           = new StartView();
            TrainerCreationView = new TrainerCreationView();
            MapView             = new MapView();
            BattleView          = new BattleView();
            BagView             = new BagView();
            PokemonStatusView   = new PokemonStatusView();
            LoadView            = new LoadView();
            SaveView            = new SaveView();

            StartViewModel           = new StartViewModel(this);
            TrainerCreationViewModel = new TrainerCreationViewModel(this);
            MapViewModel             = new MapViewModel(this);
            BattleViewModel          = new BattleViewModel(this);
            BagViewModel             = new BagViewModel(this);
            PokemonStatusViewModel   = new PokemonStatusViewModel(this);   
            LoadViewModel            = new LoadViewModel(this);
            SaveViewModel            = new SaveViewModel(this);

            // Set up game data
            Abilities = new List<AbilityModel>();
            Items = new List<ItemModel>();
            Pokemons = new List<PokemonModel>();

            LoadAbilities(Abilities);
            LoadItems(Items);
            LoadPokemons(Pokemons);

            // Set up the startup view and viewmodels
            CurrentViewModel = StartViewModel;
            CurrentView = StartView;
        }

        // Methods for loading game data
        private void LoadAbilities(List<AbilityModel> abilities)
        {
            int i = 1;
            while (true)
            {
                string json;
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Abilities." + $"{i:D3}.json");
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        json = reader.ReadToEnd();
                    }

                    var values = (JObject)JsonConvert.DeserializeObject(json);

                    AbilityModel ability = new AbilityModel(values["Name"].Value<string>(),
                                                            values["Id"].Value<int>(),
                                                            values["Description"].Value<string>(),
                                                            values["Damage"].Value<int>(),
                                                            values["DamagePerLevel"].Value<int>(),
                                                            values["Level"].Value<int>(),
                                                            values["MaxLevel"].Value<int>(),
                                                            values["MaxCharge"].Value<int>(),
                                                            values["MaxChargePerLevel"].Value<int>(),
                                                            values["Accuracy"].Value<double>());
                    abilities.Add(ability);
                    i += 1;
                }
                else
                {
                    break;
                }
            }
        }
        private void LoadItems(List<ItemModel> items)
        {
            //throw new NotImplementedException();
        }
        private void LoadPokemons(List<PokemonModel> pokemons)
        {
            int i = 1;
            while (true)
            {
                string json;
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Pokemons." + $"{i:D3}.json");
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        json = reader.ReadToEnd();
                    }

                    var values = (JObject)JsonConvert.DeserializeObject(json);

                    PokemonModel pokemon = new PokemonModel(values["Name"].Value<string>(),
                                                            values["Id"].Value<int>(),
                                                            values["Description"].Value<string>(),
                                                            values["Level"].Value<int>(),
                                                            values["MaxLevel"].Value<int>(),
                                                            values["MaxHealth"].Value<int>(),
                                                            values["MaxHealthPerLevel"].Value<int>(),
                                                            values["MaxExp"].Value<int>(),
                                                            values["MaxExpPerLevel"].Value<int>(),
                                                            values["Accuracy"].Value<double>(),
                                                            Abilities[0]);
                    pokemons.Add(pokemon);
                    i += 1;
                }
                else
                {
                    break;
                }
            }
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
        public object PokemonStatusViewModel {
            get { return _pokemonStatusViewModel; }
            set {
                _pokemonStatusViewModel = value;
                OnPropertyChanged(nameof(PokemonStatusViewModel));
            }
        }
        public object PokemonStatusView {
            get { return _pokemonStatusView; }
            set {
                _pokemonStatusView = value;
                OnPropertyChanged(nameof(PokemonStatusView));
            }
        }
        public object ItemView {
            get { return _itemView; }
            set {
                _itemView = value;
                OnPropertyChanged(nameof(ItemView));
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
        public object ItemViewModel {
            get { return _itemViewModel; }
            set {
                _itemViewModel = value;
                OnPropertyChanged(nameof(ItemViewModel));
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
        public object SaveView
        {
            get { return _saveView; }
            set
            {
                _saveView = value;
                OnPropertyChanged(nameof(SaveView));
            }
        }
        public object SaveViewModel
        {
            get { return _saveViewModel; }
            set
            {
                _saveViewModel = value;
                OnPropertyChanged(nameof(SaveViewModel));
            }
        }
        public object LoadView
        {
            get { return _loadView; }
            set
            {
                _loadView = value;
                OnPropertyChanged(nameof(LoadView));
            }
        }
        public object LoadViewModel
        {
            get { return _loadViewModel; }
            set
            {
                _loadViewModel = value;
                OnPropertyChanged(nameof(LoadViewModel));
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
        public void GoToStartViewModel(object x)
        {
            CurrentViewModel = StartViewModel;
            CurrentView = StartView;
        }
        public void GoToTrainerCreationViewModel(object x)
        {
            CurrentViewModel = TrainerCreationViewModel;
            CurrentView = TrainerCreationView;
        }
        public void GoToMapViewModel(object x)
        {
            CurrentViewModel = MapViewModel;
            CurrentView = MapView;
        }
        public void GoToBagViewModel(object x)
        {
            CurrentViewModel = BagViewModel;
            CurrentView = BagView;
        }
        public void GotoPokemonStatusViewModel(object x)
        {
            CurrentViewModel = PokemonStatusViewModel;
            CurrentView = PokemonStatusView;
        }
        public void GoToBattleViewModel(object x)
        {
            CurrentViewModel = BattleViewModel;
            CurrentView = BattleView;
        }
        public void GoToLoadViewModel(object x)
        {
            CurrentViewModel = LoadViewModel;
            CurrentView = LoadView;
        }
        public void GoToSaveViewModel(object x)
        {
            CurrentViewModel = SaveViewModel;
            CurrentView = SaveView;
        }
        private void CloseWindow(object Windows)
        {
            (Windows as System.Windows.Window)?.Close();
        }
    }
}
