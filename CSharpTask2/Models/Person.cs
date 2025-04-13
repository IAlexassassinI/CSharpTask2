using CSharpTask2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
            }
        }
        private const int MAX_AGE = 135;

        private Person() { }

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

        public bool IsAdult
        {
            get => _isAdult;
            private set { 
                _isAdult = value;
            }
        }
        public string SunSign
        {
            get => _sunSign;
            private set { 
                _sunSign = value; 
            }
        }
        public string ChineseSign
        {
            get => _chineseSign;
            private set { 
                _chineseSign = value;
            }
        }
        public bool IsBirthday
        {
            get => _isBirthday;
            private set { 
                _isBirthday = value; 
            }
        }

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


        public class PersonConverter : JsonConverter<Person>
        {
            public override Person Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {                
                var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
                var person = new Person();
                
                person.Name = jsonObject.GetProperty("Name").GetString();
                person.Surname = jsonObject.GetProperty("Surname").GetString();
                person.Email = jsonObject.GetProperty("Email").GetString();
                person.BirthDate = jsonObject.GetProperty("BirthDate").GetDateTime();

                // i decided to serialize and deserialize all fields
                person.IsAdult = jsonObject.GetProperty("IsAdult").GetBoolean();
                person.SunSign = jsonObject.GetProperty("SunSign").GetString();
                person.ChineseSign = jsonObject.GetProperty("ChineseSign").GetString();
                person.IsBirthday = jsonObject.GetProperty("IsBirthday").GetBoolean();

                return person;
            }

            public override void Write(Utf8JsonWriter writer, Person value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteString("Name", value.Name);
                writer.WriteString("Surname", value.Surname);
                writer.WriteString("Email", value.Email);
                writer.WriteString("BirthDate", value.BirthDate);

                // i decided to serialize and deserialize all fields
                writer.WriteBoolean("IsAdult", value.IsAdult);
                writer.WriteString("SunSign", value.SunSign);
                writer.WriteString("ChineseSign", value.ChineseSign);
                writer.WriteBoolean("IsBirthday", value.IsBirthday);

                writer.WriteEndObject();
            }
        }


    }
}
