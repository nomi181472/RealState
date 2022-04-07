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
  public  class PhotoRepository:Repository<Photo>,IPhotoRepository
    {
        private readonly ApplicationDbContext _db;
        public PhotoRepository(ApplicationDbContext db):base(db)
        {
            _db = db ;
        }

        public async Task Update(Photo entity)
        {
            var entityFromDb=_db.PhotoTBL.FirstOrDefault(x => x.PhotoId == entity.PhotoId);
            if (entityFromDb != null)
            {
                entityFromDb.IsActive = entity.IsActive;
                //TODO: update more field in photo
                await _db.SaveChangesAsync();
            }
           
        }
    }
}
