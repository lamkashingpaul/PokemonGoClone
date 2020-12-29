using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    class GymViewModel : ViewModelBase
    {
        //field of GymViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private ObservableCollection<PokemonModel> _pokemons;
        private ObservableCollection<TrainerModel> _trainers;
        private PokemonModel _pokemon;
        private TrainerModel _currentOccupier;
        private PokemonModel _currentPokemon;
        private TrainerModel _player;

        private Random _rng;

        private ICommand _challangePlayerCommand;
        private ICommand _selectedPokemonCommand;

        public ICommand ChallangePlayerCommand
        {
            get { return _challangePlayerCommand ?? (_challangePlayerCommand = new RelayCommand(x => { ChallangePlayer(); }, x => EnableButton())); }
        }
        public ICommand SelectedPokemonCommand
        {
            get { return _selectedPokemonCommand ?? (_selectedPokemonCommand = new RelayCommand(x => { SelectedPokemon(x); }, x => !DialogViewModel.IsVisible)); }
        }

        //constructor of GymViewModel
        public GymViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Rng = new Random();
        }

        //properties of GymViewModel
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

        public ObservableCollection<PokemonModel> Pokemons
        {
            get { return _pokemons; }
            set
            {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }
        public ObservableCollection<TrainerModel> Trainers
        {
            get { return _trainers; }
            set
            {
                _trainers = value;
                OnPropertyChanged(nameof(Trainers));
            }
        }
        public PokemonModel Pokemon
        {
            get { return _pokemon; }
            set
            {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
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

        public TrainerModel CurrentOccupier
        {
            get { return _currentOccupier; }
            set
            {
                _currentOccupier = value;
                OnPropertyChanged(nameof(CurrentOccupier));
            }
        }

        public PokemonModel CurrentPokemon
        {
            get { return _currentPokemon; }
            set
            {
                _currentPokemon = value;
                OnPropertyChanged(nameof(CurrentPokemon));
            }
        }
        public Random Rng
        {
            get { return _rng; }
            set
            {
                _rng = value;
                OnPropertyChanged(nameof(Rng));
            }
        }

        //Method of GymViewModel
        public void UpdatePlayer(TrainerModel player)
        {
            Player = player;
            Pokemons = player.Pokemons;
        }

        public void UpdateTrainers(ObservableCollection<TrainerModel> trainers)
        {
            Trainers = trainers;

            var npcs = Trainers.Where(x => x.Type.Equals("NPC"));
            int randomNPCIndex = Rng.Next(npcs.Count());

            CurrentOccupier = npcs.ElementAt(randomNPCIndex);

            int randomPokemonIndex = Rng.Next(CurrentOccupier.Pokemons.Count);
            CurrentPokemon = CurrentOccupier.Pokemons[randomPokemonIndex];
        }

        public void SelectedPokemon(object sender)
        {
            var pokemon = sender as PokemonModel;
            Pokemon = pokemon;
        }
        public void UpdateOccupier(TrainerModel player, PokemonModel pokemon)
        {
            CurrentOccupier = player;
            CurrentPokemon = pokemon;
        }

        public bool EnableButton()
        {
            if (CurrentOccupier == Player)
            {
                CurrentOccupier.ImageSource = $"/PokemonGoClone;component/Images/Players/{Player.Id:D3}S.png"; ;
                return false;
            }
            return true;
        }

        public void ChallangePlayer()
        {
            if (CurrentOccupier != Player)
            {
                if (Pokemon.Health <= 0)
                {
                    DialogViewModel.PopUp("Your pokemon cannot fight, please select other pokemon. ");
                    return;
                }

                ((BattleViewModel)MainWindowViewModel.BattleViewModel).NewBattle(Player, CurrentOccupier, Pokemon, CurrentPokemon, "Gym");
                MainWindowViewModel.GoToBattleViewModel(null);
            }
            else
            {
                var npcTrainers = MainWindowViewModel.Trainers.Where(x => x.Type.Equals("NPC"));
                int newOccupierIndex = Rng.Next(npcTrainers.Count());
                TrainerModel newOccupier = npcTrainers.ElementAt(newOccupierIndex);

                ((BattleViewModel)MainWindowViewModel.BattleViewModel).NewBattle(Player, newOccupier, CurrentPokemon, newOccupier.Pokemons[0], "Gym");
                MainWindowViewModel.GoToBattleViewModel(null);
            }
        }
    }
}
