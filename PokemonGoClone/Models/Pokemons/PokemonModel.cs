using PokemonGoClone.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PokemonGoClone.Models.Pokemons
{
    public class PokemonModel : BeingModel, ICloneable
    {
        // All fields for TrainerCreation
        private bool _isChecked;

        // All fields of Pokemon class
        private List<AbilityModel> _abilities;
        private double _accuracy;
        private int _powerUpCost;

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
                            int evolveId,
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
            EvolveId = evolveId;
            Accuracy = accuracy;

            Abilities = new List<AbilityModel>();

            if (ability != null)
            {
                Abilities.Add((AbilityModel)ability.Clone());
            }

            ImageSource = $"/PokemonGoClone;component/Images/Pokemons/{Id:D3}.png";
        }

        // Constructor for Starting Pokemon
        public PokemonModel(int id, string name, bool isChecked)
        {
            Id = id;
            Name = name;
            IsChecked = isChecked;

            ImageSource = $"/PokemonGoClone;component/Images/Pokemons/{Id:D3}.png";
        }


        // All properties of Pokemon class
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
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
        public double Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
            }
        }
     
        public int EvolveCost() {
            if (EvolveId != -1) {
                return Level * 100;
            } else {
                return 0;
            }        
        }

        public void LevelUp() {
            Level += 1;
            Random rnd = new Random();
            MaxHealth += rnd.Next(MaxHealthPerLevel - 20, MaxHealthPerLevel + 1);
            Health = MaxHealth;
            LevelUpCost = Level * 100;
        }

        public int LevelUpCost {
            get { return _powerUpCost; }
            set {
                _powerUpCost = value;
                OnPropertyChanged(nameof(LevelUpCost));
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

        public object Clone()
        {
            var pokemonModel = (PokemonModel)MemberwiseClone();
            pokemonModel.Abilities = new List<AbilityModel>();
            if (Abilities.Count > 0)
            {
                foreach (var ability in Abilities)
                {
                    pokemonModel.AddAbility((AbilityModel)ability.Clone());
                }
            }
            return pokemonModel;
        }
    }
}
