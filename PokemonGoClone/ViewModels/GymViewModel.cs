using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels {
    class GymViewModel : ViewModelBase {
        //field of GymViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private ObservableCollection<PokemonModel> _pokemons;
        private PokemonModel _pokemon;
        private TrainerModel _currentOccupier;
        private PokemonModel _currentPokemon;
        private TrainerModel _player;
        private ICommand _selectedPokemonCommand;

        public ICommand SelectedPokemonCommand {
            get { return _selectedPokemonCommand ?? (_selectedPokemonCommand = new RelayCommand(x => { SelectedPokemon(x); }, x => !DialogViewModel.IsVisible)); }
        }

        //constructor of GymViewModel
        public GymViewModel(MainWindowViewModel mainWindowViewModel) {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        //properties of GymViewModel
        public MainWindowViewModel MainWindowViewModel {
            get { return _mainWindowViewModel; }
            set {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public DialogViewModel DialogViewModel {
            get { return _dialogViewModel; }
            set {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }

        public ObservableCollection<PokemonModel> Pokemons {
            get { return _pokemons; }
            set {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }
        public PokemonModel Pokemon {
            get { return _pokemon; }
            set {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
            }
        }

        public TrainerModel Player {
            get { return _player; }
            set {
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        public TrainerModel CurrentOccupier {
            get { return _currentOccupier; }
            set {
                _currentOccupier = value;
                OnPropertyChanged(nameof(CurrentOccupier));
            }
        }

        public PokemonModel CurrentPokemon {
            get { return _currentPokemon; }
            set {
                _currentPokemon = value;
                OnPropertyChanged(nameof(CurrentPokemon));
            }
        }


        //Method of GymViewModel
        public void UpdatePlayer(TrainerModel player) {
            Player = player;
            Pokemons = player.Pokemons;
        }

        public void SelectedPokemon(object sender) {
            var pokemon = sender as PokemonModel;
            Pokemon = pokemon;
        }
    }
}
