using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;


namespace PokemonGoClone.ViewModels {
    class PokemonStatusViewModel : ViewModelBase {

        private MainWindowViewModel _mainWindowViewModel;
        private PokemonModel _pokemon;
        private string _pokemonName;

        private ICommand _changeNameCommand;
        public ICommand ChangeNameCommand {
            get { return _changeNameCommand ?? (_changeNameCommand = new RelayCommand(x => { ChangeName(x); })); }
        }

        public PokemonStatusViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
        }

        public void UpdateView(PokemonModel pokemon) {
            Pokemon = pokemon;
        }
        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewModel; }
            set {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }
        public PokemonModel Pokemon {
            get { return _pokemon; }
            set {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
            }
        }
        public string getName {
            get { return Pokemon.Name; }
            set {
                _pokemonName = value;
                OnPropertyChanged(nameof(PokemonName));
            }
        }

        public void ChangeName(object value) {
            string originalname = Pokemon.Name;
            var text = value as TextBox;
            string newname = text.Text;
            if (string.IsNullOrEmpty(newname)) {
                text.Text = originalname;
                Pokemon.Name = originalname;
            }      
        
        }

    }
}
