using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.ViewModels
{
    public class BattleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewMode; }
            set
            {
                _mainWindowViewMode = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public BattleViewModel() { }
    }
}
