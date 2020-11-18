using PokemonGoClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models
{
    public class TileModel : ViewModelBase
    {
        private char _texture;
        private string _type;
        private int _id;

        private int _xCoordinate;
        private int _yCoordinate;

        private string _imageSource;

        // Default constructor
        public TileModel()
        {

        }

        // Constructor used to draw the map
        public TileModel(char texture, string type, int id, int xCoordinate, int yCoordinate)
        {
            Texture = texture;
            Type = type;
            Id = id;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            ImageSource = $"/PokemonGoClone;component/Images/{Type}s/{Id:D3}.png";
        }

        public char Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                OnPropertyChanged(nameof(Texture));
            }
        }
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
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
        public int XCoordinate
        {
            get { return _xCoordinate; }
            set
            {
                _xCoordinate = value;
                OnPropertyChanged(nameof(XCoordinate));
            }
        }
        public int YCoordinate
        {
            get { return _yCoordinate; }
            set
            {
                _yCoordinate = value;
                OnPropertyChanged(nameof(YCoordinate));
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
    }
}
