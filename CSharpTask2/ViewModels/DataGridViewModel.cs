using CommunityToolkit.Mvvm.Input;
using CSharpTask1.Models;
using CSharpTask1.ViewModels;
using CSharpTask2.Models;
using CSharpTask2.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSharpTask2.ViewModels
{
    public class DataGridViewModel : INotifyPropertyChanged
    {
        private Person _selectedPerson;

        private ObservableCollection<Person> _filteredPersons;
        public ObservableCollection<Person> FilteredPersons
        {
            get => _filteredPersons;
            set
            {
                _filteredPersons = value;
                OnPropertyChanged();
            }
        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
                UpdateCanExecute();
            }
        }

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 


        public string FilterValue
        {
            get => _filterValue;
            set { 
                _filterValue = value; 
                OnPropertyChanged(); 
            }
        }
        private string _filterValue;

        public ObservableCollection<string> SortFilterFields { get; } = new ObservableCollection<string>
        {
            "Name", "Surname", "Email", 
            "SunSign", "ChineseSign" ,
            "IsBirthday", "BirthDate", "IsAdult"
        };

        public string SelectedFilterField
        {
            get => _selectedFilterField;
            set { _selectedFilterField = value; OnPropertyChanged(); }
        }
        private string _selectedFilterField;

        private string _selectedSortField;
        public string SelectedSortField
        {
            get => _selectedSortField;
            set { _selectedSortField = value; OnPropertyChanged(); }
        }

        private bool _sortAscending = true;
        public bool SortAscending
        {
            get => _sortAscending;
            set { _sortAscending = value; OnPropertyChanged(); }
        }


        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ForceSaveCommand { get; }
        public RelayCommand ApplyFilterCommand { get; }

        private PersonFilterSorter _filterSorter;

        public DataGridViewModel()
        {
            _filterSorter = new PersonFilterSorter();
            ApplyFilterCommand = new RelayCommand(SetSortAndFilter);
            SelectedFilterField = SortFilterFields.First();
            SelectedSortField = SortFilterFields.First();

            AddCommand = new RelayCommand(AddPerson);
            EditCommand = new RelayCommand(EditPerson, () => SelectedPerson != null);
            DeleteCommand = new RelayCommand(DeletePerson, () => SelectedPerson != null);
            ForceSaveCommand = new RelayCommand(DataManager.Instance.SavePersons);

            DataManager.Instance.Persons.CollectionChanged += AllPeopleCollectionChanged;
            RefreshFilteredPersons();
        }

        private void UpdateCanExecute()
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }

        public void SetSortAndFilter()
        {
            _filterSorter.SetFilter(SelectedFilterField, FilterValue);
            _filterSorter.SetSort(SelectedSortField, !SortAscending);
            RefreshFilteredPersons();
        }

        public void RefreshFilteredPersons()
        {
            FilteredPersons = _filterSorter.Apply(DataManager.Instance.Persons);
        }

        private void AllPeopleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshFilteredPersons();
        }

        private void AddPerson()
        {
            var window = new PersonDataWindow();
            if (window.ShowDialog() == true)
            {
                DataManager.Instance.AddPerson(((PersonViewModel)window.DataContext).PersonData);
            }
        }

        private void EditPerson()
        {
            if (SelectedPerson == null) return;

            var window = new PersonDataWindow(SelectedPerson);
            if (window.ShowDialog() == true)
            {
                DataManager.Instance.EditPerson(SelectedPerson, ((PersonViewModel)window.DataContext).PersonData);
            }
        }

        private void DeletePerson()
        {
            if (SelectedPerson != null)
            {
                DataManager.Instance.RemovePerson(SelectedPerson);
            }
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
