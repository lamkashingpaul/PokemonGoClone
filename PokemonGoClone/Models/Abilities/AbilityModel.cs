using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.ViewModels;
using System;
using System.Reflection;

namespace PokemonGoClone.Models.Abilities
{
    // Delegate of special effect for the abitliy
    [Serializable]
    public class AbilityModel : ViewModelBase, ICloneable
    {
        // Defintion of all speical effects
        // Method name of special effect must be same as Name of abitlity
        public string Sleep(TrainerModel player, TrainerModel opponent, PokemonModel playerPokemon, PokemonModel opponentPokemon)
        {
            int turnOfSleep = Rng.Next(1, 5);
            opponent.TurnsUntilAction += turnOfSleep;
            return $"\"{opponentPokemon.Name}\" falls into sleep for additional {turnOfSleep} turn{(turnOfSleep > 1 ? "s" : "")}. ";
        }
        public string Splash(TrainerModel player, TrainerModel opponent, PokemonModel playerPokemon, PokemonModel opponentPokemon)
        {
            return $"{playerPokemon.Name} jumped and did nothing. ";
        }

        // All fields shared by Ability class
        private Random Rng = new Random();

        private string _name;
        private int _id;
        private string _description;

        private int _damage;
        private int _damagePerLevel;

        private int _heal;
        private int _healPerLevel;

        private int _charge;
        private int _maxCharge;

        private double _accuracy;

        // Default constructor
        public AbilityModel(string name,
                            int id,
                            string description,
                            int damage,
                            int damagePerLevel,
                            int heal,
                            int healPerLevel,
                            int maxCharge,
                            double accuracy)
        {
            Name = name;
            Id = id;
            Description = description;
            Damage = damage;
            DamagePerLevel = damagePerLevel;
            Heal = heal;
            HealPerLevel = healPerLevel;
            MaxCharge = maxCharge;
            Charge = MaxCharge;
            Accuracy = accuracy;
        }

        // All methods of Ability class
        public string Use(TrainerModel player, TrainerModel opponent, PokemonModel playerPokemon, PokemonModel opponentPokemon)
        {
            string result = "";
            Charge -= 1;
            double chance = Rng.NextDouble();
            if (chance <= playerPokemon.Accuracy * Accuracy)
            {
                // This ability has damage
                int totalDamage = Damage + DamagePerLevel * playerPokemon.Level;
                if (totalDamage > 0)
                {
                    opponentPokemon.Health -= totalDamage;
                    result += $"\"{playerPokemon.Name}\" dealt [{totalDamage}] damage to \"{opponentPokemon.Name}\" using [{Name}]. ";
                }

                // This ability has heal
                int totalHeal = Heal + HealPerLevel * playerPokemon.Level;
                if (totalHeal > 0)
                {
                    int originalHealth = playerPokemon.Health;
                    playerPokemon.Health = Math.Min(playerPokemon.MaxHealth, playerPokemon.Health + originalHealth);
                    result += $"\"{playerPokemon.Name}\" healed [{playerPokemon.Health - originalHealth}] HP itself using [{Name}]. ";
                }

                // This ability has speical effect
                MethodInfo specialEffect = GetType().GetMethod(Name);
                if (specialEffect != null)
                {
                    result += specialEffect.Invoke(this, new object[] { player, opponent, playerPokemon, opponentPokemon });
                }
            }
            else
            {
                // You ability missed
                result += $"\"{playerPokemon.Name}\"'s [{Name}] missed. ";
            }
            return result;
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
        public int Heal
        {
            get { return _heal; }
            set
            {
                _heal = value;
                OnPropertyChanged(nameof(Heal));
            }
        }

        public int HealPerLevel
        {
            get { return _healPerLevel; }
            set
            {
                _healPerLevel = value;
                OnPropertyChanged(nameof(HealPerLevel));
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
        public void FullyRestore()
        {
            Charge = MaxCharge;
        }

        // Return a deep copy of this abitliy
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
