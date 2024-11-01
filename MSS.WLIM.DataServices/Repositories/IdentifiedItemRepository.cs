using MSS.WLIM.DataServices.Data;
using MSS.WLIM.DataServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MSS.WLIM.DataServices.Repositories
{
    public class IdentifiedItemRepository : IRepository<IdentifiedItems>
    {
        private readonly DataBaseContext _context;

        public IdentifiedItemRepository(DataBaseContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<IdentifiedItems>> GetAll()
        {
            return await _context.WHTblIdentifiedItems.ToListAsync();
        }

        public async Task<IdentifiedItems> Get(string id)
        {
            return await _context.WHTblIdentifiedItems.FindAsync(id);
        }

        public async Task<IdentifiedItems> Create(IdentifiedItems _object)
        {
            _context.WHTblIdentifiedItems.Add(_object);
            await _context.SaveChangesAsync();
            return _object;
        }

        public async Task<IdentifiedItems> Update(IdentifiedItems _object)
        {
            _context.Entry(_object).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return _object;
        }

        public async Task<bool> Delete(string id)
        {
            var data = await _context.WHTblIdentifiedItems.FindAsync(id);
            if (data == null)
            {
                return false;
            }

            _context.WHTblIdentifiedItems.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
