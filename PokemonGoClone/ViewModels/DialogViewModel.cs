using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        private string _message;

        private bool _isVisible;

        private ICommand _okCommand;
        private ICommand _actionCommand;

        public DialogViewModel()
        {
            IsVisible = false;
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                IsVisible = true;
                OnPropertyChanged(nameof(Message));
            }
        }
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsDialogVisible));
            }
        }

        public Visibility IsDialogVisible
        {
            get { return IsVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        public virtual void Ok(object x)
        {
            IsVisible = false;
        }

        public virtual void Action(object x) { }

        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand(x => { Ok(x); })); }
        }

        public ICommand Action2Command
        {
            get { return _actionCommand ?? (_actionCommand = new RelayCommand(x => { Action(x); })); }
        }
    }
}
