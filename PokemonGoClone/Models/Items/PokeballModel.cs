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
        public override (string, bool?) Use(TrainerModel player, TrainerModel opponent, PokemonModel playerPokemon, PokemonModel opponentPokemon)
        {
            Charge -= 1;
            double chance = Rng.NextDouble();
            double successChance = 1 - (opponentPokemon.Health / (double)opponentPokemon.MaxHealth) + CatchProbability;
            if (chance <= successChance)
            {
                player.AddPokemon(opponentPokemon);
                return ($"You caught {opponentPokemon.Name}.", true);
            }
            else
            {
                return ("Your pokeball missed", null);
            }
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
