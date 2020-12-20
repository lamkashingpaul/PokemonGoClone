using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.ViewModels;
using System;

namespace PokemonGoClone.Models.Abilities
{
    [Serializable]
    public class AbilityModel : ViewModelBase, ICloneable
    {
        // All fields shared by Ability class
        private Random Rand = new Random();

        private string _name;
        private int _id;
        private string _description;

        private int _damage;
        private int _damagePerLevel;

        private int _charge;
        private int _maxCharge;
        private int _maxChargePerLevel;

        private double _accuracy;

        // Default constructor
        public AbilityModel(string name,
                            int id,
                            string description,
                            int damage,
                            int damagePerLevel,
                            int maxCharge,
                            int maxChargePerLevel,
                            double accuracy)
        {
            Name = name;
            Id = id;
            Description = description;
            Damage = damage;
            DamagePerLevel = damagePerLevel;
            MaxCharge = maxCharge;
            MaxChargePerLevel = maxChargePerLevel;
            Charge = MaxCharge;
            Accuracy = accuracy;
        }

        // All methods of Ability class
        public string Use(PokemonModel caster, PokemonModel target)
        {
            Console.WriteLine("Ability used.");
            Charge -= 1;
            double chance = Rand.NextDouble();
            if (chance < caster.Accuracy * Accuracy)
            {
                if (Damage > 0)
                {
                    target.Health -= Damage + DamagePerLevel * caster.Level;
                    return $"{caster.Name} dealt {Damage} damage to {target.Name}";
                }
                else
                {
                    // This is not damage ability
                    // Action shall be implemented here
                    return "This is not damage ability.";

                }
            }
            else
            {
                // You ability missed
                return $"{caster.Name}'s ability missed.";
            }
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
        public int MaxChargePerLevel
        {
            get { return _maxChargePerLevel; }
            set
            {
                _maxChargePerLevel = value;
                OnPropertyChanged(nameof(MaxChargePerLevel));
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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
