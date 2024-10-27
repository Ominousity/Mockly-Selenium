using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EndpointInternalService : IEndpointInternalService
    {
        private readonly IEndpointInternalRepo _endpointInternalRepo;
        private readonly IResponseObjectInternalRepo _responseObjectInternalRepo;

        public EndpointInternalService(IResponseObjectInternalRepo repo, IEndpointInternalRepo endpoint) 
        {
            _responseObjectInternalRepo = repo;
            _endpointInternalRepo = endpoint;
        }
        public async Task AddEndpointAsync(Endpoint endpoint)
        {
            await _endpointInternalRepo.AddEndpointAsync(endpoint);
        }

        public async Task DeleteEndpointAsync(Guid endpointID)
        {
            await _endpointInternalRepo.DeleteEndpointAsync(endpointID);
        }

        public async Task<EndpointDTO> GetEndpointAsync(string path, HttpMethods method)
        {
            Endpoint endpoint = await _endpointInternalRepo.GetEndpointAsync(path, method);
            EndpointDTO endpointDTO = new EndpointDTO(endpoint);
            if (endpoint.ResponseObject.HasValue)
            {
                ResponseObject responseObject = await _responseObjectInternalRepo.GetResponseObjectAsync(endpoint.ResponseObject.Value);

                if (endpoint.RandomizeResponse)
                {
                    responseObject = RandomizeResponseObject(responseObject);
                }
                endpointDTO.ResponseObject = responseObject;
            }
            return endpointDTO;
        }

        public async Task<IEnumerable<Endpoint>> GetEndpointsAsync()
        {
            return await _endpointInternalRepo.GetEndpointsAsync();
        }

        public async Task UpdateEndpointAsync(Endpoint endpoint)
        {
            await _endpointInternalRepo.UpdateEndpointAsync(endpoint);
        }

        private ResponseObject RandomizeResponseObject(ResponseObject responseObject)
        {
            foreach (var field in responseObject.Data.Values)
            {
                object value = field.Value;
                if (value is string)
                {
                    field.Value = WhatKindOfString(field.Value);
                }
                else if (value is int)
                {
                    field.Value = new Random().Next(0, 100);
                }
                else if (value is bool)
                {
                    field.Value = new Random().Next(0, 1) == 1;
                }
                else if (value is double)
                {
                    field.Value = new Random().NextDouble();
                }
                else if (value is DateTime)
                {
                    field.Value = DateTime.Now;
                }
            }
            return responseObject;
        }

        private object WhatKindOfString(object value)
        {
            string stringValue = (string)value;
            return stringValue;
        }
    }
}
