using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class ShopViewModel : ViewModelBase
    {
        //field of ShopViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private List<ItemModel> _defaultItem;
        private TrainerModel _trainer;
        private ItemModel _choose;
        private ICommand _selectedItemCommand;
        private ICommand _buyCommand;

        public ICommand SelectedItemCommand {
            get { return _selectedItemCommand ?? (_selectedItemCommand = new RelayCommand(x => { SelectedItem(x); })); }
        }
        public ICommand BuyCommand {
            get { return _buyCommand ?? (_buyCommand = new RelayCommand(x => { Buy(); })); }
        }

        //constructor
        public ShopViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
        }

        //properties of ShopViewModel
        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewModel; }
            set {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public TrainerModel Trainer {
            get { return _trainer; }
            set {
                _trainer = value;
                OnPropertyChanged(nameof(Trainer));
            }
        }

        public List<ItemModel> DefaultItem {
            get { return _defaultItem; }
            set {
                _defaultItem = value;
                OnPropertyChanged(nameof(DefaultItem));
            }
        }
        public ItemModel Choose {
            get { return _choose; }
            set {
                _choose = value;
                OnPropertyChanged(nameof(Choose));
            }
        }

        //Method of ShopViewModel
        public void Update(TrainerModel trainer, List<ItemModel> defaultItem) {
            DefaultItem = defaultItem;
            Trainer = trainer;
            Choose = MainWindowViewModel.Items[0];
        }

        public void SelectedItem(object item) {
            Choose = item as ItemModel;
        }

        public void Buy() {
            Trainer.Money -= Choose.Charge;
            Trainer.AddItem(Choose);
        }



    }
}
