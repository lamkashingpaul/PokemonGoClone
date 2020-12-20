using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using PokemonGoClone.Views;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        //field of BagViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private List<PokemonModel> _pokemons;
        private ICommand _selectedPokemonCommand;

        public ICommand SelectedPokemonCommand {
            get { return _selectedPokemonCommand ?? (_selectedPokemonCommand = new RelayCommand(x => { SelectedPokemon(x); })); }
        }

        //constructor of BagViewModel
        public BagViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
        }

        //properties of BagViewModel
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public List<PokemonModel> Pokemons {
            get { return _pokemons; }
            set {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }


        //method of BagViewModel
        public void SelectedPokemon(object sender) {
            var pokemon = sender as PokemonModel;
            int index = MainWindowViewModel.Player.Pokemons.IndexOf(pokemon);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).UpdateView(pokemon, index);
            MainWindowViewModel.GotoPokemonStatusViewModel();
        }

        public void UpdatePlayer(TrainerModel trainer) {
            Pokemons = trainer.Pokemons;
        }


    }
}
