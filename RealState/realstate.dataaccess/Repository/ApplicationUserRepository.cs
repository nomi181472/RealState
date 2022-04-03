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
  public  class ApplicationUserRepository:Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db ;
        }

        public async Task Update(ApplicationUser entity)
        {
            var entityFromDb=_db.ApplicationUserTBL.FirstOrDefault(x => x.Email == entity.Email);
            if (entityFromDb != null)
            {
            /*TODO update User details*/

                await _db.SaveChangesAsync();
            }
           
        }
    }
}
