using CSharpTask1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace CSharpTask1.ViewModels
{
    class PersonViewModel: INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate = DateTime.Today;
        private bool _isProcessing;
        private string _resultText;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); UpdateCanExecute(); }
        }

        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); UpdateCanExecute(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); UpdateCanExecute(); }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(); UpdateCanExecute(); }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set { _isProcessing = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotProcessing)); OnPropertyChanged(nameof(IsProcessingVisibility)); }
        }

        public Visibility IsProcessingVisibility
        {
            get { return _isProcessing ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool IsNotProcessing => !_isProcessing;

        public string ResultText
        {
            get => _resultText;
            set { _resultText = value; OnPropertyChanged(); }
        }

        public RelayCommand ProceedCommand { get; }
        public RelayCommand DebugCommand { get; }

        public PersonViewModel()
        {
            IsProcessing = false;
            ProceedCommand = new RelayCommand(async () => await ProcessData(), ValidateFields);
            DebugCommand = new RelayCommand(DebugTest, () => true);
        }

        private void DebugTest() 
        {
            MessageBox.Show("Test Still Usable UI", "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool ValidateFields() 
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Surname) &&
                   !string.IsNullOrWhiteSpace(Email);
                                                       
        }

        private const int MAX_AGE = 135;

        private bool CheckAge(int age)
        {
            if (age < 0)
            {
                MessageBox.Show("You are not born yet", "Date entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (age > MAX_AGE)
            {
                MessageBox.Show("You are too old for this program :(", "Date entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        Person _personData;

        private async Task ProcessData()
        {
            
            IsProcessing = true;

            if (!CheckAge(DateCalculator.CalculateAge(BirthDate))) 
            {
                IsProcessing = false;
                return;
            }

            _personData = new Person(Name, Surname, Email, BirthDate);
            await _personData.CalculatePropertiesAsync();
            ResultText = _personData.ToString();

            if (_personData.IsBirthday) 
            {
                MessageBox.Show("Happy birthday!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            IsProcessing = false;
        }

        private void UpdateCanExecute()
        {
            ProceedCommand.NotifyCanExecuteChanged();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
