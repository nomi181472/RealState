using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using realstate.dataaccess.Repository.IRepository;
using realstate.models.ViewModels;
using realstate.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = SD.AdminUser   + "," + SD.VerifiedUser)]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles =SD.AdminUser)]
        public IActionResult Upsert(int? id)
        {
            Location entity = new Location();
            if (id == null) {
                return View(entity);
            }
            entity = _unitOfWork.locationRepoAccess.Get(id.GetValueOrDefault());
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Location location)
        {
            if (ModelState.IsValid)
            {
                if (location.LocationId == 0)
                {
                    _unitOfWork.locationRepoAccess.Add(location);
                    await _unitOfWork.Save();
                }
                else
                {
                   await _unitOfWork.locationRepoAccess.Update(location);//already saving inside
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        #region API CALLS
            
        [HttpGet]
        public IActionResult GetAll()
        {
            var entitiesFromDB = _unitOfWork.locationRepoAccess.GetAll();
            return Json(new { data= entitiesFromDB });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var entityFromDB = _unitOfWork.locationRepoAccess.Get(id);
            if (entityFromDB==null)
            {
                return Json(new { Success = false, message = "Error while deleting" });
            }
            _unitOfWork.locationRepoAccess.Remove(entityFromDB);
            await _unitOfWork.Save();
            return Json(new { success=true, message = "Delete Successful" });
        }
        #endregion
    }
}
