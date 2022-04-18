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
using realstate.DTOs.PostDTO;

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
        [AllowAnonymous]
        public async Task<IActionResult> Plots()
        {
            //getall plots
            var plots = _unitOfWork.plotRepoAccess.GetAll();
            
           
            List<ListPlotsToAnyone> listPlots = new List<ListPlotsToAnyone>();
            var verifiedUsers = _userManager.GetUsersInRoleAsync(SD.VerifiedUser).Result.ToList();
            foreach (var item in plots)
            {
                ListPlotsToAnyone listPlotsToAnyone = new ListPlotsToAnyone();
                var photos = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotId == item.PlotId);
                
                listPlotsToAnyone.Photos = photos.ToList();
                listPlotsToAnyone._Plot = item;
                listPlotsToAnyone._Plot.SocietyTBL= _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == item.SocietyId);
                listPlotsToAnyone.IsVerified = verifiedUsers.Exists(x => x.Id == item.UserId); ;


                listPlots.Add(listPlotsToAnyone);
                    
            }
            return View(listPlots);

            //then get verifiedusers
            //

            /*var entities = _unitOfWork.photoRepoAccess.GetAll(includeProperties: "PlotTBL")
                .Select(x => {
                    x.PlotTBL.SocietyTBL = _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == x.PlotTBL.SocietyId);
                    return x;
                });
            var groupByUserId = entities.GroupBy(x => x.PlotTBL.UserId)
                .ToDictionary(x => x.Key, x => x.ToList()
                .GroupBy(i=>i.PlotId).ToDictionary(i=>i.Key,i=>i.ToList())
                );
           
            Dictionary<string, ListPlotsToAnyone> userIdPlotsDict = new Dictionary<string, ListPlotsToAnyone>();
            foreach (var userId in groupByUserId.Keys)
            {
                bool isVerified  = verifiedUsers.Exists(x => x.Id == userId);
                foreach (var plotId in groupByUserId[userId].Keys)
                {
                    List<Photo> photos = groupByUserId[userId][plotId];
                    Plot plot = photos.FirstOrDefault().PlotTBL;
                    userIdPlotsDict.Add(userId, new ListPlotsToAnyone() { IsVerified = isVerified, Plot = plot, Photos = photos });
                }
                
                
            }
            return View(userIdPlotsDict);*/
        }
        [AllowAnonymous]
        [HttpGet]
       
        public async Task<IActionResult> PlotsWithPagination(string? category, string? search, int page = 1)
        {
            List<ListPlotsToAnyone> listPlots = new List<ListPlotsToAnyone>();
            if (page == 0)
            {
                return NoContent();
            }
            Tuple<List<Plot>,decimal> tuple= null;
            List<Plot> plots = new List<Plot>();
            //getall plots
            if (category != null && search!= null){
                tuple = _unitOfWork.plotRepoAccess.GetAllWithPagination(x => x.Type == category && x.SocietyTBL.Name.ToLower()==search.ToLower(), page, pageResult: 3);
            }
            else if (category != null)
            {
                tuple = _unitOfWork.plotRepoAccess.GetAllWithPagination(x => x.Type == category, page, pageResult: 3);
            }
            if (tuple?.Item1?.Count > 0)
            {
                plots = tuple.Item1;

                ViewBag.currentPage = page;
                ViewBag.pageCount = tuple?.Item2; ;
            }
            

            //ViewBag.Count=
            
            var verifiedUsers = _userManager.GetUsersInRoleAsync(SD.VerifiedUser).Result.ToList();
            foreach (var item in plots)
            {
                ListPlotsToAnyone listPlotsToAnyone = new ListPlotsToAnyone();
                var user = _unitOfWork.applicationUserRepoAccess.GetFirstOrDefault(x => x.Id == item.UserId);
                if (user != null)
                {
                    listPlotsToAnyone.Email = user.Email;
                    listPlotsToAnyone.Phone = user.PhoneNumber;
                    listPlotsToAnyone.Username = user.UserName;

                    var photos = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotId == item.PlotId);

                    listPlotsToAnyone.Photos = photos.ToList();
                    listPlotsToAnyone._Plot = item;
                    listPlotsToAnyone._Plot.SocietyTBL = _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == item.SocietyId);

                    listPlotsToAnyone.IsVerified = verifiedUsers.Exists(x => x.Id == item.UserId); ;


                    listPlots.Add(listPlotsToAnyone);
                }
               
                    
            }
            return View(listPlots);

            //then get verifiedusers
            //

            /*var entities = _unitOfWork.photoRepoAccess.GetAll(includeProperties: "PlotTBL")
                .Select(x => {
                    x.PlotTBL.SocietyTBL = _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == x.PlotTBL.SocietyId);
                    return x;
                });
            var groupByUserId = entities.GroupBy(x => x.PlotTBL.UserId)
                .ToDictionary(x => x.Key, x => x.ToList()
                .GroupBy(i=>i.PlotId).ToDictionary(i=>i.Key,i=>i.ToList())
                );
           
            Dictionary<string, ListPlotsToAnyone> userIdPlotsDict = new Dictionary<string, ListPlotsToAnyone>();
            foreach (var userId in groupByUserId.Keys)
            {
                bool isVerified  = verifiedUsers.Exists(x => x.Id == userId);
                foreach (var plotId in groupByUserId[userId].Keys)
                {
                    List<Photo> photos = groupByUserId[userId][plotId];
                    Plot plot = photos.FirstOrDefault().PlotTBL;
                    userIdPlotsDict.Add(userId, new ListPlotsToAnyone() { IsVerified = isVerified, Plot = plot, Photos = photos });
                }
                
                
            }
            return View(userIdPlotsDict);*/
        }
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
        
        public IActionResult Update(int? id)
        {

            AddPlotAndPhotos addPlotAndPhotos = new AddPlotAndPhotos();
            addPlotAndPhotos.PlotId = id.GetValueOrDefault();

            Plot entity = new Plot();
            Society society = new Society();
            if (id != 0 || id != null)
            {

                entity = _unitOfWork.plotRepoAccess.Get(id.GetValueOrDefault());
                if (entity != null)
                {
                    addPlotAndPhotos.PlotSize = entity.PlotSize;
                    addPlotAndPhotos.CompleteAddress = entity.CompleteAddress;
                    addPlotAndPhotos.Block = entity.Block;
                    addPlotAndPhotos.Description = entity.Description;
                    addPlotAndPhotos.Price = entity.Price;
                    addPlotAndPhotos.Type = entity.Type;
                    addPlotAndPhotos.SocietyId = entity.SocietyId;
                    var allSocities = _unitOfWork.societyRepoAccess.GetFirstOrDefault(x => x.SocietyId == entity.SocietyId);
                    addPlotAndPhotos.SocietyName = allSocities.Name;
                    addPlotAndPhotos.UserId = entity.UserId;
                    addPlotAndPhotos.PhotosUrl = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotId == id.GetValueOrDefault()).Select(x => x.PublicURL).ToList();
                    //addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
                }
            }
           
            return View(addPlotAndPhotos);

        }
        public IActionResult DetailSec(int? id)
        {

            AddPlotAndPhotos addPlotAndPhotos = new AddPlotAndPhotos();
            addPlotAndPhotos.PlotId = id.GetValueOrDefault();
            Plot entity = new Plot();
            Society society = new Society();
            if (id != 0 || id != null)
            {

                entity = _unitOfWork.plotRepoAccess.Get(id.GetValueOrDefault());
                if (entity != null)
                {
                    addPlotAndPhotos.PlotSize = entity.PlotSize;
                    addPlotAndPhotos.CompleteAddress = entity.CompleteAddress;
                    addPlotAndPhotos.Block = entity.Block;
                    addPlotAndPhotos.Description = entity.Description;
                    addPlotAndPhotos.Price = entity.Price;
                    addPlotAndPhotos.Type = entity.Type;
                    addPlotAndPhotos.SocietyId = entity.SocietyId;
                    var allSocities = _unitOfWork.societyRepoAccess.GetFirstOrDefault(x => x.SocietyId == entity.SocietyId);
                    addPlotAndPhotos.SocietyName = allSocities.Name;

                    addPlotAndPhotos.PhotosUrl = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotId == id.GetValueOrDefault()).Select(x => x.PublicURL).ToList();
                    //addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
                }
            }
            else
            {

                var allSocities = _unitOfWork.societyRepoAccess.GetAll();
                addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
            }
            return View(addPlotAndPhotos);

        }
        [AllowAnonymous]
        public IActionResult Detail(int? id)
        {
            
            AddPlotAndPhotos addPlotAndPhotos = new AddPlotAndPhotos();
            addPlotAndPhotos.PlotId = id.GetValueOrDefault();
            Plot entity = new Plot();
            Society society = new Society();
            if (id != 0 || id != null)
            {

                entity = _unitOfWork.plotRepoAccess.Get(id.GetValueOrDefault());
                if (entity != null)
                {
                    addPlotAndPhotos.PlotSize = entity.PlotSize;
                    addPlotAndPhotos.CompleteAddress = entity.CompleteAddress;
                    addPlotAndPhotos.Block = entity.Block;
                    addPlotAndPhotos.Description = entity.Description;
                    addPlotAndPhotos.Price = entity.Price;
                    addPlotAndPhotos.Type = entity.Type;
                    addPlotAndPhotos.SocietyId = entity.SocietyId;
                    var allSocities = _unitOfWork.societyRepoAccess.GetFirstOrDefault(x => x.SocietyId == entity.SocietyId);
                    addPlotAndPhotos.SocietyName = allSocities.Name;
                    
                    addPlotAndPhotos.PhotosUrl = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotId == id.GetValueOrDefault()).Select(x => x.PublicURL).ToList();
                    //addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
                }
            }
            else
            {

                var allSocities = _unitOfWork.societyRepoAccess.GetAll();
                addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
            }
            return View(addPlotAndPhotos);
            
        }
        public IActionResult Upsert(int? id)
        {
            AddPlotAndPhotos addPlotAndPhotos = new AddPlotAndPhotos();
            Plot entity = new Plot();
            Society society = new Society();
            if (id != 0 && id != null)
            {

                entity = _unitOfWork.plotRepoAccess.Get(id.GetValueOrDefault());
                if (entity != null)
                {
                    addPlotAndPhotos.PlotSize = entity.PlotSize;
                    addPlotAndPhotos.CompleteAddress = entity.CompleteAddress;
                    addPlotAndPhotos.Block = entity.Block;
                    addPlotAndPhotos.Description = entity.Description;
                    addPlotAndPhotos.Price = entity.Price;
                    addPlotAndPhotos.SocietyId = entity.SocietyId;
                    var allSocities = _unitOfWork.societyRepoAccess.GetFirstOrDefault(x=>x.SocietyId==entity.SocietyId);
                    addPlotAndPhotos.SocietyName= allSocities.Name;
                    addPlotAndPhotos.Type = entity.Type;
                    //addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();

                }
            }
            else
            {

                var allSocities = _unitOfWork.societyRepoAccess.GetAll();
                List<string> PostType = new List<string>();
                PostType.Add("Buy");
                PostType.Add("Sell");
                addPlotAndPhotos.allSocieties = allSocities.Select(x => new SelectListItem() { Text = x.Name, Value = x.SocietyId.ToString() }).ToList();
                addPlotAndPhotos.PostType = PostType.Select(x => new SelectListItem() { Text = x, Value = x.ToString() }).ToList();
            }


            

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
            Plot temp = null;
            string redirect = nameof(Index);
            if (ModelState.IsValid)
            {

                if (entity.PlotId == 0)
                {
                   
                     temp = ModelConversion.ModelConversionUsingJSON<AddPlotAndPhotos, Plot>(entity);
                    temp.UserId = userId;// _userManager.GetUserId(User); 
                    var plotId = _unitOfWork.plotRepoAccess.SetAndGet(temp).PlotId;
                    
                    
                }
                else
                {
                    if (userId == entity.UserId )
                    {
                        temp = ModelConversion.ModelConversionUsingJSON<AddPlotAndPhotos, Plot>(entity);
                        await _unitOfWork.plotRepoAccess.Update(temp); //already saving inside
                        
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                await _unitOfWork.Save();
                if (postedFiles.Count > 0)
                {
                    var urls = CopyImages(postedFiles, userId);
                    await AddPhotosInDatabase(urls, temp.PlotId);
                }
                return RedirectToAction(redirect);
            }
            return View(entity);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var entityFromDB = _unitOfWork.plotRepoAccess.Get(id.GetValueOrDefault());
            if (entityFromDB == null)
            {
                return Redirect(nameof(Index));
            }
            _unitOfWork.plotRepoAccess.Remove(entityFromDB);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var entitiesFromDB = _unitOfWork.plotRepoAccess.GetAll();
            return Json(new { data= entitiesFromDB });
        }
        
        
        #endregion
    }
}
