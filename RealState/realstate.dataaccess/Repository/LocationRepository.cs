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
  public  class LocationRepository:Repository<Location>,ILocationRepository
    {
        private readonly ApplicationDbContext _db;
        public LocationRepository(ApplicationDbContext db):base(db)
        {
            _db = db ;
        }

        public async void update(Location entity)
        {
            var entityFromDb=_db.LocationTBL.FirstOrDefault(x => x.LocationId == entity.LocationId);
            if (entityFromDb != null)
            {
                entityFromDb.City = entity.City;
                entityFromDb.Province = entity.Province;
                entityFromDb.District = entity.District;
                entityFromDb.Country = entity.Country;
                await _db.SaveChangesAsync();
            }
           
        }
    }
}
