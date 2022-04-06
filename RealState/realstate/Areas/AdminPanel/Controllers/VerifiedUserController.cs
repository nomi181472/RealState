using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = SD.AdminUser )]
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
            
        [HttpPost]
        public async Task<IActionResult> LockOrUnlock([FromBody] string id)
        {
            var entityFromDb = _unitOfWork.applicationUserRepoAccess.GetFirstOrDefault(x => x.Id == id);
            if (entityFromDb==null)
            {
                return Json(new { success = false, message = "user not found" });
            }
            if(entityFromDb.LockoutEnd!=null && entityFromDb.LockoutEnd > DateTime.Now)
            {
                entityFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                entityFromDb.LockoutEnd = DateTime.Now.AddDays(1000);
              
            }
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Succeed" });
        }
         [HttpPost]
        public async Task<IActionResult> ModifyRole([FromBody] string id)
        {
            var entityFromDb = await _userManager.FindByIdAsync(id);
            
           // var entityFromDb = _unitOfWork.applicationUserRepoAccess.GetFirstOrDefault(x => x.Id == id);
            if (entityFromDb==null)
            {
                return Json(new { success = false, message = "user not found" });
            }

            var role=await _userManager.GetRolesAsync(entityFromDb);
            if (role.Count > 0)
            {
                if(role.Contains(SD.AdminUser))
                {
                    return Json(new { success = false, message = "Admin role cannot be change" });
                }
                else if (role.Contains(SD.VerifiedUser))
                {
                    await _userManager.RemoveFromRoleAsync(entityFromDb, SD.VerifiedUser);
                   
                    
                }
                else
                {
                    await _userManager.AddToRoleAsync(entityFromDb, SD.VerifiedUser);
                }
            }
            else
            {
                await _userManager.AddToRoleAsync(entityFromDb, SD.VerifiedUser);

                await _userManager.AddToRoleAsync(entityFromDb, SD.RegisteredUser);
            }

            
            //await _unitOfWork.Save();
            return Json(new { success = true, message = "Succeed" });
        }
    
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
                applicationUser.LockoutEnd = item.LockoutEnd;
                applicationUser.Id = item.Id;
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
