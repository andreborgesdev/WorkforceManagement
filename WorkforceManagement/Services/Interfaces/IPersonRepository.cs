using System;
using System.Collections.Generic;
using WorkforceManagement.Entities;
using WorkforceManagement.Helpers;
using WorkforceManagement.ResourceParameters;

namespace WorkforceManagement.Services
{
    public interface IPersonRepository
    {
        //IEnumerable<Person> GetPersons();
        PagedList<Person> GetPersons(PersonsResourceParameters personsResourceParameters);
        IEnumerable<Person> GetPersons(IEnumerable<Guid> personIds);
        Person GetPerson(Guid personId);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void DeletePerson(Person person);
        bool PersonExists(Guid personId);
        bool Save();
    }
}
