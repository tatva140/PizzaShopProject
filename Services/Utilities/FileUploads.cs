using System.Runtime;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Utilities;

public class FileUploads
{
        private readonly IUserRepository _userRepository;
        public FileUploads(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

        public string UploadProfileImage(IFormFile file, int id)
        {
            if(file!=null && file.Length>0)
        {
            return null;
        }
        string uploadsFolder=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","images");
        string filename=$"{id}_{Path.GetExtension(file.FileName)}";
        string filePath=Path.Combine(uploadsFolder,filename);

        using (var stream=new FileStream(filePath,FileMode.Create))
        {
            file.CopyTo(stream);
        }
        _userRepository.SaveProfileImage(filename,id);
        return filename;
        }
}
