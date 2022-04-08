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
  public  class PlotRepository:Repository<Plot>,IPlotRepository
    {
        private readonly ApplicationDbContext _db;
        public PlotRepository(ApplicationDbContext db):base(db)
        {
            _db = db ;
        }

        public async Task Update(Plot entity)
        {
            var entityFromDb=_db.PlotTBL.FirstOrDefault(x => x.PlotId == entity.PlotId);
            if (entityFromDb != null)
            {
                //entityFromDb.Area = entity.Area;
                entityFromDb.Description = entity.Description;
                //TODO:many things can be modified
                await _db.SaveChangesAsync();
            }
           
        }
    }
}
