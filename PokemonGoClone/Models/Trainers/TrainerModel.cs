﻿using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using System;
using System.Collections.Generic;

namespace PokemonGoClone.Models.Trainers
{
    [Serializable]
    public class TrainerModel : BeingModel
    {
        // All fields of Trainer class
        private string _quote;
        private int _candy;
        private int _stardust;
        private int _turnsUntilAction;
        public List<PokemonModel> Pokemons;
        public List<ItemModel> Items;

        // Default constructor
        public TrainerModel(string name, string type, int id)
        {
            Name = name;
            Type = type;
            Id = id;
            Level = 1;
            Health = 128;

            Candy = 5000;
            Stardust = 0;

            Facing = 'S';

            if (Type == "Gym") {
                ImageSource = $"/PokemonGoClone;component/Images/Gym/999.png";
            } else {
                ImageSource = $"/PokemonGoClone;component/Images/{Type}s/{Id:D3}{Facing}.png";
            }
            
            Pokemons = new List<PokemonModel>();
            Items = new List<ItemModel>();
        }

        // All properties of Trainer class
        public string Quote
        {
            get { return _quote; }
            set
            {
                _quote = value;
                OnPropertyChanged(nameof(Quote));
            }
        }
        public int Candy
        {
            get { return _candy; }
            set
            {
                _candy = value;
                OnPropertyChanged(nameof(Candy));
            }
        }
        public int Stardust
        {
            get { return _stardust; }
            set
            {
                _stardust = value;
                OnPropertyChanged(nameof(Stardust));
            }
        }
        public int TurnsUntilAction
        {
            get { return _turnsUntilAction; }
            set
            {
                _turnsUntilAction = value;
                OnPropertyChanged(nameof(TurnsUntilAction));
            }
        }

        // All methods of Trainer class
        public void AddPokemon(PokemonModel pokemon)
        {
            Pokemons.Add((PokemonModel)pokemon.Clone());
        }

        public void DropPokemon(PokemonModel pokemon)
        {
            Pokemons.Remove(pokemon);
        }

        public void AddItem(ItemModel item)
        {
            if (Items.Find(x => x.Id == item.Id) == null)
            {
                Items.Add((ItemModel)item.Clone());
            }
            else
            {
                Items.Find(x => x.Id == item.Id).Charge += item.Charge;
            }
        }

        public void DropItem(ItemModel item)
        {
            Items.Remove(item);
        }
    }
}
