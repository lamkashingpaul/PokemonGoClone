using PokemonGoClone.ViewModels;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Items
{
    public abstract class ItemModel : ViewModelBase {
        // All fields shared by Item class
        private string _name;
        private int _id;
        private int _charge;
        private string _imageSource;

        public ItemModel(string name, int id, int charge) {
            Name = name;
            Id = id;
            Charge = charge;
        }

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

        public int Charge
        {
            get { return _charge; }
            set
            {
                _charge = value;
            }
        }
        public string ImageSource {
            get { return _imageSource; }
            set {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        public object Clone() {
            return MemberwiseClone();
        }

    }
}
