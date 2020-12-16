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
        private MainWindowViewModel _mainWindowViewModel;
        private List<PokemonModel> _pokemons;
        private ICommand _selectedPokemonCommand;

        public ICommand SelectedPokemonCommand {
            get { return _selectedPokemonCommand ?? (_selectedPokemonCommand = new RelayCommand(x => { SelectedPokemon(x); })); }
        }
        public BagViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
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

        public void SelectedPokemon(object sender) {
            var pokemon = sender as PokemonModel;
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).UpdateView(pokemon);
            MainWindowViewModel.GotoPokemonStatusViewModel();
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
