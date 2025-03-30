using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public Person(string name, string surname, string email, DateTime birthDate)
        {
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

        private int _yearsOld;

        public bool IsAdult => _isAdult;
        public string SunSign => _sunSign;
        public string ChineseSign => _chineseSign;
        public bool IsBirthday => _isBirthday;

        private const int ADULT_AGE = 18;
        public async Task CalculatePropertiesAsync()
        {
            await Task.Delay(5000);
            _yearsOld = DateCalculator.CalculateAge(_birthDate);
            _isAdult = _yearsOld >= ADULT_AGE; 
            _sunSign = DateCalculator.GetWesternZodiac(_birthDate);
            _chineseSign = DateCalculator.GetChineseZodiac(_birthDate);
            _isBirthday = DateCalculator.IsTodayBirthday(_birthDate);
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
