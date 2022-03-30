using realstate.models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository.IRepository
{
    public interface IUserRepository:IRepository<User>
    {
        void update(User user);
        
    }
}
