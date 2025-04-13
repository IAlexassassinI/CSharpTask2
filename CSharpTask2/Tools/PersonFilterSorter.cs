using CSharpTask1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTask2.Tools
{
    public class PersonFilterSorter
    {
        private string _filterProperty;
        private string _filterValue;
        private string _sortProperty;
        private bool _sortDescending;

        public PersonFilterSorter(string filterProperty = "", string filterValue = "", string sortProperty = "", bool sortDescending = false)
        {
            _filterProperty = filterProperty;
            _filterValue = filterValue;
            _sortProperty = sortProperty;
            _sortDescending = sortDescending;
        }

        public void SetFilter(string property, string value)
        {
            _filterProperty = property;
            _filterValue = value;
        }

        public void SetSort(string property, bool descending)
        {
            _sortProperty = property;
            _sortDescending = descending;
        }

        public ObservableCollection<Person> Apply(ObservableCollection<Person> people)
        {
            var query = people.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_filterProperty) && !string.IsNullOrWhiteSpace(_filterValue))
            {
                var propInfo = typeof(Person).GetProperty(_filterProperty);
                if (propInfo != null)
                {
                    query = query.Where(p =>
                        propInfo.GetValue(p)!.ToString()!.Contains(_filterValue, StringComparison.Ordinal)
                    );
                }
            }

            if (!string.IsNullOrWhiteSpace(_sortProperty))
            {
                var propInfo = typeof(Person).GetProperty(_sortProperty);
                if (propInfo != null)
                {
                    query = _sortDescending
                        ? query.OrderByDescending(p => propInfo.GetValue(p))
                        : query.OrderBy(p => propInfo.GetValue(p));
                }
            }

            return new ObservableCollection<Person>(query.ToList());
        }
    }
}
