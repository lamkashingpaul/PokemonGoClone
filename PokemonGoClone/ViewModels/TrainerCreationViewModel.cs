using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class TrainerCreationViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowsViewModel;
        private DialogViewModel _dialogViewModel;

        private int? _choice;

        public List<PokemonModel> StartPokemon { get; private set; }

        // All ICommands for the TrainerCreationViewModel
        private ICommand _updateChoiceCommand;
        private ICommand _trainerCreationCommand;

        // All ICommand properties for TrainerCreationViewModel
        public ICommand UpdateChoiceCommand
        {
            get { return _updateChoiceCommand ?? (_updateChoiceCommand = new RelayCommand(x => { UpdateChoice(x); })); }
        }
        public ICommand TrainerCreationCommand
        {
            get { return _trainerCreationCommand ?? (_trainerCreationCommand = new RelayCommand(x => { TrainerCreation(x); })); }
        }

        // Constructor of TrainerCreationViewModel
        public TrainerCreationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            DialogViewModel.DefaultDelegates();

            // Set up starting pokemon choices
            StartPokemon = new List<PokemonModel>
            {
                new PokemonModel(001, "Bulbasaur", false),
                new PokemonModel(004, "Charmander", false),
                new PokemonModel(007, "Squirtle", false),
            };

            Choice = null;   // Default choice
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowsViewModel; }
            set
            {
                _mainWindowsViewModel = value;
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

        // All properties of TrainerCreationViewModel
        public int? Choice
        {
            get { return _choice; }
            set
            {
                _choice = value;
                OnPropertyChanged(nameof(Choice));
            }
        }

        //  All RelayCommands for TrainerCreationViewModel
        private void UpdateChoice(object sender)
        {
            int? choice = sender as int?;
            if (choice != null)
            {
                Choice = (int)choice;
            }
        }

        private void TrainerCreation(object values)
        {
            var list = (object[])values;
            var textBox = list[0] as TextBox;
            var choice = (int?)list[1];

            string name = textBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                DialogViewModel.PopUp("You must enter your name.");
                return;
            }
            else if (choice == null)
            {
                DialogViewModel.PopUp("You must pick up your Pokemon.");
                return;
            }

            // After all checkings, create the game and map
            ((MapViewModel)MainWindowViewModel.MapViewModel).GameInitialization(name, (int)choice);
        }
    }
}
