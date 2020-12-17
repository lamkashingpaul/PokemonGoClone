using PokemonGoClone.Models;
using PokemonGoClone.Models.Abilities;
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
        private void EndBattle(object x)
        {
            DialogViewModel.IsVisible = false;
            MainWindowViewModel.GoToMapViewModel(null);
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
        public ObservableCollection<AbilityModel> PlayerPokemonAbilities { get; set; }


        private ICommand _useAbilityCommand;
        private ICommand _afkCommand;
        private ICommand _esacapeCommand;

        public ICommand UseAbilityCommand
        {
            get { return _useAbilityCommand ?? (_useAbilityCommand = new RelayCommand(x => { UseAbility(x); })); }
        }
        public ICommand AFKCommand
        {
            get { return _afkCommand ?? (_afkCommand = new RelayCommand(x => { AFK(x); })); }
        }
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

            // Test Environment, Opponent's Pokemon always starts with full HP
            OpponentPokemon.Health = OpponentPokemon.MaxHealth;

            PlayerPokemonAbilities = new ObservableCollection<AbilityModel>(PlayerPokemon.Abilities);

            BattleLogs.Clear();
            DialogViewModel.DefaultDelegates();

            Id = 0;
        }

        private void UseAbility(object x)
        {
            // If there is overlay, UseAbility() is not allowed
            if (DialogViewModel.IsVisible)
            {
                return;
            }

            if (x == null)
            {
                DialogViewModel.Message = "You must choose an ability to use!";
                return;
            }

            var ability = x as AbilityModel;
            if (ability.Charge == 0)
            {
                DialogViewModel.Message = "Ability has no charge.";
            } else
            {
                string result = ability.Use(PlayerPokemon, OpponentPokemon);
                BattleLogs.Add(new LogModel(result, Id++));
                OpponentTurn();
            }
        }

        private void OpponentTurn()
        {
            if (StateOfBattle() == null) 
            {
                var rng = new Random();
                int i = rng.Next(OpponentPokemon.Abilities.Count);
                var ability = OpponentPokemon.Abilities[i];
                string result = ability.Use(OpponentPokemon, PlayerPokemon);
                BattleLogs.Add(new LogModel(result, Id++));
            }
            StateOfBattle();
        }

        private bool? StateOfBattle()
        {
            // Either true, false or null is return to indicate the state of battle
            // true : Player Wins
            // false: Player Loses
            // null : Not the end of battle

            if (OpponentPokemon.Health == 0)
            {
                DialogViewModel.CloseDelegateMethod = EndBattle;
                DialogViewModel.Message = "You Win";

                // AI also cheats, if he loses, his pokemon is fully restored
                OpponentPokemon.Health = OpponentPokemon.MaxHealth;
                foreach (var Ability in OpponentPokemon.Abilities)
                {
                    Ability.Charge = Ability.MaxCharge;
                }
                // AI cheat ends
                return true;
            } else if (PlayerPokemon.Health == 0)
            {
                DialogViewModel.CloseDelegateMethod = EndBattle;
                DialogViewModel.Message = "You Lose";

                // Cheat, after losing player's pokemon will fully restored and has new skill
                PlayerPokemon.Health = PlayerPokemon.MaxHealth;
                PlayerPokemon.AddAbility((AbilityModel)MainWindowViewModel.Abilities[1].Clone());

                // Fully charge all abilities
                foreach (var Ability in PlayerPokemon.Abilities)
                {
                    Ability.Charge = Ability.MaxCharge;
                }
                // Cheat ends
                return false;
            } else
            {
                return null;
            }
        }
        private void AFK(object x)
        {
            OpponentTurn();
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
                DialogViewModel.CloseDelegateMethod = EndBattle;
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
