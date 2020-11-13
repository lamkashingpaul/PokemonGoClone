using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Items
{
    public class Potion : Item
    {
        public override void Use(Trainer trainer, Pokemon target)
        {
            target.Health = target.MaxHealth;
            Charge -= 1;
        }
    }
}
