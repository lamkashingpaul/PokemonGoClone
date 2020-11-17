using PokemonGoClone.Models;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private string _name;
        private int _choice;

        private const int _col = 11;
        private const int _row = 11;

        public List<TrainerModel> Trainers { get; private set; }
        public List<TileModel> Map { get; private set; }

        private ICommand _moveWCommand;
        private ICommand _moveSCommand;
        private ICommand _moveACommand;
        private ICommand _moveDCommand;

        public ICommand MoveWCommand
        {
            get { return _moveWCommand ?? (_moveWCommand = new RelayCommand(x => { MoveW(); })); }
        }
        public ICommand MoveSCommand
        {
            get { return _moveSCommand ?? (_moveSCommand = new RelayCommand(x => { MoveS(); })); }
        }
        public ICommand MoveACommand
        {
            get { return _moveACommand ?? (_moveACommand = new RelayCommand(x => { MoveA(); })); }
        }
        public ICommand MoveDCommand
        {
            get { return _moveDCommand ?? (_moveDCommand = new RelayCommand(x => { MoveD(); })); }
        }

        public MapViewModel()
        {
            // Draw the boundary of map
            Map = new List<TileModel>
            {
                new TileModel('B', 0, 0, "005"),
                new TileModel('B', 0, COL - 1, "007"),
                new TileModel('B', ROW - 1, 0, "009"),
                new TileModel('B', ROW - 1, COL - 1, "010")
            };

            for (int i = 0; i < ROW; i += ROW - 1)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    Map.Add(new TileModel('B', i, j, "006"));
                }
            }

            for (int i = 0; i < COL; i += COL - 1)
            {
                for (int j = 1; j < ROW - 1; j++)
                {
                    Map.Add(new TileModel('B', j, i, "008"));
                }
            }

            var rng = new Random();
            for (int i = 1; i < ROW - 1; i ++)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    string randomGrass = $"{rng.Next(1, 4):D3}";
                    Map.Add(new TileModel('G', i, j, randomGrass));
                }
            }

            // Create player
            Trainers = new List<TrainerModel>
            {
                new TrainerModel("test", "Player")
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

        private void MoveW()
        {
            Console.WriteLine("WWW");
            Trainers[0].Facing = 'W';
            var frontTile = Map.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            if (frontTile.Texture == 'G')
            {
                Trainers[0].XCoordinate -= 1;
            }
        }
        private void MoveS()
        {
            Console.WriteLine("SSS");
            Trainers[0].Facing = 'S';
            var frontTile = Map.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            if (frontTile.Texture == 'G')
            {
                Trainers[0].XCoordinate += 1;
            }
        }
        private void MoveA()
        {
            Console.WriteLine("AAA");
            Trainers[0].Facing = 'A';
            var frontTile = Map.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            if (frontTile.Texture == 'G')
            {
                Trainers[0].YCoordinate -= 1;
            }
        }
        private void MoveD()
        {
            Console.WriteLine("AAA");
            Trainers[0].Facing = 'D';
            var frontTile = Map.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            if (frontTile.Texture == 'G')
            {
                Trainers[0].YCoordinate += 1;
            }
        }
    }
}
