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
        Random random { get; set; }
        public PlotController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,IConfiguration configuration,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _userManager= userManager;
            _configuration = configuration;
            _db = db;
             random = new Random();
        }
        private string getRandomImages()
        {
            string[] imagesUrl = {
                "https://res.cloudinary.com/daers/image/upload/v1650460047/amknvqjn3dlrjvulxnp7.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1650460011/v4r2382off4t3qi7elmy.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1650459766/cwo0hv7f5c1kwu8ua5nh.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1650459737/ywu5vdyudhfwuw24fdgu.webp",
                "https://res.cloudinary.com/daers/image/upload/v1650459690/juootd12vp4j6jlaiynq.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1650294050/nzgq31oe9kgrogejjez2.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1650293983/ogh3lzueciaty4nfsugp.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1650288943/ctqwbtky7bqlcwvnjcfs.webp",
                "https://res.cloudinary.com/daers/image/upload/v1649800383/jcmzr9xvzz3ikrf0gash.jpg",
                "https://res.cloudinary.com/daers/image/upload/v1649784552/hwuchxf5ys82xto9dhmf.webp",

            };
           
            return imagesUrl[random.Next(0, imagesUrl.Length)];

        }
        private async Task<bool> AddDummyData(string userId)
        {
            string[] blocks=  { "A", "B", "C" };
            string[] types = { "Buy", "Sell" };
            Dictionary<string, double> blockDict = new Dictionary<string, double>();
            blockDict.Add("A", 10);
            blockDict.Add("B", 5);
            blockDict.Add("C", 20);


            var society = _unitOfWork.societyRepoAccess.GetFirstOrDefault(x => x.Name.ToLower() == "topcity");
            DateTime dateTime = new DateTime(2021,1,10) ;
            DateTime pres = dateTime;// new DateTime();
           // List<List<Plot>> allPlots = new List<List<Plot>>();
            for (int i = 1; i <= 12; i++)
            {
               // List<Plot> p = new List<Plot>();
                for(int j = 0; j < 20; j++)
                {
                    int blockIndex = random.Next(0, blocks.Length );
                    Plot plot = new Plot();
                    plot.UserId = userId;
                    plot.Block = blocks[blockIndex];
                    plot.PlotSize= blockDict[blocks[blockIndex]];
                    plot.Price = random.Next((int)(1000 * plot.PlotSize), (int)(10000 * plot.PlotSize));
                    plot.Type= types[random.Next(0, types.Length )];
                    plot.UpdateOn = pres.ToString();
                    plot.SocietyId = society.SocietyId;
                    plot.CompleteAddress = "CompleteAddressCompleteAddressCompleteAddress";
                    plot.Description = "DescriptionDescriptionDescriptionDescriptionDescription";
                    // p.Add(plot);
                     _unitOfWork.plotRepoAccess.Add(plot);
                    await _unitOfWork.Save();

                    var image = getRandomImages();
                    Photo photo = new Photo();
                    photo.IsActive = true;
                    photo.PlotId = plot.PlotId;
                    photo.PublicURL = image;
                    _unitOfWork.photoRepoAccess.Add(photo);
                   await  _unitOfWork.Save();
                   
                }
                pres=dateTime.AddMonths(i);
               // allPlots.Add(p);
            }
            return true;
        }
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
            Tuple<List<Plot>, decimal> tuple = null;
            List<Plot> plots = new List<Plot>();
            //getall plots
            if (category != null && search != null)
            {
                tuple = _unitOfWork.plotRepoAccess.GetAllWithPagination(x => x.Type == category && x.SocietyTBL.Name.ToLower() == search.ToLower(), page, pageResult: 3);
            }
            else if (category != null)
            {
                tuple = _unitOfWork.plotRepoAccess.GetAllWithPagination(x => x.Type == category, page, pageResult: 10000);
            }
            if (tuple?.Item1?.Count > 0)
            {
                plots = tuple.Item1;

                ViewBag.currentPage = page;
                ViewBag.pageCount = tuple?.Item2;
                ViewBag.category = category;//tuple?.Item2;
                ViewBag.search = search;//tuple?.Item2;
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

        }
        public async Task<IActionResult> Index()
        {
           List< ListPlotsToAnyone> listPlots = new List<ListPlotsToAnyone>();
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id.ToString();
            var Roles = _userManager.GetRolesAsync(user).Result.ToList();
            var plots =_unitOfWork.plotRepoAccess.GetAll(x => x.UserId == userId).ToList();
            foreach (var plot in plots)
            {
                ListPlotsToAnyone listPlotsToAnyone = new ListPlotsToAnyone();
                listPlotsToAnyone._Plot = plot;
                listPlotsToAnyone.Photos = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotTBL.PlotId == plot.PlotId).ToList();
                listPlotsToAnyone._Plot.SocietyTBL = _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == plot.SocietyId);
                listPlotsToAnyone.IsVerified = Roles.Exists(x => SD.VerifiedUser==x); ;

                listPlots.Add(listPlotsToAnyone);
            }

            
      /*      var entities = _unitOfWork.photoRepoAccess.GetAll(x => x.PlotTBL.UserId == userId)
                .Select(x=> {
                    x.PlotTBL.ApplicationUserTBL = new ApplicationUser() { Roles = Roles };
                    x.PlotTBL.SocietyTBL = _unitOfWork.societyRepoAccess.GetFirstOrDefault(s => s.SocietyId == x.PlotTBL.SocietyId);
                    return x;
                    });
*/
           // var groupByPlotId = entities.GroupBy(x => x.PlotId).ToDictionary(x=>x.Key,x=>x.ToList());
           
            return View(listPlots);
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
                var user = _unitOfWork.applicationUserRepoAccess.GetAll(x=>x.Id==entity.UserId).FirstOrDefault();
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
                    addPlotAndPhotos.Email = user.Email;
                    addPlotAndPhotos.Username = user.UserName;
                    addPlotAndPhotos.Phone = user.PhoneNumber;
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
                var user = _unitOfWork.applicationUserRepoAccess.GetAll(x => x.Id == entity.UserId).FirstOrDefault();
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
                    addPlotAndPhotos.Email = user.Email;
                    addPlotAndPhotos.Username = user.UserName;
                    addPlotAndPhotos.Phone = user.PhoneNumber;
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


            string redirect = nameof(Index);

            

            string userId = _userManager.GetUserId(User);
            /*await this.AddDummyData(userId); // will be commented
            return RedirectToAction(redirect);*/
            Plot temp = null;
            
            if (ModelState.IsValid)
            {

                if (entity.PlotId == 0)
                {
                   
                     temp = ModelConversion.ModelConversionUsingJSON<AddPlotAndPhotos, Plot>(entity);
                    temp.UpdateOn = DateTime.UtcNow.ToString();
                    temp.UserId = userId;// _userManager.GetUserId(User); 
                    var plotId = _unitOfWork.plotRepoAccess.SetAndGet(temp).PlotId;
                    
                    
                }
                else
                {
                    if (userId == entity.UserId )
                    {
                        temp = ModelConversion.ModelConversionUsingJSON<AddPlotAndPhotos, Plot>(entity);
                        temp.UpdateOn = DateTime.UtcNow.ToString();
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
