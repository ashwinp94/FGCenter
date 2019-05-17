using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FGCenter.Data;
using FGCenter.Models;
using Microsoft.AspNetCore.Identity;
using FGCenter.Models.ViewModels;

namespace FGCenter.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> YourComments()
        {
            ApplicationUser user = await GetCurrentUserAsync();

            return View(await _context.Comment.Where(p => p.UserId == user.Id).ToListAsync());
        }

        // GET: Comments/Create
        public IActionResult Create(int id)
        {
            var model = new CommentCreateViewModel();
            var post =  _context.Post
                .Where(c => c.PostId == id).FirstOrDefault();
            model.Post = post;
            model.Post.PostId = post.PostId;

            return View(model);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, CommentCreateViewModel model)
        {

            ModelState.Remove("Comment.User");
            ModelState.Remove("Comment.UserId");

            ApplicationUser user = await GetCurrentUserAsync();

            model.Comment.User = user;
            model.Comment.UserId = user.Id;
            model.Comment.PostId = id;

            if (ModelState.IsValid)
            {
                _context.Add(model.Comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Posts", new { id = model.Comment.PostId });
            }
            

            return View(model);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
 
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("CommentId,Text,DatePosted,EditedDate,PostId,UserId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            ApplicationUser user = await GetCurrentUserAsync();

            var CommentBeingTracked = await _context.Comment
                .Where(c => c.CommentId == id).FirstOrDefaultAsync();

            CommentBeingTracked.Text = comment.Text;

            CommentBeingTracked.EditedDate = DateTime.Now;
            

            if (user != null && ModelState.IsValid && CommentBeingTracked.UserId == user.Id)
            {
                try
                {
                    _context.Update(CommentBeingTracked);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { id = CommentBeingTracked.PostId });
            }

            return View(CommentBeingTracked);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var comment = await _context.Comment.FindAsync(id);
            if (user!= null && user.Id == comment.UserId)
            {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Games");
            }
            else
            {
                return RedirectToAction("Index", "Games");
            }

        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}
