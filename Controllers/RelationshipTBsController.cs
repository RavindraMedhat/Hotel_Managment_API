using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.Models;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipTBsController : ControllerBase
    {
        private readonly DataContext _context;

        public RelationshipTBsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/RelationshipTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelationshipTB>>> GetRelationshipTB()
        {
            return await _context.RelationshipTB.ToListAsync();
        }

        // GET: api/RelationshipTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RelationshipTB>> GetRelationshipTB(int id)
        {
            var relationshipTB = await _context.RelationshipTB.FindAsync(id);

            if (relationshipTB == null)
            {
                return NotFound();
            }

            return relationshipTB;
        }

        // PUT: api/RelationshipTBs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelationshipTB(int id, RelationshipTB relationshipTB)
        {
            if (id != relationshipTB.Relationship_ID)
            {
                return BadRequest();
            }

            _context.Entry(relationshipTB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationshipTBExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RelationshipTBs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RelationshipTB>> PostRelationshipTB(RelationshipTB relationshipTB)
        {
            _context.RelationshipTB.Add(relationshipTB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelationshipTB", new { id = relationshipTB.Relationship_ID }, relationshipTB);
        }

        // DELETE: api/RelationshipTBs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RelationshipTB>> DeleteRelationshipTB(int id)
        {
            var relationshipTB = await _context.RelationshipTB.FindAsync(id);
            if (relationshipTB == null)
            {
                return NotFound();
            }

            _context.RelationshipTB.Remove(relationshipTB);
            await _context.SaveChangesAsync();

            return relationshipTB;
        }

        private bool RelationshipTBExists(int id)
        {
            return _context.RelationshipTB.Any(e => e.Relationship_ID == id);
        }
    }
}
