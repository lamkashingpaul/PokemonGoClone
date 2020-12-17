using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class LoadViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        public LoadViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = new DialogViewModel(this);
        }
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
        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new RelayCommand(x => { Load(x); })); }
        }
        private void Load(object x)
        {
            var trainers = Serializator.Deserialize<List<TrainerModel>>("data.dat");
            if (trainers == null)
            {
                DialogViewModel.CloseDelegateMethod = MainWindowViewModel.GoToStartViewModel;
                DialogViewModel.Message = "Loading Failed";
            } else
            {
                ((MapViewModel)MainWindowViewModel.MapViewModel).GameLoad(trainers);
                DialogViewModel.CloseDelegateMethod = MainWindowViewModel.GoToMapViewModel;
                DialogViewModel.Message = "Loaded Successfully";
            }
        }
    }
}
