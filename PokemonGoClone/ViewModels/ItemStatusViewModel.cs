using PokemonGoClone.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.ViewModels {
    class ItemStatusViewModel : ViewModelBase {
        private MainWindowViewModel _mainWindowViewModel;
        private int _index;
        private ItemModel _item;
        public void UpdateView(ItemModel item, int index) {
            Item = item;
            Index = index;
        }

        public ItemStatusViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
        }

        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewModel; }
            set {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }
        public int Index {
            get { return _index; }
            set {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        public ItemModel Item {
            get { return _item; }
            set {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }
    }
}
