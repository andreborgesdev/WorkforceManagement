using System;
using System.Collections.Generic;
using WorkforceManagement.Models;

namespace WorkforceManagement.Services
{
    public interface IWorkforceManagementRepository
    {
        IEnumerable<Material> GetMaterials(Guid personId);
        Material GetMaterial(Guid materialId);
        void AddMaterial(Material material);
        void UpdateMaterial(Material materal);
        void DeleteMaterial(Material material);
        IEnumerable<Person> GetPeople();
        IEnumerable<Person> GetPeople(IEnumerable<Guid> personIds);
        Person GetPerson(Guid personId);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void DeletePerson(Person person);
        bool PersonExists(Guid personId);
        bool Save();
    }
}
