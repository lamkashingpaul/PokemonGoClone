using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PokemonGoClone.ViewModels
{
    public class TrainerCreationViewModel : ViewModelBase
    {
        private int _choice;
        public ObservableCollection<Pokemon> StartPokemon { get; private set; }
        public ObservableCollection<BitmapImage> StartPokemonImage { get; private set; }

        // All ICommands for the viewmodel
        private ICommand _updateChoiceCommand;

        // All properties for ICommands
        public ICommand UpdateChoiceCommand
        {
            get { return _updateChoiceCommand ?? (_updateChoiceCommand = new RelayCommand(x => { UpdateChoice(x); })); }
        }

        // Default constructor
        public TrainerCreationViewModel()
        {
            StartPokemon = new ObservableCollection<Pokemon>
            {
                new Pokemon(001, "Bulbasaur"),
                new Pokemon(004, "Charmander"),
                new Pokemon(007, "Squirtle"),
            };
        }

        // All properties of TrainerCreationViewModel class
        public int Choice
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
                Console.WriteLine(Choice + " Selected.");
            }
        }
    }
}
