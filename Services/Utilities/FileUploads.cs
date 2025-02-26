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

        public string UploadProfileImage(IFormFile profileImg)
        { 
                var fileName = Path.GetFileName(profileImg.FileName);

                var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                var fileExtension = Path.GetExtension(fileName);

                var newFileName = String.Concat(myUniqueFileName, fileExtension);

                var filepath = 
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{newFileName}";

                using (FileStream fs = System.IO.File.Create(filepath))
                {
                     profileImg.CopyTo(fs);
                     fs.Flush();
                }
                return newFileName;
        }
        
}

internal class PhysicalFileProvider
{
    private string v;

    public PhysicalFileProvider(string v)
    {
        this.v = v;
    }

    public string Root { get; internal set; }

}