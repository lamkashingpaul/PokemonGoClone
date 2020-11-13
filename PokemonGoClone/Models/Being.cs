using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models
{
    // This is an abstract class for all beings in the game.
    // All beings are expected to share all following fields.
    public abstract class Being
    {
        private string _name;
        private int _level;
        private int _health;
        private int _maxHealth;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
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

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
            }
        }
         public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
            }
        }
    }
}
