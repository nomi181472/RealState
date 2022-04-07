﻿using realstate.models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.dataaccess.Repository.IRepository
{
    public interface IPhotoRepository:IRepository<Photo>
    {
        Task Update(Photo entity);
       
    }
}
