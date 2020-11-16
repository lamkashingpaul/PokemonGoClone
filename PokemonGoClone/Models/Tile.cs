using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models
{
    public class Tile
    {
        private char _texture;
        private int _xCoordinate;
        private int _yCoordinate;

        private string _imageSource;

        // Default constructor
        public Tile()
        {

        }

        // Constructor used to draw the map
        public Tile(char texture, int xCoordinate, int yCoordinate, string imageSource)
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
            }
        }
        public int XCoordinate
        {
            get { return _xCoordinate; }
            set
            {
                _xCoordinate = value;
            }
        }
        public int YCoordinate
        {
            get { return _yCoordinate; }
            set
            {
                _yCoordinate = value;
            }
        }

        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
            }
        }
    }
}
