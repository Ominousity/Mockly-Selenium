using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ResponseObjectInternalRepo : IResponseObjectInternalRepo
    {
        private readonly ResponseObjectDbContext _context;
        public ResponseObjectInternalRepo(ResponseObjectDbContext context) 
        {
            _context = context;
        }

        public async Task AddResponseObjectAsync(ResponseObject responseObject)
        {
            await _context.ResponseObjects.AddAsync(responseObject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResponseObjectAsync(Guid responseObjectID)
        {
            var responseObject = await _context.ResponseObjects.FirstOrDefaultAsync(e => e.Id == responseObjectID);
            if (responseObject != null)
            {
                _context.ResponseObjects.Remove(responseObject);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"ResponseObject with ID {responseObjectID} not found.");
            }
        }

        public async Task<ResponseObject> GetResponseObjectAsync(Guid responseObjectID)
        {
            return await _context.ResponseObjects.FirstOrDefaultAsync(e => e.Id == responseObjectID) ?? throw new KeyNotFoundException($"ResponseObject with ID {responseObjectID} not found.");
        }

        public async Task UpdateResponseObjectAsync(ResponseObject responseObject)
        {
            var responseObjectToUpdate = _context.ResponseObjects.FirstOrDefault(e => e.Id == responseObject.Id);
            if (responseObjectToUpdate != null)
            {
                _context.Entry(responseObjectToUpdate).CurrentValues.SetValues(responseObject);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"ResponseObject with ID {responseObject.Id} not found.");
            }
        }
    }
}
