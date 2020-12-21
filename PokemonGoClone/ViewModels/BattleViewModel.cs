using PokemonGoClone.Models;
using PokemonGoClone.Models.Abilities;
using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BattleViewModel : ViewModelBase
    {
        private void EndBattle(object x)
        {
            DialogViewModel.Close(x);
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

        public HashSet<string> BannedItems { get; set; }
        public ObservableCollection<LogModel> BattleLogs { get; set; }
        public ObservableCollection<AbilityModel> PlayerPokemonAbilities { get; set; }
        public ObservableCollection<ItemModel> PlayerItems { get; set; }


        private ICommand _useAbilityCommand;
        private ICommand _useItemCommand;
        private ICommand _afkCommand;
        private ICommand _esacapeCommand;

        public ICommand UseAbilityCommand
        {
            get { return _useAbilityCommand ?? (_useAbilityCommand = new RelayCommand(x => { UseAbility(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand UseItemCommand
        {
            get { return _useItemCommand ?? (_useItemCommand = new RelayCommand(x => { UseItem(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand AFKCommand
        {
            get { return _afkCommand ?? (_afkCommand = new RelayCommand(x => { AFK(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand EsacapeCommand
        {
            get { return _esacapeCommand ?? (_esacapeCommand = new RelayCommand(x => { Esacape(x); }, x => !DialogViewModel.IsVisible)); }
        }

        public BattleViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;

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

        public void NewBattle(TrainerModel player, TrainerModel opponent, string typeOfBattle)
        {
            Player = player;
            Opponent = opponent;
            PlayerPokemon = Player.Pokemons[0];
            OpponentPokemon = Opponent.Pokemons[0];

            // Test Environment, Opponent's Pokemon always starts with full HP
            OpponentPokemon.Health = OpponentPokemon.MaxHealth;

            PlayerPokemonAbilities = new ObservableCollection<AbilityModel>(PlayerPokemon.Abilities);
            PlayerItems = new ObservableCollection<ItemModel>(Player.Items);

            // Create BannedItem table
            BannedItems = new HashSet<string>();
            if (typeOfBattle.Equals("Gym"))
            {
                BannedItems.Add("Pokeball");
            } else if (typeOfBattle.Equals("")) { }

            BattleLogs.Clear();
            DialogViewModel.DefaultDelegates();

            Id = 0;
        }

        private void UseAbility(object x)
        {
            if (x == null)
            {
                DialogViewModel.PopUp("You must choose an ability to use!");
                return;
            }

            var ability = x as AbilityModel;
            if (ability.Charge == 0)
            {
                DialogViewModel.PopUp("Ability has no charge.");
            }
            else
            {
                string result = ability.Use(PlayerPokemon, OpponentPokemon);
                BattleLogs.Add(new LogModel(result, Id++));
                OpponentTurn();
            }
        }

        private void UseItem(object x)
        {

        }

        private void OpponentTurn()
        {
            if (StateOfBattle() == null)
            {
                int i = Rng.Next(OpponentPokemon.Abilities.Count);
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
                DialogViewModel.PopUp("You Win", EndBattle);

                // AI also cheats, if he loses, his pokemon is fully restored
                OpponentPokemon.Health = OpponentPokemon.MaxHealth;
                foreach (var Ability in OpponentPokemon.Abilities)
                {
                    Ability.Charge = Ability.MaxCharge;
                }
                // AI cheat ends
                return true;
            }
            else if (PlayerPokemon.Health == 0)
            {
                DialogViewModel.PopUp("You Lose", EndBattle);
                /*
                // Cheat, after losing player's pokemon will fully restored and has new ability
                PlayerPokemon.Health = PlayerPokemon.MaxHealth;
                PlayerPokemon.AddRandomNewAbility(MainWindowViewModel.Abilities);

                // Fully charge all abilities
                foreach (var Ability in PlayerPokemon.Abilities)
                {
                    Ability.Charge = Ability.MaxCharge;
                }
                // Cheat ends*/
                return false;
            }
            else
            {
                return null;
            }
        }
        private void AFK(object x)
        {
            BattleLogs.Add(new LogModel("You chose to AFK", Id++));
            OpponentTurn();
        }

        private void Esacape(object x)
        {
            double chance = Rng.NextDouble();

            if (chance <= 0.5)
            {
                DialogViewModel.PopUp("Successfully Escaped!", EndBattle);
            }
            else
            {
                DialogViewModel.PopUp("Escape Failed.");
                BattleLogs.Add(new LogModel("You failed to escape", Id++));
            }
            OpponentTurn();
        }
    }
}
