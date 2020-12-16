using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Input;


namespace PokemonGoClone.ViewModels {
    class PokemonStatusViewModel : ViewModelBase {

        private MainWindowViewModel _mainWindowViewModel;
        private PokemonModel _pokemon;
        public void UpdateView(PokemonModel pokemon) {
            Pokemon = pokemon;
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
    }
}
