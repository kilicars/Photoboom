using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Photoboom.Data;
using Photoboom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Photoboom.Services
{
    public class PhotoService
    {
        private readonly PhotoContext photoContext;
        private readonly string serverFilePath;
        private readonly string photoDirectory;
        public PhotoService(PhotoContext photoContext, IConfiguration configuration)
        {
            this.photoContext = photoContext;
            serverFilePath = configuration.GetValue<string>("ServerFilePath");
            photoDirectory = configuration.GetValue<string>("PhotoDirectory") + "/";
        }

        public async Task<List<Photo>> GetPhotosAsync()
        {
            var photos = await photoContext.Photos.ToListAsync();
            foreach (var photo in photos)
            {
                photo.ImageUrl = photoDirectory + photo.ImageStorageName;
            }
            return photos;
        }

        public async Task<Photo> GetPhotoAsync(int? id)
        {
            var photo = await photoContext.Photos
                .Include(photo => photo.Tags)
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photo != null)
            {
                photo.ImageUrl = photoDirectory + photo.ImageStorageName;
                var tags = new List<string>();
                foreach (var tag in photo.Tags)
                {
                    tags.Add(tag.TagName);
                }
                photo.TagString = string.Join(", ", tags);
            }
            return photo;
        }

        public async Task AddPhoto(Photo photo)
        {
            photo.Tags = ParseTags(photo.TagString);
            photo.CreationDate = DateTime.Now;
            if (photo.ImageFile != null)
            {
                await UploadFileAsync(photo);
            }
            photoContext.Add(photo);
            await photoContext.SaveChangesAsync();
        }

        public async Task DeletePhoto(int id)
        {
            var photo = await photoContext.Photos.FindAsync(id);
            if (!string.IsNullOrEmpty(photo.ImageStorageName))
            {
                DeleteFile(photo.ImageStorageName);
            }
            photoContext.Photos.Remove(photo);
            await photoContext.SaveChangesAsync();
        }

        private void DeleteFile(string imageStorageName)
        {
            string filePath = serverFilePath + imageStorageName;
            System.IO.File.Delete(filePath);
        }

        private async Task UploadFileAsync(Photo photo)
        {
            string fileNameForStorage = FormFileName(photo.Title, photo.ImageFile.FileName);
            string filePath = serverFilePath + fileNameForStorage;
            using (var stream = System.IO.File.Create(filePath))
            {
                await photo.ImageFile.CopyToAsync(stream);
            }
            photo.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            //var fileNameForStorage = $"{Path.GetRandomFileName()}{fileExtension}";
            return fileNameForStorage;
        }

        private List<Tag> ParseTags(string tagString)
        {
            var tags = new List<Tag>();
            bool tagBegins = false;
            var curTagName = string.Empty;

            foreach (var c in tagString)
            {
                if (c == '#')
                {
                    if (tagBegins && !string.IsNullOrEmpty(curTagName))
                    {
                        var newTag = new Tag
                        {
                            TagName = curTagName
                        };
                        tags.Add(newTag);
                        curTagName = string.Empty;
                    }
                    tagBegins = true;
                    curTagName += c;
                }
                else if (char.IsLetterOrDigit(c))
                {
                    curTagName += c;
                }
            }
            // add the last one
            if (tagBegins && !string.IsNullOrEmpty(curTagName))
            {
                var newTag = new Tag
                {
                    TagName = curTagName
                };
                tags.Add(newTag);
            }

            return tags;
        }
    }
}
