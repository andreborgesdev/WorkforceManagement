using System;
using System.Collections.Generic;
using System.Linq;
using WorkforceManagement.DataAccess;
using WorkforceManagement.Models;

namespace WorkforceManagement.Services
{
    public class WorkforceManagementRepository : IWorkforceManagementRepository, IDisposable
    {
        private readonly PeopleContext _context; 

        public WorkforceManagementRepository(PeopleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddMaterial(Material material)
        {
            if (material == null)
            {
                throw new ArgumentNullException(nameof(material));
            }

            _context.Materials.Add(material);
        }

        public void DeleteMaterial(Material material)
        {
            _context.Materials.Remove(material);
        }

        public Material GetMaterial(Guid materialId)
        {
            if (materialId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(materialId));
            }

            return _context.Materials
              .Where(c => c.Id == materialId).FirstOrDefault();
        }

        public Material GetMaterial(Guid personId, Guid materialId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            if (materialId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(materialId));
            }

            return _context.Materials
              .Where(c => c.PersonId == personId && c.Id == materialId).FirstOrDefault();
        }

        public IEnumerable<Material> GetMaterials(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            return _context.Materials
                        .Where(c => c.PersonId == personId)
                        .OrderBy(c => c.Reference).ToList();
        }

        public void UpdateMaterial(Material material)
        {
            // no code in this implementation
        }

        public void AddPerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            // the repository fills the id (instead of using identity columns)
            person.Id = Guid.NewGuid();

            //foreach (var course in author.Courses)
            //{
            //    course.Id = Guid.NewGuid();
            //}

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

        public IEnumerable<Person> GetPeople()
        {
            return _context.People.ToList<Person>();
        }

        public IEnumerable<Person> GetPeople(IEnumerable<Guid> personIds)
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
