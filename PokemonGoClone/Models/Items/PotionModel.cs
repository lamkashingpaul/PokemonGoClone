using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;

namespace PokemonGoClone.Models.Items
{
    [Serializable]
    public class PotionModel : ItemModel
    {
        //field of Potion
        private int _healHP;
        public PotionModel(string name, int id, string description, int charge, int healHP) : base(name, id, charge)
        {
            Description = description;
            HealHP = healHP;
            ItemType = "Potion";
            ImageSource = $"/PokemonGoClone;component/Images/Items/Potions/{Id:D6}.png";
        }
        public override string Use(TrainerModel trainer, PokemonModel target)
        {
            Charge -= 1;
            var TrainerPokemon = trainer.Pokemons[0];
            int originalHealth = TrainerPokemon.Health;
            TrainerPokemon.Health = Math.Min(TrainerPokemon.MaxHealth, TrainerPokemon.Health + HealHP);
            return $"You healed your {TrainerPokemon.Name} by {TrainerPokemon.Health - originalHealth} HP.";
        }

        //Properties of PokeballModel
        public int HealHP
        {
            get { return _healHP; }
            set
            {
                _healHP = value;
            }
        }
    }
}
