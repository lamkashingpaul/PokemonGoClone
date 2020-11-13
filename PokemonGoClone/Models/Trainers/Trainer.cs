using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Trainers
{
    public class Trainer : Being
    {
        // All fields of Trainer class
        private List<Pokemon> _pokemons;
        private List<Item> _items;

        // Default constructor
        public Trainer(string name)
        {
            Name = name;
            Level = 1;
            Health = 128;
        //            private string _name;
        //private int _level;
        //private int _health;
        //private int _maxHealth;
    }

        // All methods of Trainer class
        public void AddPokemon (Pokemon pokemon)
        {

        }

        public void DropPokemon (Pokemon pokemon)
        {

        }

        public void AddItem (Item item)
        {

        }

        public void DropItem (Item item)
        {

        }
    }
}
