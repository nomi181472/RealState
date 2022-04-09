using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using realstate.DTOs;
using realstate.dataaccess.Data;

namespace realstate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = SD.AdminUser + "," + SD.RegisteredUser + "," + SD.VerifiedUser)]
    public class PlotController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        public PlotController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,IConfiguration configuration,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _userManager= userManager;
            _configuration = configuration;
            _db = db;
        }
        /*
            * (from ep in dbContext.tbl_EntryPoint
                join e in dbContext.tbl_Entry on ep.EID equals e.EID
                join t in dbContext.tbl_Title on e.TID equals t.TID
                where e.OwnerID == user.UID
                select new {
                    UID = e.OwnerID,
                    TID = e.TID,
                    Title = t.Title,
                    EID = e.EID
                })
            */
        /*var data=(from photo in _db.PhotoTBL
        join plot in _db.PlotTBL on photo.PlotId equals plot.PlotId
        join usermodel in _db.ApplicationUserTBL on plot.UserId equals usermodel.Id
        where plot.UserId == userId
        select new PlotCompleteDetails()
        {
            photo = photo,
            user = usermodel,
            plot=plot
        });*/
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id.ToString();
            var Roles = _userManager.GetRolesAsync(user).Result.ToList();
            var entities = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotTBL.UserId == userId, includeProperties: "PlotTBL")
                .Select(x=> {
                    x.PlotTBL.ApplicationUserTBL = new ApplicationUser() { Roles = Roles };
                    x.PlotTBL.SocietyTBL = _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == x.PlotTBL.SocietyId);
                    return x;
                    });

            var groupByPlotId = entities.GroupBy(x => x.PlotId).ToDictionary(x=>x.Key,x=>x.ToList());
           
            return View(groupByPlotId);
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
        private List<string> CopyImages(List<IFormFile> postedFiles,string userId)
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
        private async Task AddPhotosInDatabase(List<string> urls,int plotId)
        {
            var config=_configuration.GetSection("CloudinaryConfig").Get<CloudinaryConfigDTO>();
            PhotosUploder photosUploder = new PhotosUploder(config.CloudName,config.ApiKey,config.ApiSecret);
            var uploadedPhotos = await photosUploder.MultipleUploadPhotos(urls);
            foreach (var photo in uploadedPhotos)
            {
                Photo photoObj = new Photo();
                photoObj.IsActive = true;
                photoObj.PublicURL = photo.SecureUrl.AbsoluteUri;
                photoObj.PlotId = plotId;
                _unitOfWork.photoRepoAccess.Add(photoObj);



            }
           await _unitOfWork.Save();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(AddPlotAndPhotos entity, List<IFormFile> postedFiles)
        {

            string userId = _userManager.GetUserId(User);
            Plot temp2 = new Plot();
            if (ModelState.IsValid)
            {

                if (entity.PlotId == 0)
                {
                   
                    Plot temp = ModelConversion.ModelConversionUsingJSON<AddPlotAndPhotos, Plot>(entity);
                    temp.UserId = userId;// _userManager.GetUserId(User); 
                    var plotId = _unitOfWork.plotRepoAccess.SetAndGet(temp).PlotId;
                    await _unitOfWork.Save();
                    if (postedFiles.Count > 0)
                    {
                        var urls = CopyImages(postedFiles, userId);
                        await AddPhotosInDatabase(urls, temp.PlotId);
                    }
                    
                }
                else
                {
                   await _unitOfWork.plotRepoAccess.Update(temp2);//already saving inside
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
