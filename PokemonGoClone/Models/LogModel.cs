using PokemonGoClone.ViewModels;

namespace PokemonGoClone.Models
{
    public class LogModel : ViewModelBase
    {
        private string _log;
        private int _id;

        public LogModel(string log, int id)
        {
            Log = log;
            Id = id;
        }

        public string Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged(nameof(Log));
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
    }
}
