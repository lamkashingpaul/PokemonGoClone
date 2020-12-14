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
        public delegate void ActionDelegate(object x);

        private ActionDelegate _actionDelegateMethod;
        private string _message;

        private bool _isVisible;

        private ICommand _closeCommand;
        private ICommand _actionCommand;

        public DialogViewModel()
        {
            ActionDelegateMethod = null;
            IsVisible = false;
        }

        // All properies of DialogViewModel
        public ActionDelegate ActionDelegateMethod
        {
            get { return _actionDelegateMethod; }
            set
            {
                _actionDelegateMethod = value;
                OnPropertyChanged(nameof(IsActionButtonVisible));
            }
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

        public Visibility IsActionButtonVisible
        {
            get { return ActionDelegateMethod != null ? Visibility.Visible : Visibility.Collapsed; }
        }

        public virtual void Close(object x)
        {
            IsVisible = false;
        }

        public virtual void Action(object x) { }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(x => { Close(x); })); }
        }

        public ICommand ActionCommand
        {
            get { return _actionCommand ?? (_actionCommand = new RelayCommand(x => { ActionDelegateMethod(x); })); }
        }
    }
}
