using PokemonGoClone.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PokemonGoClone.Models.Pokemons
{
    public class PokemonModel : BeingModel
    {
        // All fields of Pokemon class
        private int _id;
        private List<AbilityModel> _abilities;
        private double _accurancy;

        // Default constructor
        public PokemonModel(int id, string name, int level, int maxHealth, AbilityModel randomAbility)
        {
            Id = id;
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Abilities.Add(randomAbility);
            Accurancy = 1;

            ImageSource = $"/PokemonGoClone;component/Images/Pokemons/{Id:D3}.png";
        }

        // Constructor for Starting Pokemon
        public PokemonModel(int id, string name)
        {
            Id = id;
            Name = name;

            ImageSource = $"/PokemonGoClone;component/Images/Pokemons/{Id:D3}.png";
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

        public List<AbilityModel> Abilities
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

        public void AddAbility(AbilityModel ability)
        {
            if (!Abilities.Exists(x => x.Id == ability.Id))
            {
                Abilities.Add(ability);
            }
        }

        public void DropAbility(AbilityModel ability)
        {
            Abilities.Remove(ability);
        }
    }
}
