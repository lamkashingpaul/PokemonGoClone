using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class LoadViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        public ObservableCollection<string> Saves { get; set; }

        public LoadViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Saves = MainWindowViewModel.Saves;
            Update();
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
        public void Update()
        {
            Saves.Clear();
            foreach (var save in Directory.EnumerateFiles(@".\", "*.pkmgc", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).ToList())
            {
                Saves.Add(save);
            }
        }
        private ICommand _loadCommand;
        private ICommand _refreshCommand;
        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new RelayCommand(x => { Load(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new RelayCommand(x => { Refresh(x); }, x => !DialogViewModel.IsVisible)); }
        }
        private void Load(object x)
        {
            if (!(x is string save))
            {
                DialogViewModel.PopUp("You must choose a save.");
                return;
            }

            var trainers = Serializator.Deserialize<ObservableCollection<TrainerModel>>(save);
            if (trainers == null)
            {
                DialogViewModel.PopUp("Loading failed. Please choose another save.");
            }
            else
            {
                ((MapViewModel)MainWindowViewModel.MapViewModel).GameLoad(trainers);
            }
        }
        private void Refresh(object x)
        {
            Update();
        }
    }
}
