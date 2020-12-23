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
        public override (string, bool?) Use(TrainerModel player, TrainerModel opponent, PokemonModel playerPokemon, PokemonModel opponentPokemon)
        {
            Charge -= 1;
            int originalHealth = playerPokemon.Health;
            playerPokemon.Health = Math.Min(playerPokemon.MaxHealth, playerPokemon.Health + HealHP);

            if (Charge == 0)
            {
                player.DropItem(this);
            }

            return ($"You healed your \"{playerPokemon.Name}\" [{playerPokemon.Health - originalHealth}] HP using [{Name}]. ", null);
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
