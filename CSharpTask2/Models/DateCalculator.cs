using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTask1.Models
{
    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public class DateCalculator
    {
        public static int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Now.AddYears(-age)) age--;
            return age;
        }

        public static string GetWesternZodiac(DateTime birthDate)
        {
            int day = birthDate.Day;
            Month month = ((Month)birthDate.Month);

            switch (month)
            {
                case Month.January:
                    return (day <= 19) ? "Capricorn" : "Aquarius";
                case Month.February:
                    return (day <= 18) ? "Aquarius" : "Pisces";
                case Month.March:
                    return (day <= 20) ? "Pisces" : "Aries";
                case Month.April:
                    return (day <= 19) ? "Aries" : "Taurus";
                case Month.May:
                    return (day <= 20) ? "Taurus" : "Gemini";
                case Month.June:
                    return (day <= 20) ? "Gemini" : "Cancer";
                case Month.July:
                    return (day <= 22) ? "Cancer" : "Leo";
                case Month.August:
                    return (day <= 22) ? "Leo" : "Virgo";
                case Month.September:
                    return (day <= 22) ? "Virgo" : "Libra";
                case Month.October:
                    return (day <= 22) ? "Libra" : "Scorpio";
                case Month.November:
                    return (day <= 21) ? "Scorpio" : "Sagittarius";
                case Month.December:
                    return (day <= 21) ? "Sagittarius" : "Capricorn";
                default:
                    return "Unknown";
            }
        }

        static private readonly string[] _animals = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
        static private readonly string[] _elements = { "Wood", "Fire", "Earth", "Metal", "Water" };
        static private readonly string[] _harm = { "yang", "yin"};

        public static string GetChineseZodiac(DateTime birthDate)
        {
            int animal_index = (birthDate.Year - 4) % 12;
            int element_index = (int)Math.Floor((birthDate.Year - 4.0) % 10 / 2); ;
            int harm_index = (birthDate.Year) % 2;

            return $"{_elements[element_index]} {_animals[animal_index]} ({_harm[harm_index]})";
        }

        public static bool IsTodayBirthday(DateTime BirthDate) 
        {
            return BirthDate.Day == DateTime.Now.Day && BirthDate.Month == DateTime.Now.Month;
        }
    }
}
