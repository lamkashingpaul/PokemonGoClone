﻿using PokemonGoClone.Models;
using PokemonGoClone.Models.Abilities;
using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private string _typeOfBattle;

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
            get { return _useAbilityCommand ?? (_useAbilityCommand = new RelayCommand(x => { UseAbility(x); }, x => !DialogViewModel.IsVisible && Player.TurnsUntilAction == 0)); }
        }
        public ICommand UseItemCommand
        {
            get { return _useItemCommand ?? (_useItemCommand = new RelayCommand(x => { UseItem(x); }, x => !DialogViewModel.IsVisible && Player.TurnsUntilAction == 0)); }
        }
        public ICommand AFKCommand
        {
            get { return _afkCommand ?? (_afkCommand = new RelayCommand(x => { AFK(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand EsacapeCommand
        {
            get { return _esacapeCommand ?? (_esacapeCommand = new RelayCommand(x => { Esacape(x); }, x => !DialogViewModel.IsVisible || Player.TurnsUntilAction > 0)); }
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
        public string TypeOfBattle
        {
            get { return _typeOfBattle; }
            set
            {
                _typeOfBattle = value;
                OnPropertyChanged(nameof(TypeOfBattle));
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

        public void NewBattle(TrainerModel player, TrainerModel opponent, PokemonModel playerPokemon, PokemonModel opponetPokemon, string typeOfBattle)
        {
            Player = player;
            Opponent = opponent;
            PlayerPokemon = playerPokemon;
            OpponentPokemon = opponetPokemon;

            PlayerPokemonAbilities = new ObservableCollection<AbilityModel>(PlayerPokemon.Abilities);
            PlayerItems = Player.Items;

            // Create BannedItem table
            TypeOfBattle = typeOfBattle;
            BannedItems = new HashSet<string>();
            if (TypeOfBattle.Equals("Gym") || TypeOfBattle.Equals("NPC"))
            {
                BannedItems.Add("Pokeball");
            }
            else if (TypeOfBattle.Equals("WildPokemon")) { }

            BattleLogs.Clear();
            DialogViewModel.DefaultDelegates();

            Id = 0;
            Player.TurnsUntilAction = 0;
            Opponent.TurnsUntilAction = 0;
        }

        private void UseAbility(object x)
        {
            string result;
            if (x == null)
            {
                DialogViewModel.PopUp("You must choose an ability to use! ");
                return;
            }

            var ability = x as AbilityModel;
            if (ability.Charge == 0)
            {
                DialogViewModel.PopUp("Ability has no charge. ");
            }
            else
            {
                result = ability.Use(Player, Opponent, PlayerPokemon, OpponentPokemon);
                BattleLogs.Add(new LogModel(result, Id++));
                OpponentTurn();
            }
        }

        private void UseItem(object x)
        {
            if (x == null)
            {
                DialogViewModel.PopUp("You must choose an item to use! ");
                return;
            }

            var item = x as ItemModel;
            if (BannedItems.Contains(item.ItemType))
            {
                DialogViewModel.PopUp($"{item.ItemType} is not allowed in {TypeOfBattle} battle! ");
                return;
            }

            var result = item.Use(Player, Opponent, PlayerPokemon, OpponentPokemon);

            BattleLogs.Add(new LogModel(result.Item1, Id++));

            // If the battle is terminated by using item
            if (result.Item2 == true)
            {
                if (item is PokeballModel)
                {
                    // ball is used successfully and the wild pokemon carrier should be remove from the collection
                    ((MapViewModel)MainWindowViewModel.MapViewModel).Trainers.Remove(Opponent);
                }
                DialogViewModel.PopUp(result.Item1, EndBattle);
            }
            else
            {
                OpponentTurn();
            }
        }

        private void OpponentTurn()
        {
            string result;
            if (StateOfBattle() == null)
            {
                if (Opponent.TurnsUntilAction > 0)
                {
                    Opponent.TurnsUntilAction -= 1;
                    result = $"\"{OpponentPokemon.Name}\" passed. ";
                }
                else
                {
                    int abilityId = 0;
                    var damageAbilities = OpponentPokemon.Abilities.Where(x => x.Damage > 0).ToList();
                    var healAbilities = OpponentPokemon.Abilities.Where(x => x.Heal > 0).ToList();

                    // Pokemon must have at least one damage spell by default
                    abilityId = damageAbilities[Opponent.Rng.Next(damageAbilities.Count)].Id;

                    // If AI is in danger, he has 50% of chance to heal if it is possible
                    if (OpponentPokemon.Health / (double)OpponentPokemon.MaxHealth < 0.4 && healAbilities.Count > 0)
                    {
                        double healChance = Rng.NextDouble();
                        if (healChance <= 0.5)
                        {
                            abilityId = healAbilities[Opponent.Rng.Next(healAbilities.Count)].Id;
                        }
                    }

                    var ability = OpponentPokemon.Abilities.Where(x => x.Id == abilityId).FirstOrDefault();
                    ability.Charge += 1;   // AI's ability always has charge

                    result = ability.Use(Opponent, Player, OpponentPokemon, PlayerPokemon);
                }
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
                string result = "You win. " + Reward();
                // If this is gym battle, update the gym occupier
                if (TypeOfBattle.Equals("Gym"))
                {
                    ((GymViewModel)MainWindowViewModel.GymViewModel).UpdateOccupier(Player, PlayerPokemon);
                    result += "You are now occupying this gym. ";
                    PlayerPokemon.Health = PlayerPokemon.MaxHealth;
                    ((MapViewModel)MainWindowViewModel.MapViewModel).GymTimer.Start();
                }

                // AI will train his pokrmon if he loses. His pokemon is trained and level up
                NPCTraining();

                DialogViewModel.PopUp(result, EndBattle);
                return true;
            }
            else if (PlayerPokemon.Health == 0)
            {
                // If it is not gym battle, AI's pokemon is fully recovered aftering he wins.
                if (!TypeOfBattle.Equals("Gym"))
                {
                    OpponentPokemon.Health = OpponentPokemon.MaxHealth;
                }

                DialogViewModel.PopUp("You Lose! ", EndBattle);
                return false;
            }
            else
            {
                return null;
            }
        }
        private string Reward()
        {
            string result = "List of Reward: ";
            double itemChance = Rng.NextDouble();
            if (itemChance <= 0.2)
            {
                var item = MainWindowViewModel.Items[Rng.Next(MainWindowViewModel.Items.Count)];
                Player.AddItem(item);
                result += $"{item.Name}, and ";
            }

            int candy = Rng.Next(PlayerPokemon.Health, (int)(1.1 * PlayerPokemon.Health));
            Player.Candy += candy;
            result += $"{candy} Candy. ";
            return result;

        }
        private void NPCTraining()
        {
            OpponentPokemon.Level++;
            int add = Rng.Next(1, OpponentPokemon.MaxHealthPerLevel + 1);
            OpponentPokemon.MaxHealth += add;
            OpponentPokemon.Health = OpponentPokemon.MaxHealth;
        }

        private void AFK(object x)
        {
            BattleLogs.Add(new LogModel("You chose to AFK. ", Id++));
            if (Player.TurnsUntilAction > 0)
            {
                Player.TurnsUntilAction -= 1;
            }
            OpponentTurn();
        }

        private void Esacape(object x)
        {
            double chance = Rng.NextDouble();
            double successfulEscape = 0.05 + PlayerPokemon.Health / (double)PlayerPokemon.MaxHealth / 5;
            if (chance <= successfulEscape)
            {
                DialogViewModel.PopUp("Successfully Escaped! ", EndBattle);
            }
            else
            {
                DialogViewModel.PopUp("Escape Failed. ");
                BattleLogs.Add(new LogModel("You failed to escape. ", Id++));
                OpponentTurn();
            }
        }
    }
}
