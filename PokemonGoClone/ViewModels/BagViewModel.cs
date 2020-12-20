﻿using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using PokemonGoClone.Views;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private List<PokemonModel> _pokemons;
        private ICommand _selectedPokemonCommand;

        public ICommand SelectedPokemonCommand {
            get { return _selectedPokemonCommand ?? (_selectedPokemonCommand = new RelayCommand(x => { SelectedPokemon(x); })); }
        }
        public BagViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        public void UpdatePlayer(TrainerModel trainer) {
            Pokemons = trainer.Pokemons;
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
        public void SelectedPokemon(object sender) {
            var pokemon = sender as PokemonModel;
            int index = MainWindowViewModel.Player.Pokemons.IndexOf(pokemon);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).UpdateView(pokemon, index);
            MainWindowViewModel.GotoPokemonStatusViewModel(null);
        }

        public List<PokemonModel> Pokemons
        {
            get { return _pokemons; }
            set {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }        
    }
}
