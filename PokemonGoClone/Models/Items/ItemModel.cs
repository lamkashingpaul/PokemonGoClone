using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Items
{
    [Serializable]
    public abstract class ItemModel
    {
        // All fields shared by Item class
        private string _name;
        private int _id;
        private string _description;

        private int _charge;
        private int _maxCharge;

        // All methods of Item class
        public abstract void Use(TrainerModel trainer, PokemonModel target);

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
    }
}
