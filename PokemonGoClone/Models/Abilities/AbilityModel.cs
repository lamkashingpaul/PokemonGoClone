using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Abilities
{
    public abstract class AbilityModel
    {   
        // All fields shared by Ability class
        private string _name;
        private int _id;
        private string _description;

        private int _damage;

        private int _level;
        private int _charge;
        private int _maxCharge;

        private double _accurancy;

        private Random _rng;

        // Default constructor
        public AbilityModel()
        {
            Rng = new Random();
        }

        // All methods of Ability class
        public abstract void Use(PokemonModel caster, PokemonModel target);

        // All properties of fields
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public int Damage
        {
            get { return _damage; }
            set
            {
                _damage = value;
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
            }
        }
        public int Charge
        {
            get { return _charge; }
            set
            {
                _charge = value;
            }
        }

        public int MaxCharge
        {
            get { return _maxCharge; }
            set
            {
                _maxCharge = value;
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

        public Random Rng
        {
            get { return _rng; }
            set
            {
                _rng = value;
            }
        }
    }
}
