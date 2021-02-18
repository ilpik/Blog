using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]

    public class PanelController : Controller
    {
        private readonly IRepository _repository;
        private readonly IFileManager _fileManager;

        public PanelController(
            IRepository repository,
            IFileManager fileManager
            )
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public IActionResult Index()
        {
            var post = _repository.GetAllPosts();
            return View(post);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            { 
                return View(new PostViewModel());
            }
            else
            {
                var post = _repository.GetPost((int)id);
                return View(new PostViewModel {  
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    CurrentImage = post.Image

                });
            }

        }

            [HttpPost]
            public async Task<IActionResult> Edit(PostViewModel postVM)
            {
                var post = new Post {
                    Id = postVM.Id,
                    Title = postVM.Title,
                    Body = postVM.Body,
                };
                if (postVM.Image == null) post.Image = postVM.CurrentImage;
                else post.Image = await _fileManager.SaveImage(postVM.Image); 

                if (post.Id > 0)
                    _repository.UpdatePost(post);
                else
                    _repository.AddPost(post);

                if (await _repository.SaveChangesAsync())
                    return RedirectToAction("Index");
                else
                    return View(post);
            }


            [HttpGet]
            public async Task<IActionResult> Remove(int id)
            {
                _repository.RemovePost(id);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

    }
}
