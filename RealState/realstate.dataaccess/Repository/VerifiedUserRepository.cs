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
  public  class VerifiedUserRepository:Repository<VerifiedUser>, IVerifiedUserRepository
    {
        private readonly ApplicationDbContext _db;
        public VerifiedUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db ;
        }

        public async Task Update(VerifiedUser entity)
        {
            var entityFromDb=_db.VerifiedUserTBL.FirstOrDefault(x => x.VerifiedUserID == entity.VerifiedUserID);
            if (entityFromDb != null)
            {
                entityFromDb.IsVerified = entity.IsVerified;
                entityFromDb.SecurityLevel = entity.SecurityLevel;
               
                await _db.SaveChangesAsync();
            }
           
        }
    }
}
