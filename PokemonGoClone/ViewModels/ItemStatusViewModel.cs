using PokemonGoClone.Models.Items;
using PokemonGoClone.Utilities;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    class ItemStatusViewModel : ViewModelBase
    {

        //field of ItemStatusViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private int _index;
        private ItemModel _item;
        private ICommand _dropItemCommand;

        public ICommand DropItemCommand
        {
            get { return _dropItemCommand ?? (_dropItemCommand = new RelayCommand(x => { DropItem(); })); }
        }

        //constructor of ItemStatusViewModel
        public ItemStatusViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        //Properties of ItemStatusViewModel
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            set
            {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        public ItemModel Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        //Method of ItemStatusViewModel
        public void UpdateView(ItemModel item, int index)
        {
            Item = item;
            Index = index;
        }

        public void DropItem()
        {
            MainWindowViewModel.Player.DropItem(Item);
            MainWindowViewModel.GoToItemViewModel(null);
        }

    }
}
