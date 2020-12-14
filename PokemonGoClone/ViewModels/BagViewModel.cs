using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System.Collections.Generic;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;

        private List<PokemonModel> _pokemons;

        public BagViewModel(TrainerModel trainer)
        {
            _pokemons = trainer.Pokemons;
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewMode; }
            set
            {
                _mainWindowViewMode = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public List<PokemonModel> Pokemons
        {
            get { return _pokemons; }
        }



        
    }
}
