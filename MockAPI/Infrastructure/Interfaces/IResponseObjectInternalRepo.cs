using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IResponseObjectInternalRepo
    {
        Task AddResponseObjectAsync(ResponseObject responseObject);
        Task DeleteResponseObjectAsync(Guid responseObjectID);
        Task<ResponseObject> GetResponseObjectAsync(Guid responseObjectID);
        Task UpdateResponseObjectAsync(ResponseObject responseObject);
    }
}
