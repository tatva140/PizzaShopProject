using System.Runtime;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Utilities;

public class FileUploads
{
        private readonly IUserRepository _userRepository;
        private readonly string _imageFolderPath="wwwroot/images/";
        public FileUploads(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

        public string UploadProfileImage(IFormFile profileImg)
        { 

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImg.FileName;
            var filePath = Path.Combine(_imageFolderPath, uniqueFileName);

            Directory.CreateDirectory(_imageFolderPath); // Ensure directory exists

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profileImg.CopyToAsync(stream)
;
            }
                return uniqueFileName;
        }
        
}

