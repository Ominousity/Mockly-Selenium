using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EndpointInternalRepo : IEndpointInternalRepo
    {
        private readonly EndpointDbContext _context;
        public EndpointInternalRepo(EndpointDbContext dbContext) 
        {
            _context = dbContext;
        }

        public async Task AddEndpointAsync(Endpoint endpoint)
        {
            await _context.Endpoints.AddAsync(endpoint);
            _context.SaveChanges();
        }

        public async Task DeleteEndpointAsync(Guid endpointID)
        {
            var endpoint = await _context.Endpoints.FirstOrDefaultAsync(e => e.Id == endpointID);
            if (endpoint != null)
            {
                _context.Endpoints.Remove(endpoint);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Endpoint with ID {endpointID} not found.");
            }
        }

        public async Task<Endpoint> GetEndpointAsync(string path, HttpMethods method)
        {
            return await _context.Endpoints.FirstOrDefaultAsync(e => e.Path == path && e.Method == method) ?? throw new KeyNotFoundException($"Endpoint with the given Path {path} and method {method} not found.");
        }

        public async Task<IEnumerable<Endpoint>> GetEndpointsAsync()
        {
            return await _context.Endpoints.ToListAsync();
        }

        public async Task UpdateEndpointAsync(Endpoint endpoint)
        {
            var existingEndpoint = await _context.Endpoints.FirstOrDefaultAsync(e => e.Id == endpoint.Id);
            if (existingEndpoint != null)
            {
                _context.Entry(existingEndpoint).CurrentValues.SetValues(endpoint);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Endpoint with ID {endpoint.Id} not found.");
            }
        }
    }
}
