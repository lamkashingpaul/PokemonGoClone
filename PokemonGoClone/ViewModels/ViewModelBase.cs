using System;
using System.ComponentModel;

namespace PokemonGoClone.ViewModels
{
    [Serializable]
    public class ViewModelBase : INotifyPropertyChanged
    {
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
