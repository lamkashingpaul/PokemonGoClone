using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Abilities
{
    // This is an example class to showcase the
    public class AbilityModel001 : AbilityModel
    {
        public AbilityModel001()
        {
            Name = "Headbutt";
            Id = 1;
            Description = "Basic Ability to damage your target";

            Damage = 10;

            Level = 1;
            MaxCharge = 10;
            Charge = MaxCharge;

            Accuracy = 0.9;
        }
        public override void Use(PokemonModel caster, PokemonModel target)
        {
            double chance = Rand.NextDouble();
            if (caster.Accuracy * Accuracy >= chance)
            {
                if (Damage > 0)
                {
                    target.Health -= Damage * Level;
                }
                else
                {
                    // This is not damage ability
                    // Action shall be implemented here
                }
            }
            Charge -= 1;
        }
    }
}
