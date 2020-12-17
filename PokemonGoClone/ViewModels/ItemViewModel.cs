using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System.Collections.Generic;

namespace PokemonGoClone.ViewModels {
    public class ItemViewModel : ViewModelBase {
        private MainWindowViewModel _mainWindowViewModel;

        private List<ItemModel> _items;

        public ItemViewModel(TrainerModel trainer) {
            _items = trainer.Items;
        }

        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewModel; }
            set {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public List<ItemModel> Items {
            get { return _items; }
        }



    }
}
