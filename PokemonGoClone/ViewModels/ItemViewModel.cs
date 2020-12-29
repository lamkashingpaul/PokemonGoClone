using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        //field of ItemViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private ObservableCollection<ItemModel> _items;
        private TrainerModel _player;
        private ICommand _selectedItemCommand;

        public ICommand SelectedItemCommand
        {
            get { return _selectedItemCommand ?? (_selectedItemCommand = new RelayCommand(x => { SelectedItem(x); }, x => !DialogViewModel.IsVisible)); }
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

        public ObservableCollection<ItemModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public TrainerModel Player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged(nameof(Player));
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
        public void UpdatePlayer(TrainerModel player)
        {
            Player = player;
            Items = player.Items;
        }


    }
}
