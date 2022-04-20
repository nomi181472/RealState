using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using realstate.dataaccess.Repository.IRepository;
using realstate.DTOs;
using realstate.models.ViewModels;
using realstate.models.ViewModels.VMModels;
using realstate.utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = SD.AdminUser)] //Update(remove roles )
    public class SocietyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public SocietyController(IUnitOfWork unitOfWork,  IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
             _configuration= configuration;
    }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            SocietyVM entity = new SocietyVM() { society=new Society(), allLocations =_unitOfWork.locationRepoAccess.GetAll()
                .Select(x=>new SelectListItem() {Text=x.City,Value=x.LocationId.ToString()})};
            entity.society.Map = "empty";
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
        private List<string> CopyImages(List<IFormFile> postedFiles)
        {
            string imagesPath = Path.Combine(Environment.CurrentDirectory, "Images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
            List<string> actualPaths = new List<string>();

            foreach (var postedFile in postedFiles)
            {
                var actualPath = Path.Combine(imagesPath, Guid.NewGuid().ToString() + postedFile.FileName);
                using (FileStream stream = new FileStream(actualPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    actualPaths.Add(actualPath);

                }

            }
            return actualPaths;
        }
        private async Task<string> AddPhotosInDatabase(List<string> urls)
        {
            var config = _configuration.GetSection("CloudinaryConfig").Get<CloudinaryConfigDTO>();
            PhotosUploder photosUploder = new PhotosUploder(config.CloudName, config.ApiKey, config.ApiSecret);
            var uploadedPhotos = await photosUploder.MultipleUploadPhotos(urls);
            string url = null;
            foreach (var photo in uploadedPhotos)
            {
                Photo photoObj = new Photo();
                photoObj.IsActive = true;
                photoObj.PublicURL = photo.SecureUrl.AbsoluteUri;
                url= photo.SecureUrl.AbsoluteUri;
//photoObj.PlotId = plotId;
              //  _unitOfWork.photoRepoAccess.Add(photoObj);



            }
           // await _unitOfWork.Save();
            return url;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SocietyVM societyVM, List<IFormFile> postedFiles )
        {

            if (ModelState.IsValid)
            {
                if (societyVM?.society.SocietyId == 0)
                {

                    if (postedFiles.Count > 0)
                    {
                        var urls = CopyImages(postedFiles);
                       var url= await AddPhotosInDatabase(urls);
                        societyVM.society.Map = url;
                    }
                   
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
