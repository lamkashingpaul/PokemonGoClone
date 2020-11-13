﻿using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Abilities
{
    // This is an example class to showcase the 
    public class Headbutt : Ability
    {
        public Headbutt()
        {
            Name = "Headbutt";
            Id = 0;
            Description = "Basic Ability to damage your target";

            Damage = 10;

            Level = 1;
            MaxCharge = 10;
            Charge = MaxCharge;

            Accurancy = 90;
        }
        public override void Use(Pokemon caster, Pokemon target)
        {
            double chance = Rng.NextDouble();
            if (caster.Accurancy * this.Accurancy >= chance)
            {
                if (Damage > 0)
                {
                    target.Health -= (int)Damage * Level;
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
