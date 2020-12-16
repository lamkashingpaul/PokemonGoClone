﻿using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BattleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;
        private TrainerModel _player;
        private TrainerModel _opponent;

        private PokemonModel _playerPokemon;
        private PokemonModel _opponentPokemon;

        private ICommand _esacapeCommand;
        
        public ICommand EsacapeCommand
        {
            get { return _esacapeCommand ?? (_esacapeCommand = new RelayCommand(x => { Esacape(x); })); }
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

        public BattleViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
        }

        public void NewBattle(TrainerModel player, TrainerModel opponent)
        {
            Player = player;
            Opponent = opponent;
            PlayerPokemon = Player.Pokemons[0];
            OpponentPokemon = Opponent.Pokemons[0];
        }

        private void Esacape(object x)
        {
            throw new NotImplementedException();
        }
    }
}
