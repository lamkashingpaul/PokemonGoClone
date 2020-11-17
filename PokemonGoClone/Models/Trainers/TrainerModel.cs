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
    public class TrainerModel : BeingModel
    {
        // All fields of Trainer class
        private List<PokemonModel> _pokemons;
        private List<ItemModel> _items;

        // Default constructor
        public TrainerModel(string name, string type)
        {
            Name = name;
            Type = type;
            Level = 1;
            Health = 128;

            Facing = 'S';

            ImageSource = $"/PokemonGoClone;component/Images/{Type}s/{Type}{Facing}.png";
        }

        // All methods of Trainer class
        public void AddPokemon (PokemonModel pokemon)
        {

        }

        public void DropPokemon (PokemonModel pokemon)
        {

        }

        public void AddItem (ItemModel item)
        {

        }

        public void DropItem (ItemModel item)
        {

        }
    }
}
