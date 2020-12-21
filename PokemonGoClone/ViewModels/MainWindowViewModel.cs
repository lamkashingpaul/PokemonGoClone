using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonGoClone.Models;
using PokemonGoClone.Models.Abilities;
using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using PokemonGoClone.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // All preloaded game data
        public List<PokemonModel> Pokemons { get; private set; }   // Pokemon data
        public List<AbilityModel> Abilities { get; private set; }   // Abilities data
        public List<ItemModel> Items { get; private set; }   // Items data
        public ObservableCollection<string> Saves { get; set; }   // List of *.pkmgc saves

        public string Name;   // Name of Player
        public int Choice;    // Choice of starting pokemon

        public TrainerModel Player;
        // All trainers inside the game, Trainers[0] is assigned to variable Player
        // Trainers is linked with Trainers inside MapViewModel;
        public List<TrainerModel> Trainers;

        // Map of the game, it is linked with Map inside MapViewModel
        public List<TileModel> Map;

        // All available views
        private object _dialogView;
        private object _startView;
        private object _trainerCreationView;
        private object _mapView;
        private object _shopView;
        private object _bagView;
        private object _pokemonStatusView;
        private object _itemView;
        private object _itemStatusView;
        private object _battleView;
        private object _saveView;
        private object _loadView;
        private object _currentView;

        // All availabe viewmodels
        private object _dialogViewModel;
        private object _startViewModel;
        private object _trainerCreationViewModel;
        private object _mapViewModel;
        private object _shopViewModel;
        private object _bagViewModel;
        private object _pokemonStatusViewModel;
        private object _itemViewModel;
        private object _itemStatusViewModel;
        private object _battleViewModel;
        private object _saveViewModel;
        private object _loadViewModel;
        private object _currentViewModel;

        // All ICommands to navigate between views and viewmodels
        private ICommand _goToStartViewModelCommand;
        private ICommand _goToTrainerCreationViewModelCommand;
        private ICommand _goToMapViewModelCommand;
        private ICommand _goToBagViewModelCommand;
        private ICommand _goToItemViewModelCommand;
        private ICommand _goToBattleViewModelCommand;
        private ICommand _goToSaveViewModelCommand;
        private ICommand _goToLoadViewModelCommand;

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
        public ICommand GoToItemViewModelCommand
        {
            get { return _goToItemViewModelCommand ?? (_goToItemViewModelCommand = new RelayCommand(x => { GoToItemViewModel(x); })); }
        }
        public ICommand GoToBattleViewModelCommand
        {
            get { return _goToBattleViewModelCommand ?? (_goToBattleViewModelCommand = new RelayCommand(x => { GoToBattleViewModel(x); })); }
        }

        // Default constructor
        public MainWindowViewModel()
        {
            // Load predefined game data
            Abilities = new List<AbilityModel>();
            Items = new List<ItemModel>();
            Pokemons = new List<PokemonModel>();

            LoadAbilities(Abilities);
            LoadItems(Items);
            LoadPokemons(Pokemons);

            Saves = new ObservableCollection<string>();

            // Create instance for views and viewmodels
            StartView = new StartView();
            TrainerCreationView = new TrainerCreationView();
            MapView = new MapView();
            BattleView = new BattleView();
            BagView = new BagView();
            PokemonStatusView = new PokemonStatusView();
            ItemView = new ItemView();
            ItemStatusView = new ItemStatusView();
            ShopView = new ShopView();
            LoadView = new LoadView();
            SaveView = new SaveView();

            DialogViewModel = new DialogViewModel(this);
            StartViewModel = new StartViewModel(this);
            TrainerCreationViewModel = new TrainerCreationViewModel(this);
            MapViewModel = new MapViewModel(this);
            BattleViewModel = new BattleViewModel(this);
            BagViewModel = new BagViewModel(this);
            PokemonStatusViewModel = new PokemonStatusViewModel(this);
            ItemViewModel = new ItemViewModel(this);
            ItemStatusViewModel = new ItemStatusViewModel(this);
            ShopViewModel = new ShopViewModel(this);
            LoadViewModel = new LoadViewModel(this);
            SaveViewModel = new SaveViewModel(this);

            // Set up the startup view and viewmodels
            CurrentViewModel = StartViewModel;
            CurrentView = StartView;
        }

        // Methods for loading game data
        private void LoadAbilities(List<AbilityModel> abilities)
        {
            string json;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Abilities.abilities.json");
            if (stream != null)
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    json = reader.ReadToEnd();
                }

                var jArray = JArray.Parse(json);

                foreach (var obj in jArray)
                {
                    AbilityModel ability = new AbilityModel(obj["Name"].Value<string>(),
                                                            obj["Id"].Value<int>(),
                                                            obj["Description"].Value<string>(),
                                                            obj["Damage"].Value<int>(),
                                                            obj["DamagePerLevel"].Value<int>(),
                                                            obj["MaxCharge"].Value<int>(),
                                                            obj["MaxChargePerLevel"].Value<int>(),
                                                            obj["Accuracy"].Value<double>());
                    abilities.Add(ability);
                }

            }
        }
        private void LoadItems(List<ItemModel> items)
        {
            int i = 1;
            while (true)
            {
                string json;
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Items.Pokeball." + $"{i:D3}.json");
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        json = reader.ReadToEnd();
                    }

                    var values = (JObject)JsonConvert.DeserializeObject(json);

                    PokeballModel item = new PokeballModel(values["Name"].Value<string>(),
                                                           values["Id"].Value<int>(),
                                                           values["Charge"].Value<int>(),
                                                           values["CatchProbability"].Value<double>()
                                                           );

                    items.Add(item);
                    i += 1;
                }
                else
                {
                    break;
                }
            }
            i = 1;
            while (true)
            {
                string json;
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Items.Potion." + $"{i:D3}.json");
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        json = reader.ReadToEnd();
                    }

                    var values = (JObject)JsonConvert.DeserializeObject(json);

                    PotionModel item = new PotionModel(values["Name"].Value<string>(),
                                                            values["Id"].Value<int>(),
                                                            values["Charge"].Value<int>(),
                                                            values["HealHP"].Value<int>()
                                                            );
                    items.Add(item);
                    i += 1;
                }
                else
                {
                    break;
                }
            }
        }
        private void LoadPokemons(List<PokemonModel> pokemons)
        {
            string json;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Pokemons.pokemons.json");
            if (stream != null)
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    json = reader.ReadToEnd();
                }

                var jArray = JArray.Parse(json);

                foreach (var obj in jArray)
                {
                    var rng = new Random();
                    int randomAbilityIndex = rng.Next(Abilities.Count);
                    PokemonModel pokemon = new PokemonModel(obj["Name"].Value<string>(),
                                                            obj["Id"].Value<int>(),
                                                            obj["Description"].Value<string>(),
                                                            obj["Level"].Value<int>(),
                                                            obj["MaxLevel"].Value<int>(),
                                                            obj["MaxHealth"].Value<int>(),
                                                            obj["MaxHealthPerLevel"].Value<int>(),
                                                            obj["Accuracy"].Value<double>(),
                                                            obj["EvolveId"].ToObject<int[]>(),
                                                            obj["EvolveCost"].Value<int>(),
                                                            obj["PowerUpCostBase"].Value<int>(),
                                                            obj["PowerUpCostPerLevel"].Value<int>(),
                                                            Abilities[randomAbilityIndex]);
                    pokemons.Add(pokemon);
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
        public object ShopView
        {
            get { return _shopView; }
            set
            {
                _shopView = value;
                OnPropertyChanged(nameof(ShopView));
            }
        }
        public object ShopViewModel
        {
            get { return _shopViewModel; }
            set
            {
                _shopViewModel = value;
                OnPropertyChanged(nameof(ShopViewModel));
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
        public object PokemonStatusView
        {
            get { return _pokemonStatusView; }
            set
            {
                _pokemonStatusView = value;
                OnPropertyChanged(nameof(PokemonStatusView));
            }
        }
        public object PokemonStatusViewModel
        {
            get { return _pokemonStatusViewModel; }
            set
            {
                _pokemonStatusViewModel = value;
                OnPropertyChanged(nameof(PokemonStatusViewModel));
            }
        }
        public object ItemStatusView
        {
            get { return _itemStatusView; }
            set
            {
                _itemStatusView = value;
                OnPropertyChanged(nameof(ItemStatusView));
            }
        }
        public object ItemStatusViewModel
        {
            get { return _itemStatusViewModel; }
            set
            {
                _itemStatusViewModel = value;
                OnPropertyChanged(nameof(ItemStatusViewModel));
            }
        }
        public object ItemView
        {
            get { return _itemView; }
            set
            {
                _itemView = value;
                OnPropertyChanged(nameof(ItemView));
            }
        }
        public object ItemViewModel
        {
            get { return _itemViewModel; }
            set
            {
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
        public object DialogView
        {
            get { return _dialogView; }
            set
            {
                _dialogView = value;
                OnPropertyChanged(nameof(DialogView));
            }
        }
        public object DialogViewModel
        {
            get { return _dialogViewModel; }
            set
            {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
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
        public void GoToShopViewModel(object x)
        {
            CurrentViewModel = ShopViewModel;
            CurrentView = ShopView;
        }
        public void GoToItemViewModel(object x)
        {
            CurrentViewModel = ItemViewModel;
            CurrentView = ItemView;
        }
        public void GoToPokemonStatusViewModel(object x)
        {
            CurrentViewModel = PokemonStatusViewModel;
            CurrentView = PokemonStatusView;
        }
        public void GotoItemStatusViewModel(object x)
        {
            CurrentViewModel = ItemStatusViewModel;
            CurrentView = ItemStatusView;
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
    }
}
