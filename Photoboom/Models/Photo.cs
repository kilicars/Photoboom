using Microsoft.AspNetCore.Http;
using Photoboom.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Photoboom.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }

        [Required]
        [StringLength(60)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Image File")]
        [MaxFileSize(1 * 1024 * 1024)]
        [PermittedExtensions(new string[] { ".jpg", ".png", ".gif", ".jpeg" })]
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public string ImageStorageName { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }

        [NotMapped]
        [Display(Name ="Tags")]
        public string TagString { get; set; }

        public List<Tag> Tags { get; set; }

        public DateTime? CreationDate { get; set; }

    }
}
