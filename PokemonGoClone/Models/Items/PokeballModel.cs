using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;

namespace PokemonGoClone.Models.Items
{
    [Serializable]
    public class PokeballModel : ItemModel
    {
        //field of Pokemonball
        private double _catchProbability;

        //derived class constructor
        public PokeballModel(string name, int id, string description, int cost, double catchProbability) : base(name, id, cost)
        {
            Description = description;
            CatchProbability = catchProbability;
            ItemType = "Pokeball";
            ImageSource = $"/PokemonGoClone;component/Images/Items/Pokeballs/{Id:D6}.png";
        }
        public override string Use(TrainerModel trainer, PokemonModel target)
        {
            return "";
        }

        //Properties of PokeballModel
        public double CatchProbability
        {
            get { return _catchProbability; }
            set
            {
                _catchProbability = value;
                OnPropertyChanged(nameof(CatchProbability));
            }
        }
    }
}
