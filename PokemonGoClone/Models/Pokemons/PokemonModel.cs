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
        private List<AbilityModel> _abilities;
        private double _accuracy;

        // Default constructor
        public PokemonModel(string name,
                            int id,
                            string description,
                            int level,
                            int maxLevel,
                            int maxHealth,
                            int maxHealthPerLevel,
                            int maxExp,
                            int maxExpPerLevel,
                            double accuracy,
                            AbilityModel ability)
        {
            Name = name;
            Id = id;
            Description = description;
            Level = level;
            MaxLevel = maxLevel;
            MaxHealth = maxHealth;
            Health = maxHealth;
            MaxHealthPerLevel = maxHealthPerLevel;
            MaxExp = maxExp;
            MaxExpPerLevel = maxExpPerLevel;
            Accuracy = accuracy;

            if (ability != null)
            {
                Abilities.Add(ability);
            }

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

        public List<AbilityModel> Abilities
        {
            get { return _abilities; }
            set
            {
                _abilities = value;
            }
        }
        public double Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
            }
        }

        public void LevelUp()
        {
            Level += 1;

            Exp = 0;
            MaxExp += MaxExpPerLevel;

            MaxHealth += MaxHealthPerLevel;
            Health = MaxHealth;
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
