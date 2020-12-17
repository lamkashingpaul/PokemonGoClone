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
    [Serializable]
    public class TrainerModel : BeingModel
    {
        // All fields of Trainer class
        private string _quote;
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

            Facing = 'S';

            ImageSource = $"/PokemonGoClone;component/Images/{Type}s/{Id:D3}{Facing}.png";

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

        // All methods of Trainer class
        public void AddPokemon(PokemonModel pokemon)
        {
            Pokemons.Add((PokemonModel)(pokemon.Clone()));
        }

        public void DropPokemon(PokemonModel pokemon)
        {

        }

        public void AddItem(ItemModel item)
        {

        }

        public void DropItem(ItemModel item)
        {

        }
    }
}
