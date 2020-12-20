﻿using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class ShopViewModel : ViewModelBase
    {
        //field of ShopViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private List<ItemModel> _defaultItem;
        private TrainerModel _trainer;
        private ItemModel _choose;
        public PokemonModel _random;
        private ICommand _selectedItemCommand;
        private ICommand _buyCommand;
        private ICommand _randomCommand;
        public ICommand SelectedItemCommand
        {
            get { return _selectedItemCommand ?? (_selectedItemCommand = new RelayCommand(x => { SelectedItem(x); })); }
        }
        public ICommand BuyCommand
        {
            get { return _buyCommand ?? (_buyCommand = new RelayCommand(x => { Buy(); })); }
        }

        public ICommand RandomCommand
        {
            get { return _randomCommand ?? (_randomCommand = new RelayCommand(x => { RandomPokemon(); })); }
        }

        //constructor
        public ShopViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
        }

        //properties of ShopViewModel
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public TrainerModel Trainer
        {
            get { return _trainer; }
            set
            {
                _trainer = value;
                OnPropertyChanged(nameof(Trainer));
            }
        }

        public List<ItemModel> DefaultItem
        {
            get { return _defaultItem; }
            set
            {
                _defaultItem = value;
                OnPropertyChanged(nameof(DefaultItem));
            }
        }
        public ItemModel Choose
        {
            get { return _choose; }
            set
            {
                _choose = value;
                OnPropertyChanged(nameof(Choose));
            }
        }

        public PokemonModel Random
        {
            get { return _random; }
            set
            {
                _random = value;
                OnPropertyChanged(nameof(Random));
            }
        }

        //Method of ShopViewModel
        public void Update(TrainerModel trainer, List<ItemModel> defaultItem)
        {
            DefaultItem = defaultItem;
            Trainer = trainer;
            Choose = MainWindowViewModel.Items[0];
        }

        public void SelectedItem(object item)
        {
            Choose = item as ItemModel;
        }

        public void Buy()
        {
            Trainer.Money -= Choose.Charge;
            Trainer.AddItem(Choose);
        }

        public void RandomPokemon()
        {
            Trainer.Money -= 500;
            PokemonModel pokemon = RandomPokemonMethod();
            Random = pokemon;
            Trainer.AddPokemon(pokemon);
        }

        //factory Method
        public PokemonModel RandomPokemonMethod()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, MainWindowViewModel.Pokemons.Count);
            PokemonModel pokemon = MainWindowViewModel.Pokemons[x];
            int originalHealth = pokemon.MaxHealth;
            pokemon.MaxHealth = rnd.Next(originalHealth - 200, originalHealth + 101);
            if (pokemon.MaxHealth <= originalHealth)
            {
                pokemon.Description = "It is Normal (N) Pokemon!";
            }
            else if (pokemon.MaxHealth <= originalHealth + 90)
            {
                pokemon.Description = "It is Rare (R) Pokemon!";
            }
            else if (pokemon.MaxHealth < originalHealth + 100)
            {
                pokemon.Description = "It is Super Rare (SR) Pokemon!";
            }
            else
            {
                pokemon.Description = "It is Ultra Rare (UR) Pokemon!";
            }

            return pokemon;
        }


    }
}

