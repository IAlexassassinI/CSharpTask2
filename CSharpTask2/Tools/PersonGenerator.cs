using CSharpTask1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpTask2.Tools
{
    public static class PersonGenerator
    {
        private static Random _random = new Random();

        public static async Task<List<Person>> GeneratePersons(int count)
        {
            List<Person> result = new List<Person>();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < count; i++)
            {
                string name = $"Name{i}";
                string surname = $"Surname{i}";
                string email = $"user{i}@example.com";
                DateTime birthDate = DateTime.Today.AddYears(-_random.Next(0, 100)).AddDays(-_random.Next(0, 365));
                Person person = new Person(name, surname, email, birthDate);
                result.Add(person);
                tasks.Add(person.CalculatePropertiesAsync());
            }

            MessageBox.Show("Program is calculating properties for Persons (from async tast part)", "Please wait", MessageBoxButton.OK, MessageBoxImage.Information);
            await Task.WhenAll(tasks);
            return result;
        }
    }
}
