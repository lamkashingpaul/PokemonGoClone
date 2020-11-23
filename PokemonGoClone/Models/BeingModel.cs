using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models
{
    // This is an abstract class for all beings in the game.
    // All beings are expected to share all following fields.
    public abstract class BeingModel : TileModel
    {
        private string _name;
        private int _level;
        private int _health;
        private int _maxHealth;
        private int _maxHealthPerLevel;

        private char _facing;

        private int _xFacing;
        private int _yFacing;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                OnPropertyChanged(nameof(MaxHealth));
            }
        }

        public int MaxHealthPerLevel
        {
            get { return _maxHealthPerLevel; }
            set
            {
                _maxHealthPerLevel = value;
                OnPropertyChanged(nameof(MaxHealthPerLevel));
            }
        }

        public char Facing
        {
            get { return _facing; }
            set
            {
                _facing = value;
                if (_facing == 'W')
                {
                    XFacing = XCoordinate - 1;
                    YFacing = YCoordinate;
                }
                else if (_facing == 'S')
                {
                    XFacing = XCoordinate + 1;
                    YFacing = YCoordinate;
                }
                else if (_facing == 'A')
                {
                    XFacing = XCoordinate;
                    YFacing = YCoordinate - 1;
                }
                else if (_facing == 'D')
                {
                    XFacing = XCoordinate;
                    YFacing = YCoordinate + 1;
                }

                ImageSource = $"/PokemonGoClone;component/Images/{Type}s/{Id:D3}{Facing}.png";
                OnPropertyChanged(nameof(Facing));
            }
        }

        public int XFacing
        {
            get { return _xFacing; }
            set
            {
                _xFacing = value;
                OnPropertyChanged(nameof(XFacing));
            }
        }

        public int YFacing
        {
            get { return _yFacing; }
            set
            {
                _yFacing = value;
                OnPropertyChanged(nameof(YFacing));
            }
        }
    }
}
