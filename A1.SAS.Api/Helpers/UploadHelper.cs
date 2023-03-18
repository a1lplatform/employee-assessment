namespace A1.SAS.Api.Helpers
{
    public class UploadHelper
    {
        private string folderName = Path.Combine($"wwwroot/upload");
        private readonly string folderImportFile = Path.Combine("wwwroot", "import-files");

        /// <summary>
        /// Get PathName file upload
        /// </summary>
        /// <returns></returns>
        public string GetPathName(string route = "")
        {
            var pathCurrent = Path.Combine(Directory.GetCurrentDirectory(), folderName + route);

            if (!Directory.Exists(pathCurrent))
            {
                DirectoryInfo directory = Directory.CreateDirectory(pathCurrent);
            }
            return pathCurrent;
        }

        /// <summary>
        /// Upload File Image chỉ cho .jpg
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public void UploadFile(IFormFile file, string fileName)
        {
            var pathToSave = Path.Combine(this.GetPathName());

            var fullPath = Path.Combine(pathToSave, fileName + ".jpg");

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        /// <summary>
        /// Delete File chỉ cho .jpg
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFile(string fileName)
        {
            if (File.Exists(Path.Combine(this.GetPathName(), fileName)))
            {
                File.Delete(Path.Combine(this.GetPathName(), fileName));
            }
        }
    }
}
