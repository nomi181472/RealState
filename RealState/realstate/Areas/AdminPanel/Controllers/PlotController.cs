using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using realstate.dataaccess.Repository.IRepository;
using realstate.models.ViewModels;
using realstate.models.ViewModels.VMModels;
using realstate.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PlotController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlotController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddPlotAndPhotos()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            AddPlotAndPhotos addPlotAndPhotos = new AddPlotAndPhotos();
            Plot entity = new Plot();
            Society society = new Society();
            if (id != null) {
                return View(entity);
                entity = _unitOfWork.plotRepoAccess.Get(id.GetValueOrDefault());
                if (entity != null)
                {
                    addPlotAndPhotos.PlotSize = entity.PlotSize;
                    addPlotAndPhotos.CompleteAddress = entity.CompleteAddress;
                    addPlotAndPhotos.Block = entity.Block;
                    addPlotAndPhotos.Description = entity.Description;
                    addPlotAndPhotos.Price = entity.Price;
                    addPlotAndPhotos.Price = entity.Price;
                }
            }


            var allSocities = _unitOfWork.societyRepoAccess.GetAll();
            addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
            

            return View(addPlotAndPhotos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Plot entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.PlotId == 0)
                {
                    _unitOfWork.plotRepoAccess.Add(entity);
                    await _unitOfWork.Save();
                }
                else
                {
                   await _unitOfWork.plotRepoAccess.Update(entity);//already saving inside
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        #region API CALLS
            
        [HttpGet]
        public IActionResult GetAll()
        {
            var entitiesFromDB = _unitOfWork.plotRepoAccess.GetAll();
            return Json(new { data= entitiesFromDB });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var entityFromDB = _unitOfWork.plotRepoAccess.Get(id);
            if (entityFromDB==null)
            {
                return Json(new { Success = false, message = "Error while deleting" });
            }
            _unitOfWork.plotRepoAccess.Remove(entityFromDB);
            await _unitOfWork.Save();
            return Json(new { success=true, message = "Delete Successful" });
        }
        #endregion
    }
}
