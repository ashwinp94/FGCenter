﻿using System;
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
    public class GamesController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var model = new GameIndexViewModel();

            //get users
            ApplicationUser user = await GetCurrentUserAsync();
            model.User = user;

            //get post counts
            var PostsCount = await (
                from g in _context.Game
                from p in _context.Post.Where(co => g.GameId == co.GameId).DefaultIfEmpty()
                group new { g, p } by new { g.GameId, g.Name, g.ImageUrl } into grouped
                select new GamesWithPostCountViewModel
                {
                    PostCount = grouped.Where(gr => gr.p != null).Count(),
                    Game = new Game
                    {
                        GameId = grouped.Key.GameId,
                        Name = grouped.Key.Name,
                        ImageUrl = grouped.Key.ImageUrl,
                    }
                }).ToListAsync();

            model.GamesWithPostCount = PostsCount;

            return View(model);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //CREATE A NEW GameDetailViewModel
            var model = new GameDetailViewModel();

            //GET GAME INFO
            var game = await _context.Game
                .Where(g => g.GameId == id).FirstOrDefaultAsync();

                model.Game = game;

            //GET COMMENT COUNT
            var CommentCount = await (
                from p in _context.Post.Where(po => po.GameId == id)
                from c in _context.Comment.Where(co => p.PostId == co.PostId).DefaultIfEmpty()
                group new { p, c } by new { p.PostId, p.Title, p.DatePosted, p.User, p.User.UserName, p.User.Id } into grouped
                select new PostWithCommentCountViewModel
                {
                    NumberOfComments = grouped.Where(gr => gr.c != null).Count(),
                    Post = new Post
                    {
                        PostId = grouped.Key.PostId,
                        Title = grouped.Key.Title,
                        DatePosted = grouped.Key.DatePosted,
                        User = new ApplicationUser
                        {
                            Id = grouped.Key.User.Id,
                            UserName = grouped.Key.User.UserName
                        }
                    }
                }).ToListAsync();

            model.PostsWithCommentCount = CommentCount;

            //GET USER

            ApplicationUser user = await GetCurrentUserAsync();
            model.User = user;

            return View(model);
           
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,Name,ImageUrl,DeveloperName")] Game game)
        {
            ApplicationUser user = await GetCurrentUserAsync();

            if (user != null && user.UserName == "admin@admin.com" && ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Name,ImageUrl,DeveloperName")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }
            ApplicationUser user = await GetCurrentUserAsync();

            if (user != null && user.UserName =="admin@admin.com" && ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            return View(game);
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
