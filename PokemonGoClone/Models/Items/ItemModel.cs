using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.ViewModels;
using System;

namespace PokemonGoClone.Models.Items
{
    [Serializable]
    public abstract class ItemModel : ViewModelBase
    {
        // All fields shared by Item class
        private string _name;
        private int _id;
        private string _description;
        private string _itemType;
        private int _charge;
        private int _cost;
        private string _imageSource;
        private Random _rng;

        public ItemModel(string name, int id, int cost)
        {
            Name = name;
            Id = id;
            Cost = cost;
            Charge = 1;
            Rng = new Random();
        }

        // All methods of Item class
        public abstract string Use(TrainerModel trainer, PokemonModel target);

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
        public int Charge
        {
            get { return _charge; }
            set
            {
                _charge = value;
                OnPropertyChanged(nameof(Charge));
            }
        }
        public int Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                OnPropertyChanged(nameof(ItemType));
            }
        }
        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        public Random Rng
        {
            get { return _rng; }
            set
            {
                _rng = value;
                OnPropertyChanged(nameof(Rng));
            }
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
