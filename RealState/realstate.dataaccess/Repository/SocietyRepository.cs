using realstate.dataaccess.Data;
using realstate.dataaccess.Repository.IRepository;
using realstate.models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository
{
  public  class SocietyRepository : Repository<Society>, ISocietyRepository
    {
        private readonly ApplicationDbContext _db;
        public SocietyRepository(ApplicationDbContext db):base(db)
        {
            _db = db ;
        }

        public async Task Update(Society entity)
        {
            var entityFromDb=_db.SocietyTBL.FirstOrDefault(x => x.SocietyId == entity.SocietyId);
            if (entityFromDb != null)
            {
                entityFromDb.Name = entity.Name;
                entityFromDb.LocationId = entity.LocationId;
                await _db.SaveChangesAsync();
            }
           
        }
    }
}
