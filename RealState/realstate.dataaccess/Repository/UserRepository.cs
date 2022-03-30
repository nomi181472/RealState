using realstate.dataaccess.Data;
using realstate.dataaccess.Repository.IRepository;
using realstate.models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository
{
   public class UserRepository:Repository<User>,IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext  db):base(db)
        {
            _db = db;
        }

        public void update(User user)
        {
            var entityFromDb = _db.UserTBL.FirstOrDefault(x => x.UserGUID == user.UserGUID);
            if (entityFromDb != null)
            {
                //TODO:UserNameUpdate
            }
        }
    }
}
