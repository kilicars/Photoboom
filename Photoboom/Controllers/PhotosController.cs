using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Photoboom.Data;
using Photoboom.Models;
using Photoboom.Services;

namespace Photoboom.Controllers
{
    public class PhotosController : Controller
    {
        private readonly PhotoService photoService;

        public PhotosController(PhotoService photoService)
        {
            this.photoService = photoService;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            var photos = await photoService.GetPhotosAsync();
            return View(photos);
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var photo = await photoService.GetPhotoAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoId,Title,ImageFile,TagString")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                await photoService.AddPhoto(photo);
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var photo = await photoService.GetPhotoAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await photoService.DeletePhoto(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
