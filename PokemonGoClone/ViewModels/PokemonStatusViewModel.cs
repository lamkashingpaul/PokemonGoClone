using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;


namespace PokemonGoClone.ViewModels {
    class PokemonStatusViewModel : ViewModelBase {
        
        //field of PokemonStatusViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private PokemonModel _pokemon;
        private string _originalName;
        private string _defaultName;
        private int _index;
        //private bool _isEnabled;
        //private bool _evolveButtonIsEnabled;

        //ICommand of PokemonStatusViewModel
        private ICommand _changeNameCommand;
        private ICommand _becomeFirstPokemonCommand;
        private ICommand _dropPokemonCommand;
        public ICommand ChangeNameCommand {
            get { return _changeNameCommand ?? (_changeNameCommand = new RelayCommand(x => { ChangeName(x); })); }
        }
        public ICommand BecomeFirstPokemonCommand {
            get { return _becomeFirstPokemonCommand ?? (_becomeFirstPokemonCommand = new RelayCommand(x => { BecomeFirstPokemon(); })); }
        }
        public ICommand DropPokemonCommand {
            get { return _dropPokemonCommand ?? (_dropPokemonCommand = new RelayCommand(x => { DropPokemon(); })); }
        }
        /*
        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                OnPropertyChanged(nameof(EvolveButtonIsEnabled));
            }
        }
        public bool EvolveButtonIsEnabled {
            get { return Pokemon.Id == 4; }
            set {
                _evolveButtonIsEnabled = value;
                OnPropertyChanged(nameof(EvolveButtonIsEnabled));
            }
        }
        */

        //Constructor of PokemonStatusViewModel
        public PokemonStatusViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            //EvolveButtonIsEnabled = false;
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
        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewModel; }
            set {
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
        public PokemonModel Pokemon {
            get { return _pokemon; }
            set {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
            }
        }
        public int Index {
            get { return _index; }
            set {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        //Method of PokemonStatusViewModel
        public void ChangeName(object value) {
            var text = value as TextBox;
            string newName = text.Text;
            if (!string.IsNullOrEmpty(newName))
            {
                Pokemon.Name = newName;
                OriginalName = newName;
            }
            DefaultName = OriginalName;
        }

        public void BecomeFirstPokemon() {
            PokemonModel tmp = MainWindowViewModel.Player.Pokemons[Index];
            for (int i = 0; i < Index; i++) {
                MainWindowViewModel.Player.Pokemons[Index-i] = MainWindowViewModel.Player.Pokemons[Index-i-1];
            }
            MainWindowViewModel.Player.Pokemons[0] = tmp;
        }
        public void UpdateView(PokemonModel pokemon, int index) {
            Pokemon = pokemon;
            OriginalName = Pokemon.Name;
            DefaultName = Pokemon.Name;
            Index = index;
            //OnPropertyChanged(nameof(EvolveButtonIsEnabled));
        }
        public void DropPokemon() {
            MainWindowViewModel.Player.DropPokemon(Pokemon);
            MainWindowViewModel.GoToBagViewModel();
        }

    }
}
