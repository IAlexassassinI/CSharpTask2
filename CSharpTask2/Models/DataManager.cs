using CSharpTask1.Models;
using CSharpTask2.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CSharpTask1.Models.Person;

namespace CSharpTask2.Models
{
    public class DataManager
    {
        private const string _personsFilePath = "users.json";
        private const string _stateFilePath = "state.json";

        private static DataManager _instance;

        public static DataManager Instance => _instance ??= new DataManager();

        private ObservableCollection<Person> _persons = new ObservableCollection<Person>();
        public ObservableCollection<Person> Persons
        {
            get => _persons;
            set => _persons = value;
        }

        private States _states = new States();
        public States States
        {
            get => _states;
            set => _states = value;
        }

        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions();

        private DataManager() 
        {
            _jsonOptions.Converters.Add(new PersonConverter());

            LoadStates();
            if (_states.isFirstBoot)
            {
                _states.isFirstBoot = false;
                Task.Run(async () =>
                {
                    AddPersons(await PersonGenerator.GeneratePersons(50));
                });
            }
            else
            {
                LoadPersonsProceed();
            }
        }

        public void SaveAll()
        {
            SavePersons();
            SaveStates();
        }

        public void AddPerson(Person person) => _persons.Add(person);

        public void AddPersons(IList<Person> persons)
        {
            foreach (var person in persons)
            {
                AddPerson(person);
            }
        }

        public void RemovePerson(Person person) => _persons.Remove(person);

        public void EditPerson(Person oldPerson, Person newPerson)
        {
            var index = _persons.IndexOf(oldPerson);
            if (index != -1)
            {
                _persons[index] = newPerson;
            }
        }

        public void SavePersons()
        {
            var json = JsonSerializer.Serialize(_persons, _jsonOptions);
            File.WriteAllText(_personsFilePath, json);
        }

        private ObservableCollection<Person> LoadPersons()
        {
            if (File.Exists(_personsFilePath))
            {
                var json = File.ReadAllText(_personsFilePath);
                return JsonSerializer.Deserialize<ObservableCollection<Person>>(json, _jsonOptions);
            }
            return new ObservableCollection<Person>();
        }

        public void LoadPersonsProceed()
        {
            var persons = LoadPersons();
            AddPersons(persons);
        }

        public void SaveStates()
        {
            var json = JsonSerializer.Serialize(_states);
            File.WriteAllText(_stateFilePath, json);
        }

        private void LoadStates()
        {
            if (File.Exists(_stateFilePath))
            {
                var json = File.ReadAllText(_stateFilePath);
                var states = JsonSerializer.Deserialize<States>(json);
                if (states != null)
                {
                    _states = states;
                }
            }
        }
    }
}
