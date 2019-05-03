using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FGCenter.Data;
using FGCenter.Models;
using FGCenter.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace FGCenter.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Post.Include(p => p.Game).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new PostDetailViewModel();

            var getpost = await _context.Post
                 .Include(p=> p.User)
                 .Include(p => p.Game)
                 .Where(p => p.PostId == id).FirstOrDefaultAsync();

            model.Post = getpost;

            var GroupedComment = await _context.Comment
                .Include(p => p.Post)
                .Include(p => p.User)
                .Where(p => p.PostId == id)
                .ToListAsync();
     

            model.GroupedComments = GroupedComment;

            //GET USER

            ApplicationUser user = await GetCurrentUserAsync();
            model.User = user;

            return View(model);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Text,DatePosted,EditedDate,UserId,GameId")] Post post)
        {

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            ApplicationUser user = await GetCurrentUserAsync();

            post.User = user;
            post.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = post.PostId });
            }


            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Text,DatePosted,EditedDate,UserId,GameId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            ApplicationUser user = await GetCurrentUserAsync();

            post.User = user;
            post.UserId = user.Id;

            var PostBeingTrack = await _context.Post
                .Where(p => p.PostId == id).FirstOrDefaultAsync();

            PostBeingTrack.EditedDate = DateTime.Now;
            PostBeingTrack.Text = post.Text;
            PostBeingTrack.Title = post.Title;
            PostBeingTrack.DatePosted = post.DatePosted;

            if (ModelState.IsValid && PostBeingTrack.UserId == user.Id)
            {
                try
                {
                    _context.Update(PostBeingTrack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(PostBeingTrack.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(PostBeingTrack);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Game)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var post = await _context.Post.FindAsync(id);

            if(user.Id == post.UserId)
            {
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}
