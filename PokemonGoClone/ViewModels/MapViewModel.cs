using PokemonGoClone.Models;
using PokemonGoClone.Models.Pokemons;
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
        private MainWindowViewModel _mainWindowViewMode;

        private const int _col = 11;
        private const int _row = 11;

        public List<TrainerModel> Trainers { get; private set; }
        public List<TileModel> Map { get; private set; }

        private ICommand _moveCommand;
        private ICommand _interactCommand;

        public ICommand MoveCommand
        {
            get { return _moveCommand ?? (_moveCommand = new RelayCommand(x => { Move(x); })); }
        }
        public ICommand InteractCommand
        {
            get { return _interactCommand ?? (_interactCommand = new RelayCommand(x => { Interact(); })); }
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewMode; }
            set
            {
                _mainWindowViewMode = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public MapViewModel() { }

        public void GameInitialization(string name, int choice)
        {
            // Draw the boundary of map
            Map = new List<TileModel>
            {
                new TileModel('B', "Tile", 5, 0, 0),
                new TileModel('B', "Tile", 7, 0, COL - 1),
                new TileModel('B', "Tile", 9, ROW - 1, 0),
                new TileModel('B', "Tile", 10, ROW - 1, COL - 1)
            };

            for (int i = 0; i < ROW; i += ROW - 1)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    Map.Add(new TileModel('B', "Tile", 6, i, j));
                }
            }

            for (int i = 0; i < COL; i += COL - 1)
            {
                for (int j = 1; j < ROW - 1; j++)
                {
                    Map.Add(new TileModel('B', "Tile", 8, j, i));
                }
            }

            var rng = new Random();
            for (int i = 1; i < ROW - 1; i++)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    int randomGrass = rng.Next(1, 4);
                    Map.Add(new TileModel('G', "Tile", randomGrass, i, j));
                }
            }

            // Create player
            // Note that the player is always the Trainers[0]

            Trainers = new List<TrainerModel>
            {
                new TrainerModel(name, "Player", 1)
                {
                    XCoordinate = ROW / 2,
                    YCoordinate = COL / 2,
                }
            };

            // Add Pokemon to player
            Trainers[0].AddPokemon((PokemonModel)(MainWindowViewModel.Pokemons.Find(x => x.Id == choice).Clone()));

            // Create BagView for Player


            // Add more NPC trainers
            for (int i = 0; i < 3; i++)
            {
                string randomNPC = $"{i + 1:D3}";
                int randomX, randomY;
                do
                {
                    randomX = rng.Next(1, ROW - 1);
                    randomY = rng.Next(1, ROW - 1);
                } while (Trainers.Find(x => x.XCoordinate == randomX && x.YCoordinate == randomY) != null);
                Trainers.Add(new TrainerModel(randomNPC, "NPC", i + 1) { XCoordinate = randomX, YCoordinate = randomY });
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

        private void Move(object sender)
        {
            string command = sender as string;
            char direction = command[0];
            Trainers[0].Facing = direction;

            // Find if there is any object in front
            var frontTile = Map.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            var frontBeing = Trainers.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);

            if (frontTile.Texture == 'G' && frontBeing == null)
            {
                switch (direction)
                {
                    case 'W':
                        Trainers[0].XCoordinate -= 1;
                        break;
                    case 'S':
                        Trainers[0].XCoordinate += 1;
                        break;
                    case 'A':
                        Trainers[0].YCoordinate -= 1;
                        break;
                    case 'D':
                        Trainers[0].YCoordinate += 1;
                        break;
                    default:
                        break;
                }
                Trainers[0].Facing = direction;
            }
        }

        private void Interact()
        {
            var frontObject = Trainers.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            if (frontObject == null)
            {
                return;
            }

            Console.WriteLine($"{frontObject.Name} Found at ({frontObject.XCoordinate}, {frontObject.YCoordinate}).");
        }
    }
}
