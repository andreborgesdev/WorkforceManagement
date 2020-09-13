using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.DataAccess;
using WorkforceManagement.Entities;

namespace WorkforceManagement.Services
{
    public class MaterialRepository : IMaterialRepository, IDisposable
    {
        private readonly PeopleContext _context;

        public MaterialRepository(PeopleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddMaterial(Guid personId, Material material)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            if (material == null)
            {
                throw new ArgumentNullException(nameof(material));
            }

            material.PersonId = personId;

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

        public bool PersonExists(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            return _context.People.Any(a => a.Id == personId);
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
