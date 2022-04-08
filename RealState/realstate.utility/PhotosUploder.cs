using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.utility
{

  public  class PhotosUploder
    {
     
        private Cloudinary _Cloudinary { get; set; }
        public PhotosUploder(string CloudName,string ApiKey,string ApiSecret)
        {

            _Cloudinary = new Cloudinary(new Account(CloudName, ApiKey, ApiSecret));
        }
        public async  Task<ImageUploadResult> UploadPhotos(string imagePath)
        {
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imagePath)
                };
                return await _Cloudinary.UploadAsync(uploadParams);
            }
            catch (Exception e)
            {

                throw e; //TODO: custom exception when we decided it is working fine;
            }  
        }
        public async  Task<List<ImageUploadResult>> MultipleUploadPhotos(List<string> imagePaths)
        {
            try
            {
                List<ImageUploadResult> imageUploads = new List<ImageUploadResult>();
                foreach (var imagePath in imagePaths)
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imagePath)
                    };
                    imageUploads.Add(await _Cloudinary.UploadAsync(uploadParams));
                }
                return imageUploads;
               
                 
            }
            catch (Exception e)
            {

                throw e; //TODO: custom exception when we decided it is working fine;
            }  
        }
    }
}
