using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository.IRepository
{
   public  interface IUnitOfWork:IDisposable
    {
        IUserRepository userRepoAccess { get; }
        ILocationRepository locationRepoAccess { get; }
        ISocietyRepository societyRepoAccess { get; }
        IVerifiedUserRepository verifiedUserRepoAccess { get; }
        IPlotRepository plotRepoAccess { get; }
        IApplicationUserRepository applicationUserRepoAccess { get; }
        Task Save();
        ISP_Call sP_Call { get; }
    }
}
