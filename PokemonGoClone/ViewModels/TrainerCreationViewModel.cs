using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PokemonGoClone.ViewModels
{
    public class TrainerCreationViewModel : ViewModelBase
    {
        // Delegates for DialogViewModel Action
        public void DefaultName(object x)
        {
            Console.WriteLine("This is called");
            DialogViewModel.IsVisible = false;
        }

        private MainWindowViewModel _mainWindowsViewModel;
        private DialogViewModel _dialogViewModel;

        private int? _choice;

        public List<PokemonModel> StartPokemon { get; private set; }
       
        // All ICommands for the viewmodel
        private ICommand _updateChoiceCommand;
        private ICommand _trainerCreationCommand;

        // All properties for ICommands
        public ICommand UpdateChoiceCommand
        {
            get { return _updateChoiceCommand ?? (_updateChoiceCommand = new RelayCommand(x => { UpdateChoice(x); })); }
        }

        public ICommand TrainerCreationCommand
        {
            get { return _trainerCreationCommand ?? (_trainerCreationCommand = new RelayCommand(x => { TrainerCreation(x); })); }
        }


        // Default constructor
        public TrainerCreationViewModel()
        {
            StartPokemon = new List<PokemonModel>
            {
                new PokemonModel(001, "Bulbasaur", false),
                new PokemonModel(004, "Charmander", true),
                new PokemonModel(007, "Squirtle", false),
            };

            Choice = 004;

            DialogViewModel = new DialogViewModel
            {
                ActionDelegateMethod = null
            };
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

        // All properties of TrainerCreationViewModel class
        public int? Choice
        {
            get { return _choice; }
            set
            {
                _choice = value;
                OnPropertyChanged(nameof(Choice));
            }
        }

        //  All RelayCommands for ICommand
        private void UpdateChoice(object sender)
        {
            int? choice = sender as int?;
            if (choice != null)
            {
                Choice = (int)choice;
                Console.WriteLine(Choice);
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
                DialogViewModel.Message = "You must enter your name.";
                return;
            } else if (choice == null)
            {
                DialogViewModel.Message = "You must pick up your Pokemon.";
                return;
            }

            MainWindowViewModel.StartNewGame(name, (int)choice);
        }
    }
}
