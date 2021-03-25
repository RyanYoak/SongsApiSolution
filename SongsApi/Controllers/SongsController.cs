using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongsApi.Domain;
using SongsApi.Models.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsApi.Controllers
{
    public class SongsController : ControllerBase
    {
        private SongsDataContext _context;

        public SongsController(SongsDataContext context)
        {
            _context = context;
        }

        [HttpPost("/songs")]
        public async Task<ActionResult> AddASong([FromBody] PostSongRequest request)
        {
            /// 1. Validate the entity (maybe use fluent validation)
            ///     If not valid, send a 400 back with or without details about what they did wrong.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /// 2. Modify the domain, in other words, save it to the database
            var song = new Song
            {
                Title = request.Title,
                Artist = request.Artist,
                RecomendedBy = request.RecommendedBy,
                IsActive = true,
                AddedToInventory = DateTime.Now
            };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            /// 3. Return
            ///     201 Created status code
            ///     Give them a copy of the new created rwsource
            ///     Add a locaiton header of the newly created resoucre
            ///         Location: http://localhost:1337/songs/?
            ///         
            var response = new GetASongResponse
            {
                Id = song.Id,
                Title = song.Title,
                Artist = song.Artist,
                RecommendedBy = song.RecomendedBy
            };

            return CreatedAtRoute("songs#getasong", new { id = response.Id }, response);
        }




        [HttpGet("/songs")]
        public async Task<ActionResult> GetAllSongs()
        {
            var response = new GetSongsResponse();

            var data = await _context.Songs
                .Where(song => song.IsActive)
                .Select(song => new SongSummaryItem
                {
                    Id = song.Id,
                    Title = song.Title,
                    Artist = song.Artist,
                    RecommendedBy = song.RecomendedBy
                })
                .OrderBy(song => song.Title)
                .ToListAsync();

            response.data = data;

            return Ok(response);
        }

        [HttpGet("/songs/{id:int}", Name = "songs#getasong")]
        public async Task<ActionResult> GetASong(int id)
        {
            var response = await _context.Songs.Where(s => s.IsActive && s.Id == id)
                .Select(s => new GetASongResponse
                {
                    Id = s.Id,
                    Title = s.Title,
                    Artist = s.Artist,
                    RecommendedBy = s.RecomendedBy
                }).SingleOrDefaultAsync(); // This returns either one or no thing
            
            if(response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
