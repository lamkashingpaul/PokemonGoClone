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
        private string _description;
        private int _level;
        private int _maxLevel;
        private int _health;
        private int _maxHealth;
        private int _maxHealthPerLevel;
        private int _exp;
        private int _maxExp;
        private int _maxExpPerLevel;

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
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
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
        public int MaxLevel
        {
            get { return _maxLevel; }
            set
            {
                _maxLevel = value;
                OnPropertyChanged(nameof(MaxLevel));
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
        public int Exp
        {
            get { return _exp; }
            set
            {
                _exp = value;
                OnPropertyChanged(nameof(Exp));
            }
        }
        public int MaxExp
        {
            get { return _maxExp; }
            set
            {
                _maxExp = value;
                OnPropertyChanged(nameof(MaxExp));
            }
        }
        public int MaxExpPerLevel
        {
            get { return _maxExpPerLevel; }
            set
            {
                _maxExpPerLevel = value;
                OnPropertyChanged(nameof(MaxExpPerLevel));
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
