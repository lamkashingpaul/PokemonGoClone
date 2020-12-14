using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System.Collections.Generic;

namespace PokemonGoClone.ViewModels {
    public class ItemViewModel : ViewModelBase {
        private MainWindowViewModel _mainWindowViewMode;

        private List<ItemModel> _items;

        public ItemViewModel(TrainerModel trainer) {
            _items = trainer.Items;
        }

        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewMode; }
            set {
                _mainWindowViewMode = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public List<ItemModel> Items {
            get { return _items; }
        }



    }
}
