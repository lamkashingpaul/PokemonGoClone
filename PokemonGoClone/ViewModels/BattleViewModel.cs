using PokemonGoClone.Models;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BattleViewModel : ViewModelBase
    {
        private void EscapedSuccessful(object x)
        {
            DialogViewModel.IsVisible = false;
            MainWindowViewModel.GoToMapViewModel();
        }

        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private TrainerModel _player;
        private TrainerModel _opponent;

        private PokemonModel _playerPokemon;
        private PokemonModel _opponentPokemon;

        private int _id;

        private Random _rng;

        public ObservableCollection<LogModel> BattleLogs { get; set; }

        private ICommand _esacapeCommand;
        
        public ICommand EsacapeCommand
        {
            get { return _esacapeCommand ?? (_esacapeCommand = new RelayCommand(x => { Esacape(x); })); }
        }

        public BattleViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = new DialogViewModel(this);
            BattleLogs = new ObservableCollection<LogModel>();
            Rng = new Random();
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewMode; }
            set
            {
                _mainWindowViewMode = value;
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

        public Random Rng
        {
            get { return _rng; }
            set
            {
                _rng = value;
                OnPropertyChanged(nameof(Rng));
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
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

        public TrainerModel Opponent
        {
            get { return _opponent; }
            set
            {
                _opponent = value;
                OnPropertyChanged(nameof(Opponent));
            }
        }
        public PokemonModel PlayerPokemon
        {
            get { return _playerPokemon; }
            set
            {
                _playerPokemon = value;
                OnPropertyChanged(nameof(PlayerPokemon));
            }
        }

        public PokemonModel OpponentPokemon
        {
            get { return _opponentPokemon; }
            set
            {
                _opponentPokemon = value;
                OnPropertyChanged(nameof(OpponentPokemon));
            }
        }

        public void NewBattle(TrainerModel player, TrainerModel opponent)
        {
            Player = player;
            Opponent = opponent;
            PlayerPokemon = Player.Pokemons[0];
            OpponentPokemon = Opponent.Pokemons[0];

            BattleLogs.Clear();
            DialogViewModel.DefaultDelegates();

            Id = 0;
        }

        private void Esacape(object x)
        {
            // If there is overlay, Escape() is not allowed
            if (DialogViewModel.IsVisible)
            {
                return;
            }

            double chance = Rng.NextDouble();

            if (chance < 0.5)
            {
                DialogViewModel.CloseDelegateMethod = EscapedSuccessful;
                DialogViewModel.Message = "Successfully Escaped!";
            }
            else
            {
                DialogViewModel.Message = "Escape Failed.";
                BattleLogs.Add(new LogModel("You failed to escape", Id++));
            }
        }
    }
}
