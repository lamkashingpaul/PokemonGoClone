﻿using PokemonGoClone.Models.Abilities;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Windows.Controls;
using System.Windows.Input;


namespace PokemonGoClone.ViewModels
{
    class PokemonStatusViewModel : ViewModelBase
    {

        //field of PokemonStatusViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private PokemonModel _pokemon;
        private TrainerModel _player;
        private string _originalName;
        private string _defaultName;
        private int _index;
        private int _powerUpCost;
        private int _evolveCost;

        private Random _rng;

        //private bool _isEnabled;
        //private bool _evolveButtonIsEnabled;

        //ICommand of PokemonStatusViewModel
        private ICommand _changeNameCommand;
        private ICommand _becomeFirstPokemonCommand;
        private ICommand _showAbilityDescriptionCommand;
        private ICommand _sellPokemonCommand;
        private ICommand _powerUpCommand;
        private ICommand _evolveCommand;

        public ICommand ChangeNameCommand
        {
            get { return _changeNameCommand ?? (_changeNameCommand = new RelayCommand(x => { ChangeName(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand BecomeFirstPokemonCommand
        {
            get { return _becomeFirstPokemonCommand ?? (_becomeFirstPokemonCommand = new RelayCommand(x => { BecomeFirstPokemon(); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand ShowAbilityDescriptionCommand
        {
            get { return _showAbilityDescriptionCommand ?? (_showAbilityDescriptionCommand = new RelayCommand(x => { ShowAbilityDescription(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand SellPokemonCommand
        {
            get { return _sellPokemonCommand ?? (_sellPokemonCommand = new RelayCommand(x => { SellPokemon(); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand PowerUpCommand
        {
            get { return _powerUpCommand ?? (_powerUpCommand = new RelayCommand(x => { PowerUp(); }, x => PowerUpButton())); }
        }
        public ICommand EvolveCommand
        {
            get { return _evolveCommand ?? (_evolveCommand = new RelayCommand(x => { Evolve(); }, x => EvolveButton())); }
        }


        //Constructor of PokemonStatusViewModel
        public PokemonStatusViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Rng = new Random();
        }

        //Properties of PokemonStatusViewModel
        public string OriginalName
        {
            get { return _originalName; }
            set
            {
                _originalName = value;
            }
        }
        public string DefaultName
        {
            get { return _defaultName; }
            set
            {
                _defaultName = value;
                OnPropertyChanged(nameof(DefaultName));
            }
        }
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
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
        public PokemonModel Pokemon
        {
            get { return _pokemon; }
            set
            {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
            }
        }
        public TrainerModel Player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        public int PowerUpCost
        {
            get { return _powerUpCost; }
            private set
            {
                _powerUpCost = value;
                OnPropertyChanged(nameof(PowerUpCost));
            }
        }

        public int EvolveCost
        {
            get { return _evolveCost; }
            private set
            {
                _evolveCost = value;
                OnPropertyChanged(nameof(EvolveCost));
            }
        }
        public Random Rng
        {
            get { return _rng; }
            private set
            {
                _rng = value;
                OnPropertyChanged(nameof(Rng));
            }
        }

        //Method of PokemonStatusViewModel
        public void UpdatePlayer(TrainerModel player)
        {
            Player = player;
        }
        public void UpdateView(PokemonModel pokemon, int index)
        {
            Pokemon = pokemon;
            OriginalName = Pokemon.Name;
            DefaultName = Pokemon.Name;
            Index = index;
        }
        private void ChangeName(object value)
        {
            string result;
            var text = value as TextBox;
            string newName = text.Text;
            if (!string.IsNullOrEmpty(newName))
            {
                Pokemon.Name = newName;
                OriginalName = newName;
                result = $"Your pokemon has new name \"{newName}\".";
            }
            else
            {
                result = $"You must provide a name for your pokemon.";
            }
            DialogViewModel.PopUp(result);
            DefaultName = OriginalName;
        }

        private void BecomeFirstPokemon()
        {
            PokemonModel tmp = Player.Pokemons[Index];
            for (int i = 0; i < Index; i++)
            {
                Player.Pokemons[Index - i] = Player.Pokemons[Index - i - 1];
            }
            Player.Pokemons[0] = tmp;
            DialogViewModel.PopUp($"{tmp.Name} is now your leading pokemon now. ");
        }
        private void ShowAbilityDescription(object x)
        {
            var ability = x as AbilityModel;
            DialogViewModel.PopUp(ability.Description);
        }
        private void SellPokemon()
        {
            if (Player.Pokemons.Count == 1)
            {
                DialogViewModel.PopUp("You are not allowed to sell your last Pokemon. ");
            }
            else
            {
                DialogViewModel.PopUp($"You sold {Pokemon.Name} for {Pokemon.PowerUpCost} Candy. ");
                Player.Candy += Pokemon.PowerUpCost;
                Player.DropPokemon(Pokemon);
                MainWindowViewModel.GoToBagViewModel(null);
            }
        }

        public void PowerUp()
        {
            Pokemon.Level++;
            int add = Rng.Next(1, Pokemon.MaxHealthPerLevel + 1);
            Pokemon.MaxHealth += add;
            Pokemon.Health = Pokemon.MaxHealth;
            Player.Candy -= PowerUpCost;
            DialogViewModel.PopUp($"You have successfully Power Up {Pokemon.Name}. \nIts MaxHealth is added by {add}. ");
        }
        public void Evolve()
        {
            int originalPokemonIndex = Player.Pokemons.IndexOf(Pokemon);
            int evolveId = Pokemon.EvolveId[Rng.Next(Pokemon.EvolveId.Length)];
            var newPokemon = (PokemonModel)MainWindowViewModel.Pokemons.Find(x => x.Id == evolveId).Clone();

            newPokemon.Abilities = Pokemon.Abilities;
            newPokemon.AddRandomNewAbility(MainWindowViewModel.Abilities);

            string originalPokemon = Pokemon.Name;
            DefaultName = Pokemon.Name;
            Player.Pokemons[originalPokemonIndex] = newPokemon;

            Player.Stardust -= EvolveCost;
            Pokemon = newPokemon;
            DialogViewModel.PopUp($"You have successfully evolved {originalPokemon} to {Pokemon.Name}. ");
        }
        public bool PowerUpButton()
        {
            if (DialogViewModel.IsVisible)
            {
                return false;
            }
            PowerUpCost = Pokemon.PowerUpCostBase + (Pokemon.PowerUpCostPerLevel * Pokemon.Level);
            if (Pokemon.Level == Pokemon.MaxLevel || Player.Candy < PowerUpCost)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool EvolveButton()
        {
            if (DialogViewModel.IsVisible)
            {
                return false;
            }
            EvolveCost = Pokemon.EvolveCost;
            if (Pokemon.EvolveId[0] == 0 || Pokemon.Level != Pokemon.MaxLevel || Player.Stardust < EvolveCost)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
