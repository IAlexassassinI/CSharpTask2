using CSharpTask2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpTask1.Models
{
    public class Person
    {

        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate;

        public string Name => _name;
        public string Surname => _surname;
        public string Email => _email;
        public DateTime BirthDate => _birthDate;
        private const int MAX_AGE = 135;

        public Person(string name, string surname, string email, DateTime birthDate)
        {
            if (birthDate > DateTime.Now){
                throw new NotBornException();
            }

            if (birthDate < DateTime.Now.AddYears(-MAX_AGE))
            {
                throw new TooOldException();
            }

            if (!IsValidEmail(email)) { 
                throw new InvalidEmailFormatException();
            }

            _name = name;
            _surname = surname;
            _email = email;
            _birthDate = birthDate;
        }

        public Person(string name, string surname, string email)
            : this(name, surname, email, DateTime.MinValue) { }

        public Person(string name, string surname, DateTime birthDate)
            : this(name, surname, string.Empty, birthDate) { }


        private bool _isAdult;
        private string _sunSign;
        private string _chineseSign;
        private bool _isBirthday;

        public bool IsAdult => _isAdult;
        public string SunSign => _sunSign;
        public string ChineseSign => _chineseSign;
        public bool IsBirthday => _isBirthday;

        private const int ADULT_AGE = 18;

        public async Task CalculatePropertiesAsync()
        {
            //here more async code
            var yearsOldTask = DateCalculator.CalculateAgeAsync(_birthDate);
            var sunSignTask = DateCalculator.GetWesternZodiacAsync(_birthDate);
            var chineseSignTask = DateCalculator.GetChineseZodiacAsync(_birthDate);
            var isBirthdayTask = DateCalculator.IsTodayBirthdayAsync(_birthDate);

            _isAdult = await yearsOldTask >= ADULT_AGE;
            _sunSign = await sunSignTask;
            _chineseSign = await chineseSignTask;
            _isBirthday = await isBirthdayTask;
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public override string ToString()
        {
            return $"Name: {Name}\n" +
                   $"Surname: {Surname}\n" +
                   $"Email: {Email}\n" +
                   $"Birthdate: {BirthDate.ToShortDateString()}\n" +
                   $"Is adult: {IsAdult}\n" +
                   $"Sun sign: {SunSign}\n" +
                   $"Chinese zodiac sign: {ChineseSign}\n" +
                   $"Is today birthday: {IsBirthday}";
        }

    }
}
