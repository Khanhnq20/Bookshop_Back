using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookshop.Entity;
using Bookshop.SQLContext;
using AutoMapper;
using Bookshop.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using Bookshop.Interface;
using System.IO;

namespace Bookshop.Controllers
{
    [ApiController]
    [Route("api/staff")]
    public class StaffController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IFormFileService _formFileService;

        public StaffController(DataContext context, IMapper mapper, IFormFileService formFileService)
        {
            _context = context;
            _mapper = mapper;
            _formFileService = formFileService;
        }

        [HttpGet("getGenre")]
        public async Task<IActionResult> GetGenre()
        {
            var show_genre = await _context.Genres.ToListAsync();
            return Ok(show_genre);
        }
        [HttpPost("createGenre")]
        public async Task<IActionResult> CreateGenre([FromBody]GenreCreationDTO request)
        {
            var create_genre = _mapper.Map<Genre>(request);
            await _context.Genres.AddAsync(create_genre);
            _context.SaveChanges();
            return Ok("Created");
        }
        [HttpDelete("deleteGenre")]
        public async Task<IActionResult> DeleteGenre([FromQuery] int id)
        {
            var find_genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
            if(find_genre == null)
            {
                return BadRequest("SOS");
            }
            _context.Genres.Remove(find_genre);
            _context.SaveChanges();
            return Ok("Deleted");
        }
        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct()
        {
            var show_product = await _context.Products.Include(t => t.Type).ToListAsync();
            return Ok(show_product);
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromForm]ProductCreationDTO request)
        {
            string filePath = await _formFileService.UploadFileAsync(request.fileImage, "user");
            var create_product = _mapper.Map<Product>(request);
            create_product.fileImage = filePath;

            create_product.Genres.ForEach(genre => _context.Entry(genre).State = EntityState.Unchanged);

            _context.Products.Add(create_product);
            _context.SaveChanges();
            return Ok("Created");
        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductCreationDTO request,[FromQuery] int id)
        {
            var old_product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            var update_product = _mapper.Map(request,old_product);
            _context.Update(update_product);
            _context.SaveChanges();
            return Ok("Updated");
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if(product == null)
                {
                    return BadRequest("SOS");
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok("Deleted");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
