using PokemonGoClone.Models;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private string _name;
        private int _choice;

        private const int _col = 11;
        private const int _row = 11;

        public List<Trainer> Trainers { get; private set; }
        public List<Tile> Map { get; private set; }

        public MapViewModel()
        {
            // Draw the boundary of map
            Map = new List<Tile>
            {
                new Tile('B', 0, 0, "005"),
                new Tile('B', 0, COL - 1, "007"),
                new Tile('B', ROW - 1, 0, "009"),
                new Tile('B', ROW - 1, COL - 1, "010")
            };

            for (int i = 0; i < ROW; i += ROW - 1)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    Map.Add(new Tile('B', i, j, "006"));
                }
            }

            for (int i = 0; i < COL; i += COL - 1)
            {
                for (int j = 1; j < ROW - 1; j++)
                {
                    Map.Add(new Tile('B', j, i, "008"));
                }
            }

            var rng = new Random();
            for (int i = 1; i < ROW - 1; i ++)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    string randomGrass = $"{rng.Next(1, 4):D3}";
                    Map.Add(new Tile('G', i, j, randomGrass));
                }
            }

            // Create player
            Trainers = new List<Trainer>
            {
                new Trainer("test", "Player")
                {
                    XCoordinate = ROW / 2,
                    YCoordinate = COL / 2,
                }
            };     
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public int Choice
        {
            get { return _choice; }
            set
            {
                _choice = value;
            }
        }

        public int COL
        {
            get { return _col; }
        }

        public int ROW
        {
            get { return _row; }
        }
    }
}
