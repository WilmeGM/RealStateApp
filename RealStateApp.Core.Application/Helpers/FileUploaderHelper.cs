using Microsoft.AspNetCore.Http;

namespace RealStateApp.Core.Application.Helpers
{
    public static class FileUploaderHelper
    {
        public static string UploadImage(IFormFile file, string id, bool editMode = false, string imageUrl = "")
        {
            if (editMode && file == null) return imageUrl;

            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string basePath = $"/Images/ProfilePictures/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            string pathWithFileName = Path.Combine(path, fileName);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            // Guardar el archivo en el servidor
            using (var stream = new FileStream(pathWithFileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Eliminar la imagen anterior si está en modo edición y hay una URL previa
            if (editMode && !string.IsNullOrEmpty(imageUrl))
            {
                string oldImagePath = Path.Combine(path, Path.GetFileName(imageUrl));

                if (File.Exists(oldImagePath)) File.Delete(oldImagePath);
            }

            return $"{basePath}/{fileName}";
        }

        public static string UploadImage(IFormFile file, string agentId, int imageNumber, bool editMode = false, string imageUrl = "")
        {
            if (editMode && file == null) return imageUrl;

            if(file == null) return null;

            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo(file.FileName);
            string fileName = $"{imageNumber}_{guid}{fileInfo.Extension}";

            string basePath = $"/Images/Properties/{agentId}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            string pathWithFileName = Path.Combine(path, fileName);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            // Guardar el archivo en el servidor
            using (var stream = new FileStream(pathWithFileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Eliminar la imagen anterior si está en modo edición y hay una URL previa
            if (editMode && !string.IsNullOrEmpty(imageUrl))
            {
                string oldImagePath = Path.Combine(path, Path.GetFileName(imageUrl));

                if (File.Exists(oldImagePath)) File.Delete(oldImagePath);
            }

            return $"{basePath}/{fileName}";
        }
    }
}
