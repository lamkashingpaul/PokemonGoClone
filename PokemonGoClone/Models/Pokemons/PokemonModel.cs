using PokemonGoClone.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PokemonGoClone.Models.Pokemons
{
    [Serializable]
    public class PokemonModel : BeingModel, ICloneable
    {
        // All fields for TrainerCreationViewModel
        private bool _isChecked;

        // All fields of Pokemon class
        private ObservableCollection<AbilityModel> _abilities;
        private double _accuracy;
        private int[] _evolveId;
        private int _evolveCost;
        private int _powerUpCostBase;
        private int _powerUpCostPerLevel;

        // Default constructor
        public PokemonModel(string name,
                            int id,
                            string description,
                            int level,
                            int maxLevel,
                            int maxHealth,
                            int maxHealthPerLevel,
                            double accuracy,
                            int[] evolveId,
                            int evolveCost,
                            int powerUpCostBase,
                            int powerUpCostPerLevel,
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
            Accuracy = accuracy;
            EvolveId = evolveId;
            EvolveCost = evolveCost;
            PowerUpCostBase = powerUpCostBase;
            PowerUpCostPerLevel = powerUpCostPerLevel;

            Abilities = new ObservableCollection<AbilityModel>();

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

        public ObservableCollection<AbilityModel> Abilities
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

        public int[] EvolveId
        {
            get { return _evolveId; }
            set
            {
                _evolveId = value;
                OnPropertyChanged(nameof(EvolveId));
            }
        }

        public int EvolveCost
        {
            get { return _evolveCost; }
            set
            {
                _evolveCost = value;
                OnPropertyChanged(nameof(EvolveCost));
            }
        }
        public int PowerUpCostBase
        {
            get { return _powerUpCostBase; }
            set
            {
                _powerUpCostBase = value;
                OnPropertyChanged(nameof(PowerUpCostBase));
                OnPropertyChanged(nameof(PowerUpCost));
            }
        }
        public int PowerUpCostPerLevel
        {
            get { return _powerUpCostPerLevel; }
            set
            {
                _powerUpCostPerLevel = value;
                OnPropertyChanged(nameof(PowerUpCostPerLevel));
                OnPropertyChanged(nameof(PowerUpCost));
            }
        }
        public int PowerUpCost
        {
            get { return PowerUpCostBase + Level * PowerUpCostPerLevel; }
        }

        public void LevelUp()
        {
            Level += 1;
            MaxHealth += Rng.Next(MaxHealthPerLevel - 20, MaxHealthPerLevel + 1);
            Health = MaxHealth;
            OnPropertyChanged(nameof(PowerUpCost));
        }


        public void AddAbility(AbilityModel ability)
        {
            if (!Abilities.Any(x => x.Id == ability.Id))
            {
                Abilities.Add(ability);
            }
        }

        public void AddRandomNewAbility(List<AbilityModel> abilities)
        {
            var availableAbilities = abilities.Where(x => Abilities.All(x2 => x2.Id != x.Id)).ToList();
            if (availableAbilities.Count > 0)
            {
                int randomAbilityIndex = Rng.Next(availableAbilities.Count);
                AddAbility((AbilityModel)availableAbilities[randomAbilityIndex].Clone());
            }
        }

        public void DropAbility(AbilityModel ability)
        {
            Abilities.Remove(ability);
        }

        public void FullyRestore()
        {
            Health = MaxHealth;
            foreach (var ability in Abilities)
            {
                ability.FullyRestore();
            }
        }

        public object Clone()
        {
            var pokemonModel = (PokemonModel)MemberwiseClone();
            pokemonModel.Abilities = new ObservableCollection<AbilityModel>();
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
