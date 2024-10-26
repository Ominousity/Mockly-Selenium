using Infrastructure.Entities;
using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IEndpointInternalRepo
    {
        Task AddEndpointAsync(Endpoint endpoint);
        Task DeleteEndpointAsync(Guid endpointID);
        Task<Endpoint> GetEndpointAsync(string path, HttpMethods method);
        Task<IEnumerable<Endpoint>> GetEndpointsAsync();
        Task UpdateEndpointAsync(Endpoint endpoint);
    }
}
