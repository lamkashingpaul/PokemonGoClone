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
        private const int _col = 10;
        private const int _row = 10;

        private Trainer _player;
        private ObservableObject<Tile> Map;

        public MapViewModel(string name, int choice)
        {
            Map = new ObservableObject<Tile>;
            for (int i = 0; )
            {

            }
        }
    }
}
