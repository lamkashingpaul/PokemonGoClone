using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        private List<PokemonModel> _pokemons;
        public BagViewModel(TrainerModel trainer)
        {
            _pokemons = trainer.Pokemons;
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

        public List<PokemonModel> Pokemons
        {
            get { return _pokemons; }
        }



        
    }
}
