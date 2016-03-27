using BLL.Interface.Entities;
using BLL.Interface.Services;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalMaterial> materialRepository;

        public MaterialService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.materialRepository = uow.MaterialRepository;
        }

        public IEnumerable<MaterialEntity> GetAllMaterial()
        {
            return materialRepository.GetAll().Select(ent => ent.ToBllMaterial());
        }

        public IEnumerable<MaterialEntity> GetAllMaterial(int start)
        {
            return materialRepository.GetBetween(start, 10).Select(ent => ent.ToBllMaterial());
        }

        public IEnumerable<MaterialEntity> GetAllMaterial(string search)
        {
            return materialRepository.GetByPredicate(ent => ent.FileName.ToLower().Contains(search.ToLower()) 
                || (!String.IsNullOrEmpty(ent.Description) && ent.Description.ToLower().Contains(search.ToLower()))).Select(ent => ent.ToBllMaterial());
        }

        public MaterialEntity GetMaterialById(int Id)
        {
            return materialRepository.GetById(Id).ToBllMaterial();
        }

        public void CreateMaterial(MaterialEntity material)
        {
            materialRepository.Create(material.ToDalMaterial());
            uow.Commit();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
