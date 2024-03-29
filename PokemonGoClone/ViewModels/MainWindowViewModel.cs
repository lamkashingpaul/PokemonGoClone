﻿using Newtonsoft.Json.Linq;
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
        public List<int> BeastsId { get; private set; }   // Beasts Pokemon Id

        public ObservableCollection<string> Saves { get; set; }   // List of *.pkmgc saves

        public string Name;   // Name of Player
        public int Choice;    // Choice of starting pokemon

        public TrainerModel Player;
        // All trainers inside the game, Trainers[0] is assigned to variable Player
        // Trainers is linked with Trainers inside MapViewModel;
        public ObservableCollection<TrainerModel> Trainers;

        // Map of the game, it is linked with Map inside MapViewModel
        public ObservableCollection<TileModel> Map;

        // All available views
        private object _dialogView;
        private object _startView;
        private object _trainerCreationView;
        private object _mapView;
        private object _shopView;
        private object _bagView;
        private object _gymView;
        private object _pokemonStatusView;
        private object _itemView;
        private object _itemStatusView;
        private object _battleView;
        private object _saveView;
        private object _loadView;
        private object _receptionView;
        private object _racecourseView;
        private object _currentView;

        // All availabe viewmodels
        private object _dialogViewModel;
        private object _startViewModel;
        private object _trainerCreationViewModel;
        private object _mapViewModel;
        private object _shopViewModel;
        private object _bagViewModel;
        private object _gymViewModel;
        private object _pokemonStatusViewModel;
        private object _itemViewModel;
        private object _itemStatusViewModel;
        private object _battleViewModel;
        private object _saveViewModel;
        private object _loadViewModel;
        private object _receptionViewModel;
        private object _racecourseViewModel;
        private object _currentViewModel;

        // All ICommands to navigate between views and viewmodels
        private ICommand _goToStartViewModelCommand;
        private ICommand _goToTrainerCreationViewModelCommand;
        private ICommand _goToMapViewModelCommand;
        private ICommand _goToBagViewModelCommand;
        private ICommand _goToGymViewModelCommand;
        private ICommand _goToItemViewModelCommand;
        private ICommand _goToBattleViewModelCommand;
        private ICommand _goToSaveViewModelCommand;
        private ICommand _goToLoadViewModelCommand;
        private ICommand _goToReceptionViewModelCommand;
        private ICommand _goToRacecourseViewModelCommand;

        private Random _rng;

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
        public ICommand GoToGymViewModelCommand
        {
            get { return _goToGymViewModelCommand ?? (_goToGymViewModelCommand = new RelayCommand(x => { GoToGymViewModel(x); })); }
        }
        public ICommand GoToItemViewModelCommand
        {
            get { return _goToItemViewModelCommand ?? (_goToItemViewModelCommand = new RelayCommand(x => { GoToItemViewModel(x); })); }
        }
        public ICommand GoToBattleViewModelCommand
        {
            get { return _goToBattleViewModelCommand ?? (_goToBattleViewModelCommand = new RelayCommand(x => { GoToBattleViewModel(x); })); }
        }
        public ICommand GoToReceptionViewModelCommand
        {
            get { return _goToReceptionViewModelCommand ?? (_goToReceptionViewModelCommand = new RelayCommand(x => { GoToReceptionViewModel(x); })); }
        }
        public ICommand GoToRacecourseViewModelCommand
        {
            get { return _goToRacecourseViewModelCommand ?? (_goToRacecourseViewModelCommand = new RelayCommand(x => { GoToRacecourseViewModel(x); })); }
        }

        // Default constructor
        public MainWindowViewModel()
        {
            Rng = new Random();

            // Load predefined game data
            Abilities = new List<AbilityModel>();
            Items = new List<ItemModel>();
            Pokemons = new List<PokemonModel>();
            BeastsId = new List<int>();

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
            GymView = new GymView();
            PokemonStatusView = new PokemonStatusView();
            ItemView = new ItemView();
            ItemStatusView = new ItemStatusView();
            ShopView = new ShopView();
            LoadView = new LoadView();
            SaveView = new SaveView();
            ReceptionView = new ReceptionView();
            RacecourseView = new RacecourseView();

            DialogViewModel = new DialogViewModel(this);
            StartViewModel = new StartViewModel(this);
            TrainerCreationViewModel = new TrainerCreationViewModel(this);
            MapViewModel = new MapViewModel(this);
            BattleViewModel = new BattleViewModel(this);
            BagViewModel = new BagViewModel(this);
            GymViewModel = new GymViewModel(this);
            PokemonStatusViewModel = new PokemonStatusViewModel(this);
            ItemViewModel = new ItemViewModel(this);
            ItemStatusViewModel = new ItemStatusViewModel(this);
            ShopViewModel = new ShopViewModel(this);
            LoadViewModel = new LoadViewModel(this);
            SaveViewModel = new SaveViewModel(this);
            ReceptionViewModel = new ReceptionViewModel(this);
            RacecourseViewModel = new RacecourseViewModel(this);

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
                                                            obj["Heal"].Value<int>(),
                                                            obj["HealPerLevel"].Value<int>(),
                                                            obj["MaxCharge"].Value<int>(),
                                                            obj["Accuracy"].Value<double>());
                    abilities.Add(ability);
                }
            }
        }
        private void LoadItems(List<ItemModel> items)
        {
            // Load Pokeballs
            string json;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Items.Pokeballs.pokeballs.json");
            if (stream != null)
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    json = reader.ReadToEnd();
                }

                var jArray = JArray.Parse(json);

                foreach (var obj in jArray)
                {
                    PokeballModel item = new PokeballModel(obj["Name"].Value<string>(),
                                                           obj["Id"].Value<int>(),
                                                           obj["Description"].Value<string>(),
                                                           obj["Cost"].Value<int>(),
                                                           obj["CatchProbability"].Value<double>());
                    items.Add(item);
                }
            }

            // Load Potions
            stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Items.Potions.potions.json");
            if (stream != null)
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    json = reader.ReadToEnd();
                }

                var jArray = JArray.Parse(json);

                foreach (var obj in jArray)
                {
                    PotionModel item = new PotionModel(obj["Name"].Value<string>(),
                                                       obj["Id"].Value<int>(),
                                                       obj["Description"].Value<string>(),
                                                       obj["Cost"].Value<int>(),
                                                       obj["HealHP"].Value<int>());
                    items.Add(item);
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

                List<AbilityModel> damageAbilities = Abilities.FindAll(x => x.Damage > 0);
                foreach (var obj in jArray)
                {
                    // Each Pokemon must have one damage ability
                    int randomAbilityIndex = Rng.Next(damageAbilities.Count);
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
                                                            damageAbilities[randomAbilityIndex]);
                    // Add additional on ability to pokemon
                    pokemon.AddRandomNewAbility(Abilities);
                    pokemons.Add(pokemon);
                }
            }

            BeastsId = new List<int>() { 144, 145, 146, 150, 151 };
        }

        // All properties of views and viewmodels
        public Random Rng
        {
            get { return _rng; }
            set
            {
                _rng = value;
                OnPropertyChanged(nameof(Rng));
            }
        }
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
        public object GymView
        {
            get { return _gymView; }
            set
            {
                _gymView = value;
                OnPropertyChanged(nameof(GymView));
            }
        }
        public object GymViewModel
        {
            get { return _gymViewModel; }
            set
            {
                _gymViewModel = value;
                OnPropertyChanged(nameof(GymViewModel));
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
        public object ReceptionView
        {
            get { return _receptionView; }
            set
            {
                _receptionView = value;
                OnPropertyChanged(nameof(ReceptionView));
            }
        }
        public object ReceptionViewModel
        {
            get { return _receptionViewModel; }
            set
            {
                _receptionViewModel = value;
                OnPropertyChanged(nameof(ReceptionViewModel));
            }
        }
        public object RacecourseView
        {
            get { return _racecourseView; }
            set
            {
                _racecourseView = value;
                OnPropertyChanged(nameof(RacecourseView));
            }
        }
        public object RacecourseViewModel
        {
            get { return _racecourseViewModel; }
            set
            {
                _racecourseViewModel = value;
                OnPropertyChanged(nameof(RacecourseViewModel));
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
            ((MapViewModel)MapViewModel).CheckMailBox();
        }
        public void GoToBagViewModel(object x)
        {
            CurrentViewModel = BagViewModel;
            CurrentView = BagView;
        }
        public void GoToGymViewModel(object x)
        {
            CurrentViewModel = GymViewModel;
            CurrentView = GymView;
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
        public void GoToReceptionViewModel(object x)
        {
            CurrentViewModel = ReceptionViewModel;
            CurrentView = ReceptionView;
        }
        public void GoToRacecourseViewModel(object x)
        {
            CurrentViewModel = RacecourseViewModel;
            CurrentView = RacecourseView;
        }
    }
}
