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
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace Bookshop.Controllers
{
    [Authorize(Roles = "staff")]
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
        [AllowAnonymous]
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
            _context.Genres.Add(create_genre);
            _context.SaveChanges();
            return Ok(create_genre);
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
        [AllowAnonymous]
        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct()
        {
            var show_product = await _context.Products.Include(t => t.Type).ToListAsync();
            return Ok(show_product);
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromForm]ProductCreationDTO request)
        {
            try
            {
                string filePath = await _formFileService.UploadFileAsync(request.fileImage, "user");
                var create_product = _mapper.Map<Product>(request);
                create_product.fileImage = filePath;
                _context.Products.Add(create_product);
                _context.SaveChanges();
                return Ok("Created");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("getSingleProduct")]
        public async Task<IActionResult> GetSingleProduct([FromQuery] int id)
        {
            var find_product = await _context.Products
                .Include(p => p.ProductGenres)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Id == id);
            return Ok(find_product);
        }
        [HttpPut("updateImage")]
        public async Task<IActionResult> UpdateImage([FromForm] IFormFile request, int id)
        {
            var old_product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            string savedPath = await _formFileService.UploadFileAsync(request, "user");
            old_product.fileImage = savedPath;
            _context.Update(old_product);
            _context.SaveChanges();
            return Ok("Updated Image");
        }
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDTO request, int id)
        {
            var old_product = _context.Products
                .Include(p => p.ProductGenres)
                .Include(p => p.Type)
                .FirstOrDefault(p => p.Id == id);

            var update_product = _mapper.Map(request, old_product);
            _context.Products.Update(update_product);
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
        [AllowAnonymous]
        [HttpGet("filterProduct")]
        public async Task<IActionResult> FilterProduct([FromQuery] int id)
        {
            var foundRelatedProduct = _context.ProductGenres.Include(p=>p.Product).Where(g => g.GenreId == id);
            return Ok(foundRelatedProduct.ProjectTo<ProductGenresDTO>(_mapper.ConfigurationProvider));
        }
        [AllowAnonymous]
        [HttpGet("searchProduct")]
        public async Task<IActionResult> SearchProduct(string searchString)
        {
            var products = await _context.Products.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                var f_products = await _context.Products.Where(s => s.Name.Contains(searchString)).ToListAsync();
                return Ok(f_products);
            }

            return Ok(products);
        }

        [HttpGet("searchGenre")]
        public async Task<IActionResult> SearchGenre(string searchString)
        {
            var genres = await _context.Genres.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                var f_genres = await _context.Genres.Where(s => s.Name.Contains(searchString)).ToListAsync();
                return Ok(f_genres);
            }

            return Ok(genres);
        }
        [AllowAnonymous]
        [HttpGet("searchProductMain")]
        public async Task<IActionResult> SearchProductMain(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var f_products = await _context.Products.Where(s => s.Name.Contains(searchString)).ToListAsync();
                return Ok(f_products);
            }
            return Ok();
        }

    }
}
