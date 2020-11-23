using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System.Collections.Generic;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;

        private List<PokemonModel> _pokemens;
        private List<ItemModel> _items;
        public BagViewModel(TrainerModel trainer)
        {
            _pokemens = trainer.Pokemons;
            _items = trainer.Items;
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

        public List<PokemonModel> Pokemon
        {
            get { return _pokemens; }
        }

        public List<ItemModel> Items
        {
            get { return _items; }
        }
    }
}
