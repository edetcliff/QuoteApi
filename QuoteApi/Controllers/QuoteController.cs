using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteApi.Data;
using QuoteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuoteController : ControllerBase
    {
        private QuoteApiDbContext Context;
        
        public QuoteController(QuoteApiDbContext context)
        {
            Context = context;
        }

        //Sort
        // GET: api/<QuoteController>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [AllowAnonymous]
        public IActionResult Get(string sort)
        {
            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = Context.Quotes.OrderByDescending(x => x.CreatedAt);
                    break;
                case "asc":
                    quotes = Context.Quotes.OrderBy(x => x.CreatedAt);
                    break;
                default:
                    quotes = Context.Quotes;
                    break;
            }
            return Ok(quotes);
        }
        


        //Paging
        [HttpGet]
        [Route("[action]")]
        public IActionResult Paging(int? pageNumber, int? pageSize)
        {
            var quotes = Context.Quotes;
            var currentPageNumber = pageNumber ?? 5;
            var currentPageSize = pageSize ?? 1;

            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        //Search
        [HttpGet]
        [Route("[action]")]
        public IActionResult Search(string type)
        {
            var quotes = Context.Quotes.Where(x => x.Type.StartsWith(type));
            return Ok(quotes);
        }


        // GET api/<QuoteController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quotes = Context.Quotes.Find(id);
            if(quotes == null)
            {
                return NotFound("No record was found");
            }
            return Ok(quotes);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult MyQuote()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var quotes = Context.Quotes.Where(q => q.UserId == userId);
            return Ok(quotes);
        }

        // POST api/<QuoteController>
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            quote.UserId = userId;
            Context.Quotes.Add(quote);
            Context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<QuoteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var entity = Context.Quotes.Find(id);
            if (entity == null)
            {
                return NotFound("No record found");
            }

            if(entity.UserId != userId)
            {
                return BadRequest("No record with such useId found");
            }
            else
            {
                entity.Title = quote.Title;
                entity.Author = quote.Author;
                entity.Description = quote.Description;
                entity.Type = quote.Type;
                entity.CreatedAt = quote.CreatedAt;
                Context.SaveChanges();
                return Ok("Record Updated Successfully");
            }
            
        }

        // DELETE api/<QuoteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var quotes = Context.Quotes.Find(id);
            if(quotes == null)
            {
                return NotFound("No record found");
            }
            if(userId != quotes.UserId)
            {
                return BadRequest("This Record cannot be deleted");
            }
            else
            {
                Context.Quotes.Remove(quotes);
                Context.SaveChanges();
                return Ok("Quote deleted");
            }
        }
    }
}
