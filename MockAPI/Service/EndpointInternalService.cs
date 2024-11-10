using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
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
                ResponseObjectDto response = new ResponseObjectDto(responseObject);

                if (endpoint.RandomizeResponse)
                {
                    response = RandomizeResponseObject(response);
                }
                endpointDTO.ResponseObject = response;
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

        private ResponseObjectDto RandomizeResponseObject(ResponseObjectDto responseObject)
        {
            foreach (var field in responseObject.Data.Values)
            {
                object value = field.Value;
                field.Value = WhatKindOfString(value);
            }
            return responseObject;
        }

        private object WhatKindOfString(object value)
        {
            string stringValue = value.ToString();
            if (isDateTime(stringValue))
            {
                return DateTime.Now.ToString();
            }
            else if (isGuid(stringValue))
            {
                return Guid.NewGuid().ToString();
            }
            else if (isNumber(stringValue))
            {
                return new Random().Next(0, 100).ToString();
            }
            else if (isBoolean(stringValue))
            {
                return (new Random().Next(0, 1) == 1).ToString();
            }
            else if (isStreetAddress(stringValue))
            {
                return MassiveInfoReader.GetRandomStreetName();
            }
            else if (IsName(stringValue))
            {
                string firstName = MassiveInfoReader.GetRandomFirstName();
                string lastName = MassiveInfoReader.GetRandomLastName();
                string fullName = firstName + " " + lastName;
                return fullName;
            }
            return stringValue;
        }

        private bool isDateTime(string value)
        {
            DateTime dateTime;
            return DateTime.TryParse(value, out dateTime);
        }

        private bool isGuid(string value)
        {
            Guid guid;
            return Guid.TryParse(value, out guid);
        }

        private bool isNumber(string value)
        {
            int number;
            return int.TryParse(value, out number);
        }

        private bool isBoolean(string value)
        {
            bool boolean;
            return bool.TryParse(value, out boolean);
        }

        private bool isStreetAddress(string value)
        {
            return Regex.IsMatch(value, @"\b(?:Street|Avenue|Boulevard|Rd|Ln|St|Ave|Dr)\b",
                         RegexOptions.IgnoreCase);
        }

        private bool IsName(string input)
        {
            return Regex.IsMatch(input, @"^[A-Z][a-z]+$");
        }
    }
}
