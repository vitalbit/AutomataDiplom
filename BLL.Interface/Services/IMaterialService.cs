using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface IMaterialService : IDisposable
    {
        IEnumerable<MaterialEntity> GetAllMaterial();
        IEnumerable<MaterialEntity> GetAllMaterial(int start);
        IEnumerable<MaterialEntity> GetAllMaterial(string search);
        MaterialEntity GetMaterialById(int Id);
        void CreateMaterial(MaterialEntity material);
    }
}
