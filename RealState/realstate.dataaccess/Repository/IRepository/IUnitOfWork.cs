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
        Task Save();
        ISP_Call sP_Call { get; }
    }
}
