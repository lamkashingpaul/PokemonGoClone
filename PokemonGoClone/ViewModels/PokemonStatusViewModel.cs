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

        private MainWindowViewModel _mainWindowViewModel;
        private PokemonModel _pokemon;
        private string _originalName;
        private string _defaultName;

        private ICommand _changeNameCommand;
        public ICommand ChangeNameCommand {
            get { return _changeNameCommand ?? (_changeNameCommand = new RelayCommand(x => { ChangeName(x); })); }
        }

        public PokemonStatusViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
        }

        public void UpdateView(PokemonModel pokemon) {
            Pokemon = pokemon;
            OriginalName = Pokemon.Name;
            DefaultName = Pokemon.Name;
        }
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
        public PokemonModel Pokemon {
            get { return _pokemon; }
            set {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
            }
        }

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

    }
}
