﻿using PokemonGoClone.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Pokemons
{
    public class Pokemon : Being
    {
        // All fields of Pokemon class
        private int _id;
        private List<Ability> _abilities;
        private double _accurancy;

        // Default constructor
        public Pokemon(int id, string name, int level, int maxHealth, Ability randomAbility)
        {
            Id = id;
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Abilities.Add(randomAbility);
            Accurancy = 1;
        }

        // All properties of Pokemon class
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public List<Ability> Abilities
        {
            get { return _abilities; }
            set
            {
                _abilities = value;
            }
        }
        public double Accurancy
        {
            get { return _accurancy; }
            set
            {
                _accurancy = value;
            }
        }

        public void AddAbility(Ability ability)
        {
            if (!Abilities.Exists(x => x.Id == ability.Id))
            {
                Abilities.Add(ability);
            }
        }

        public void DropAbility(Ability ability)
        {
            Abilities.Remove(ability);
        }
    }
}