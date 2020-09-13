using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Entities;

namespace WorkforceManagement.Services
{
    public interface IMaterialRepository
    {
        IEnumerable<Material> GetMaterials(Guid personId);
        Material GetMaterial(Guid personId, Guid materialId);
        void AddMaterial(Guid personId, Material material);
        void UpdateMaterial(Material materal);
        void DeleteMaterial(Material material);
        bool PersonExists(Guid personId);
        bool Save();
    }
}
