using realstate.dataaccess.Data;
using realstate.dataaccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
       public IUserRepository user { get; private set; }
        public ILocationRepository location { get; private set; }
        public ISP_Call sP_Call { get; private set; }

  
         
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            user = new UserRepository(_db);
            sP_Call = new SP_CALL(_db);
            location = new LocationRepository(_db);
        }
         public async void save()
        {
           await _db.SaveChangesAsync();


        }

        public async void Dispose()
        {
            await _db.DisposeAsync();
        }
    }
}
