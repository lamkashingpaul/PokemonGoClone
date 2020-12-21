using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        //field of ItemViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private List<ItemModel> _items;
        private ICommand _selectedItemCommand;

        public ICommand SelectedItemCommand
        {
            get { return _selectedItemCommand ?? (_selectedItemCommand = new RelayCommand(x => { SelectedItem(x); })); }
        }

        //Constructor of ItemViewModel 
        public ItemViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        //Properties of ItemViewModel
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

        public List<ItemModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        //Method of ItemViewModel
        public void SelectedItem(object sender)
        {
            var item = sender as ItemModel;
            int index = MainWindowViewModel.Player.Items.IndexOf(item);
            ((ItemStatusViewModel)MainWindowViewModel.ItemStatusViewModel).UpdateView(item, index);
            MainWindowViewModel.GotoItemStatusViewModel(null);
        }
        public void UpdatePlayer(TrainerModel trainer)
        {
            Items = trainer.Items;
        }


    }
}
