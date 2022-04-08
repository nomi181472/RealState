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
       public IUserRepository userRepoAccess { get; private set; }
        public ILocationRepository locationRepoAccess { get; private set; }
        public ISP_Call sP_Call { get; private set; }

        public ISocietyRepository societyRepoAccess { get; private set; }
        public IApplicationUserRepository applicationUserRepoAccess { get; private set; }

        public IVerifiedUserRepository verifiedUserRepoAccess { get; private set; }

        public IPlotRepository plotRepoAccess { get; private set; }

        public IPhotoRepository photoRepoAccess { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            userRepoAccess = new UserRepository(_db);
            sP_Call = new SP_CALL(_db);
            locationRepoAccess = new LocationRepository(_db);
            societyRepoAccess = new SocietyRepository(_db);
            verifiedUserRepoAccess = new VerifiedUserRepository(_db);
            applicationUserRepoAccess = new ApplicationUserRepository(_db);
            plotRepoAccess = new PlotRepository(_db);
            photoRepoAccess = new PhotoRepository(_db);
        }
         public  async Task Save()
        {
            await _db.SaveChangesAsync();


        }

        public  void Dispose()
        {
             _db.Dispose();
        }
    }
}
