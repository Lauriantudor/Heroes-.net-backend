using Dapper;
using Hero_Api.Data;
using Hero_Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Hero_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly DataContext dataContext;
        public HeroController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hero>>> GetAllHeroes()
        {
            if (this.dataContext.Heroes==null)
            {
                return NotFound();
            }
            return await this.dataContext.Heroes.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHero(int id)
        {
            if (this.dataContext.Heroes == null)
            {
                return NotFound();
            }
            var hero = await this.dataContext.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }
            return hero;

        }
        [HttpPost]
        public async Task<IActionResult> CreateHero([FromBody] Hero hero)
        {
            await this.dataContext.Heroes.AddAsync(hero);
            await this.dataContext.SaveChangesAsync();

            return Ok(hero);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHero(int id, Hero hero)
        {
            if(id != hero.Id)
            {
                return BadRequest();
            }   
            this.dataContext.Entry(hero).State = EntityState.Modified;
            try
            {
                await this.dataContext.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if (!HeroAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
       private bool HeroAvailable(int id)
        {
            return this.dataContext.Heroes.Any(h => h.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHero(int id)
        {
            if (this.dataContext.Heroes ==null)
            {
                return NotFound();
            }

            var hero =  await this.dataContext.Heroes.FindAsync(id);
            if (hero == null) 
            { 
                return NotFound();
            }
            this.dataContext.Heroes.Remove(hero);
            await this.dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
