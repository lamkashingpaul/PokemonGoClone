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
        private int _xCoordinate;
        private int _yCoordinate;

        private string _imageSource;

        // Default constructor
        public TileModel()
        {

        }

        // Constructor used to draw the map
        public TileModel(char texture, int xCoordinate, int yCoordinate, string imageSource)
        {
            Texture = texture;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            ImageSource = $"/PokemonGoClone;component/Images/Tiles/{imageSource}.png";
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
