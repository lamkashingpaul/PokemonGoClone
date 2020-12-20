using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class SaveViewModel : ViewModelBase
    {
        // Delegate to prompt uesr to confirm overwrite
        private void ConfirmOverwrite(object x)
        {
            WriteToFile(FileName);
        }

        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;

        private string _fileName;
        private string _defaultFileName;
        public ObservableCollection<string> Saves { get; set; }

        public SaveViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Saves = MainWindowViewModel.Saves;
            DefaultFileName = "";
            Update();
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
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            set
            {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        public string DefaultFileName
        {
            get { return _defaultFileName; }
            set
            {
                _defaultFileName = value;
                OnPropertyChanged(nameof(DefaultFileName));
            }
        }
        public void Update()
        {
            Saves.Clear();
            foreach (var save in Directory.EnumerateFiles(@".\", "*.pkmgc", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).ToList())
            {
                Saves.Add(save);
            }
        }
        private ICommand _updateFileNameCommand;
        private ICommand _saveCommand;
        private ICommand _refreshCommand;
        public ICommand UpdateFileNameCommand
        {
            get { return _updateFileNameCommand ?? (_updateFileNameCommand = new RelayCommand(x => { UpdateFileName(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(x => { Save(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new RelayCommand(x => { Refresh(x); }, x => !DialogViewModel.IsVisible)); }
        }
        private void UpdateFileName(object sender)
        {
            var content = sender as string;
            DefaultFileName = content;
        }
        private void Save(object sender)
        {
            Update();
            var textbox = sender as TextBox;
            FileName = textbox.Text;

            if (string.IsNullOrEmpty(FileName))
            {
                DialogViewModel.PopUp("You must have your filename.");
                return;
            }

            FileName = GoodFilename(FileName);

            if (Saves.Where(x => String.Equals(x, FileName) == true).Count() > 0)
            {
                DialogViewModel.PopUp("Filename always exists. Do you want to overwrite?", null, ConfirmOverwrite);
            } else
            {
                WriteToFile(FileName);
            }
            Update();
        }
        private void WriteToFile(string fileName)
        {
            bool success = Serializator.Serialize(fileName, MainWindowViewModel.Trainers);
            string result = success ? "Savad." : "Save failed.";
            DefaultFileName = fileName;
            DialogViewModel.PopUp(result);
        }
        private void Refresh(object x)
        {
            Update();
        }
        public string GoodFilename(string fileName)
        {
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            if (string.Equals(Path.GetExtension(fileName), ".pkmgc") == false)
            {
                fileName += ".pkmgc";
            }
            return fileName;
        }
    }
}
