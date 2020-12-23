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
        private ObservableCollection<TrainerModel> _trainers;
        private PokemonModel _pokemon;
        private TrainerModel _currentOccupier;
        private PokemonModel _currentPokemon;
        private TrainerModel _player;
        private ICommand _challangePlayerCommand;
        private ICommand _selectedPokemonCommand;

        public ICommand ChallangePlayerCommand {
            get { return _challangePlayerCommand ?? (_challangePlayerCommand = new RelayCommand(x => { ChallangePlayer(); }, x => EnableButton())); }
        }
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
        public ObservableCollection<TrainerModel> Trainers {
            get { return _trainers; }
            set {
                _trainers = value;
                OnPropertyChanged(nameof(Trainers));
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

        public void UpdateTrainers(ObservableCollection<TrainerModel> trainers) {
            Trainers = trainers;
            Random rnd = new Random();
            int index1 = rnd.Next(1,Trainers.Count - 1);
            CurrentOccupier = Trainers[index1];
            int index2 = rnd.Next(0, Trainers[index1].Pokemons.Count);
            CurrentPokemon = CurrentOccupier.Pokemons[index2];
        }

        public void SelectedPokemon(object sender) {
            var pokemon = sender as PokemonModel;
            Pokemon = pokemon;            
        }
        public void UpdateOccupier(TrainerModel player,PokemonModel pokemon) {
            CurrentOccupier = player;
            CurrentPokemon = pokemon;
        }

        public bool EnableButton() {
            if (CurrentOccupier == Player) {
                CurrentOccupier.ImageSource = $"/PokemonGoClone;component/Images/Players/{Player.Id:D3}S.png"; ;
                return false;
            }
            return true;        
        }

        public void ChallangePlayer() {
            ((BattleViewModel)MainWindowViewModel.BattleViewModel).NewBattle(Player, CurrentOccupier, Pokemon, CurrentPokemon, "Gym");
            MainWindowViewModel.GoToBattleViewModel(null);
        }

    }
}
