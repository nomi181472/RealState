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
    [Authorize(Roles = SD.AdminUser)] //Update(remove roles )
    public class SocietyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SocietyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            SocietyVM entity = new SocietyVM() { society=new Society(), allLocations =_unitOfWork.locationRepoAccess.GetAll()
                .Select(x=>new SelectListItem() {Text=x.City,Value=x.LocationId.ToString()})};
            if (id == null) {
                return View(entity);
            }
            entity.society = _unitOfWork.societyRepoAccess.Get(id.GetValueOrDefault());
            if (entity.society  == null)
            {
                return NotFound();
            }
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SocietyVM societyVM)
        {
            if (ModelState.IsValid)
            {
                if (societyVM?.society.SocietyId == 0)
                {
                    _unitOfWork.societyRepoAccess.Add(societyVM.society);
                    await _unitOfWork.Save();
                }
                else
                {
                   await _unitOfWork.societyRepoAccess.Update(societyVM.society);//already saving inside
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(societyVM);
        }

        #region API CALLS
            
        [HttpGet]
        public IActionResult GetAll()
        {
            var entitiesFromDB = _unitOfWork.societyRepoAccess.GetAll(includeProperties:$"LocationTBL");
            return Json(new { data= entitiesFromDB });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var entityFromDB = _unitOfWork.societyRepoAccess.Get(id);
            if (entityFromDB==null)
            {
                return Json(new { Success = false, message = "Error while deleting" });
            }
            _unitOfWork.societyRepoAccess.Remove(entityFromDB);
            await _unitOfWork.Save();
            return Json(new { success=true, message = "Delete Successful" });
        }
        #endregion
    }
}
