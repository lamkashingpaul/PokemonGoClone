using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels {
    public class ItemViewModel : ViewModelBase {
        private MainWindowViewModel _mainWindowViewModel;

        private List<ItemModel> _items;
        private ICommand _selectedItemCommand;

        public ICommand SelectedItemCommand {
            get { return _selectedItemCommand ?? (_selectedItemCommand = new RelayCommand(x => { SelectedItem(x); })); }
        }
        public ItemViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
        }

        public void UpdatePlayer(TrainerModel trainer) {
            Items = trainer.Items;
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
            set {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public void SelectedItem(object sender) {
            var item = sender as ItemModel;
            int index = MainWindowViewModel.Player.Items.IndexOf(item);
            ((ItemStatusViewModel)MainWindowViewModel.ItemStatusViewModel).UpdateView(item, index);
            MainWindowViewModel.GotoItemStatusViewModel();
        }



    }
}
