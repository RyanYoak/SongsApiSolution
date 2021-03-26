using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongsApi.Domain;
using SongsApi.Models.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace SongsApi.Controllers
{
    public class SongsController : ControllerBase
    {
        private SongsDataContext _context;
        private IMapper _mapper;
        private MapperConfiguration _config;

        public SongsController(SongsDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        [HttpDelete("/songs/{id:int}")]
        public async Task<ActionResult> RemoveSong(int id)
        {
            var savedSong = await _context.GetActiveSongs().SingleOrDefaultAsync(s => s.Id == id);

            if(savedSong != null)
            {
                savedSong.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }


        [HttpPost("/songs")]
        public async Task<ActionResult> AddASong([FromBody] PostSongRequest request)
        {
            /// Simulating network delay
            //await Task.Delay(8 * 1000);


            /// 1. Validate the entity (maybe use fluent validation)
            ///     If not valid, send a 400 back with or without details about what they did wrong.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /// 2. Modify the domain, in other words, save it to the database
            //var song = new Song
            //{
            //    Title = request.Title,
            //    Artist = request.Artist,
            //    RecomendedBy = request.RecommendedBy,
            //    IsActive = true,
            //    AddedToInventory = DateTime.Now
            //};

            var song = _mapper.Map<Song>(request);

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            /// 3. Return
            ///     201 Created status code
            ///     Give them a copy of the new created rwsource
            ///     Add a locaiton header of the newly created resoucre
            ///         Location: http://localhost:1337/songs/?
            ///         
            //var response = new GetASongResponse
            //{
            //    Id = song.Id,
            //    Title = song.Title,
            //    Artist = song.Artist,
            //    RecommendedBy = song.RecomendedBy
            //};
            var response =_mapper.Map<GetASongResponse>(song);

            return CreatedAtRoute("songs#getasong", new { id = response.Id }, response);
        }




        [HttpGet("/songs")]
        public async Task<ActionResult> GetAllSongs()
        {

            /// Simulating network delay
            //await Task.Delay(8 * 1000);

            var response = new GetSongsResponse();

            var data = await _context.GetActiveSongs()
                //.Select(song => _mapper.Map<SongSummaryItem>(song))
                .ProjectTo<SongSummaryItem>(_config)
                .OrderBy(song => song.Title)
                .ToListAsync();

            response.data = data;

            return Ok(response);
        }

        [HttpGet("/songs/{id:int}", Name = "songs#getasong")]
        public async Task<ActionResult> GetASong(int id)
        {
            var response = await _context.GetActiveSongs()
                .Where(s => s.Id == id)
                .ProjectTo<GetASongResponse>(_config)
                //.Select(s => new GetASongResponse
                //{
                //    Id = s.Id,
                //    Title = s.Title,
                //    Artist = s.Artist,
                //    RecommendedBy = s.RecomendedBy
                /*})*/.SingleOrDefaultAsync(); // This returns either one or no thing
            
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
