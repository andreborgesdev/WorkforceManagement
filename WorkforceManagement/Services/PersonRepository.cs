using System;
using System.Collections.Generic;
using System.Linq;
using WorkforceManagement.DataAccess;
using WorkforceManagement.Entities;
using WorkforceManagement.ResourceParameters;

namespace WorkforceManagement.Services
{
    public class PersonRepository : IPersonRepository, IDisposable
    {
        private readonly PeopleContext _context; 

        public PersonRepository(PeopleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddPerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            // the repository fills the id (instead of using identity columns)
            person.Id = Guid.NewGuid();

            foreach (var material in person.Materials)
            {
                material.Id = Guid.NewGuid();
            }

            _context.People.Add(person);
        }

        public bool PersonExists(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            return _context.People.Any(a => a.Id == personId);
        }

        public void DeletePerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            _context.People.Remove(person);
        }

        public Person GetPerson(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            return _context.People.FirstOrDefault(a => a.Id == personId);
        }

        public IEnumerable<Person> GetPersons()
        {
            return _context.People.ToList<Person>();
        }

        public IEnumerable<Person> GetPersons(IEnumerable<Guid> personIds)
        {
            if (personIds == null)
            {
                throw new ArgumentNullException(nameof(personIds));
            }

            return _context.People.Where(a => personIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public IEnumerable<Person> GetPersons(PersonsResourceParameters personsResourceParameters)
        {
            if (personsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(personsResourceParameters));
            }

            if (string.IsNullOrWhiteSpace(personsResourceParameters.Gender) && string.IsNullOrWhiteSpace(personsResourceParameters.SearchQuery))
            {
                return GetPersons();
            }

            var collection = _context.People as IQueryable<Person>;

            if (!string.IsNullOrWhiteSpace(personsResourceParameters.Gender))
            {
                var gender = personsResourceParameters.Gender.Trim();
                collection = collection.Where(a => a.Gender == gender);
            }

            if (!string.IsNullOrWhiteSpace(personsResourceParameters.SearchQuery))
            {
                var searchQuery = personsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Gender.Contains(searchQuery)
                || a.Email.Contains(searchQuery)
                || a.FirstName.Contains(searchQuery)
                || a.LastName.Contains(searchQuery)
                || a.Address.Contains(searchQuery));
            }

            return collection.ToList();
        }

        public void UpdatePerson(Person person)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
