﻿using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Items
{
    public class PokeballModel : ItemModel
    {
        public override void Use(TrainerModel trainer, PokemonModel target)
        {
            return;
        }
    }
}