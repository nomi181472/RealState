using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class VerifiedUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public VerifiedUserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert( int? id)
        {
            VerifiedUser entity = new VerifiedUser();
            if (id == 0) {
                return View(entity);
            }
            //TODO update User
            //entity = _userManager.GetClaimsAsync();
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(VerifiedUser entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.VerifiedUserID == 0)
                {
                    _unitOfWork.verifiedUserRepoAccess.Add(entity);
                    await _unitOfWork.Save();
                }
                else
                {
                   await _unitOfWork.verifiedUserRepoAccess.Update(entity);//already saving inside
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        #region API CALLS
            
        [HttpGet]
        public IActionResult GetAll()
        {

            //TODO get all users 
            List<AllApplicationUser> users = new List<AllApplicationUser>();
            var entitiesFromDB = _unitOfWork.applicationUserRepoAccess.GetAll();
            foreach (var item in entitiesFromDB)
            {
                AllApplicationUser applicationUser = new AllApplicationUser();
                applicationUser.Email = item.Email;
                applicationUser.PhoneNumber = item.PhoneNumber;
                applicationUser.UserName = item.UserName;
                
                applicationUser.IsVerified = _userManager.GetRolesAsync(item).Result.Any(x=>x==SD.VerifiedUser || x==SD.AdminUser);
                users.Add(applicationUser);
            }
            return Json(new { data= users });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var entityFromDB = _unitOfWork.verifiedUserRepoAccess.Get(id);
            if (entityFromDB==null)
            {
                return Json(new { Success = false, message = "Error while deleting" });
            }
            _unitOfWork.verifiedUserRepoAccess.Remove(entityFromDB);
            await _unitOfWork.Save();
            return Json(new { success=true, message = "Delete Successful" });
        }
        #endregion
    }
}
