using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Abilities
{
    public class AbilityModel : ViewModelBase
    {
        // All fields shared by Ability class
        private Random Rand = new Random();

        private string _name;
        private int _id;
        private string _description;

        private int _damage;
        private int _damagePerLevel;

        private int _level;
        private int _charge;
        private int _maxCharge;

        private double _accuracy;

        // Default constructor
        public AbilityModel(string name,
                            int id,
                            string description,
                            int damage,
                            int damagePerLevel,
                            int level,
                            int maxCharge,
                            double accuracy)
        {
            Name = name;
            Id = id;
            Description = description;
            Damage = damage;
            DamagePerLevel = damagePerLevel;
            Level = level;
            MaxCharge = maxCharge;
            Charge = MaxCharge;
            Accuracy = accuracy;
        }

        // All methods of Ability class
        public void Use(PokemonModel caster, PokemonModel target)
        {
            Console.WriteLine("Ability used.");
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

        // All properties of fields
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public int Damage
        {
            get { return _damage; }
            set
            {
                _damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }

        public int DamagePerLevel
        {
            get { return _damagePerLevel; }
            set
            {
                _damagePerLevel = value;
                OnPropertyChanged(nameof(DamagePerLevel));
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        public int Charge
        {
            get { return _charge; }
            set
            {
                _charge = value;
                OnPropertyChanged(nameof(Charge));
            }
        }

        public int MaxCharge
        {
            get { return _maxCharge; }
            set
            {
                _maxCharge = value;
                OnPropertyChanged(nameof(MaxCharge));
            }
        }

        public double Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
                OnPropertyChanged(nameof(Accuracy));
            }
        }

        public void LevelUp()
        {
            Level += 1;
            Damage += DamagePerLevel;
        }
    }
}
