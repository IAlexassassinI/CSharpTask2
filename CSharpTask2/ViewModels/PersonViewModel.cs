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
using CSharpTask2.Exceptions;

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
        public RelayCommand CancelCommand { get; }
        public RelayCommand DebugCommand { get; }

        private Window? _linked_window = null;

        public PersonViewModel(Person? person = null, Window? window = null) //need to input window to make it dialog 
        {
            _linked_window = window;
            
            IsProcessing = false;
            ProceedCommand = new RelayCommand(async () => await ProcessData(), ValidateFields);
            CancelCommand = new RelayCommand(DoCancel, () => true);
            DebugCommand = new RelayCommand(DebugTest, () => true);

            if (person != null)
            {
                Name = person.Name;
                Surname = person.Surname;
                Email = person.Email;
                BirthDate = person.BirthDate;
            }
        }

        public Person PersonData => _personData;
        public bool HasValidPerson => _personData != null;

        private void DoCancel()
        {
            if (_linked_window != null)
            {
                _linked_window.DialogResult = false;
                _linked_window.Close();
            }
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

        
        private Person? _personData = null;

        private async Task ProcessData()
        {
            
            IsProcessing = true;

            try
            {
                Person tmpPerson = new Person(Name, Surname, Email, BirthDate);

                await tmpPerson.CalculatePropertiesAsync();

                _personData = tmpPerson;
                ResultText = _personData.ToString();

                if (_personData.IsBirthday)
                {
                    MessageBox.Show("Happy birthday!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (_linked_window != null)
                {
                    _linked_window.DialogResult = true;
                    _linked_window.Close();
                }
            }
            catch (NotBornException ex)
            {
                MessageBox.Show("You are not born yet", "Date entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TooOldException ex)
            {
                MessageBox.Show("You are too old for this program :(", "Date entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidEmailFormatException ex)
            {
                MessageBox.Show(ex.Message, "Date entered incorrectly", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsProcessing = false;
            }
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
