using PokemonGoClone.Utilities;
using System.Windows;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        public delegate void ActionDelegate(object x);
        public delegate void CloseDelegate(object x);

        private MainWindowViewModel _mainWindowViewModel;
        private ActionDelegate _actionDelegateMethod;
        private ActionDelegate _closeDelegateMethod;
        private string _message;

        private bool _isVisible;

        private ICommand _closeCommand;
        private ICommand _actionCommand;

        public DialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DefaultDelegates();
            IsVisible = false;
        }

        public void DefaultDelegates()
        {
            ActionDelegateMethod = null;
            CloseDelegateMethod = Close;
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
        public ActionDelegate CloseDelegateMethod
        {
            get { return _closeDelegateMethod; }
            set
            {
                _closeDelegateMethod = value;
                OnPropertyChanged(nameof(CloseDelegateMethod));
            }
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }
        public void PopUp(string message)
        {
            CloseDelegateMethod = Close;
            ActionDelegateMethod = null;
            Message = message;
            IsVisible = true;
        }
        public void PopUp(string message, ActionDelegate closeDelegateMethod)
        {
            CloseDelegateMethod = closeDelegateMethod ?? Close;
            ActionDelegateMethod = null;
            Message = message;
            IsVisible = true;
        }
        public void PopUp(string message, ActionDelegate closeDelegateMethod, ActionDelegate actionDelegateMethod)
        {
            CloseDelegateMethod = closeDelegateMethod ?? Close;
            ActionDelegateMethod = actionDelegateMethod ?? null;
            Message = message;
            IsVisible = true;
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
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
            DefaultDelegates();
            IsVisible = false;
        }

        public virtual void Action(object x) { }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(x => { CloseDelegateMethod(x); })); }
        }

        public ICommand ActionCommand
        {
            get { return _actionCommand ?? (_actionCommand = new RelayCommand(x => { ActionDelegateMethod(x); })); }
        }
    }
}
