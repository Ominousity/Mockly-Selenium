using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Entities
{
    public class ResponseObject
    {
        public Guid Id { get; set; }

        // Store the raw byte array instead of a Dictionary directly
        public byte[] Data { get; set; } = new byte[0];

        public ResponseObject()
        {
            Id = Guid.NewGuid();
        }

        // Serialize Data to JSON for database storage as a byte array
        public void SerializeData(Dictionary<string, CustomObject> data)
        {
            var json = JsonSerializer.Serialize(data);
            Data = Encoding.UTF8.GetBytes(json);
        }

        // Deserialize JSON back to Dictionary for in-memory use
        public Dictionary<string, CustomObject> DeserializeData()
        {
            var json = Encoding.UTF8.GetString(Data);
            return JsonSerializer.Deserialize<Dictionary<string, CustomObject>>(json) ?? new Dictionary<string, CustomObject>();
        }

        public string ToJson()
        {
            return Encoding.UTF8.GetString(Data);
        }

        public static ResponseObject FromJson(JsonElement json)
        {
            var data = json.ToString();
            return new ResponseObject { Data = Encoding.UTF8.GetBytes(json.ToString()) };
        }
    }

    // Wrapper class to hold any type of value with explicit type information
    public class CustomObject
    {
        public object Value { get; set; }

        // Serialize type name for reference
        public string TypeName { get; set; }

        public CustomObject() { }

        public CustomObject(object value)
        {
            Value = value;
            TypeName = value?.GetType().AssemblyQualifiedName;
        }

        public static CustomObject Create(object value)
        {
            return new CustomObject(value);
        }

        public object? GetTypedValue()
        {
            if (Value == null) return null;

            if (Type.GetType(TypeName) is { } type)
            {
                return Convert.ChangeType(Value, type);
            }

            return Value;
        }
    }

    public class ResponseObjectDto
    {
        public Guid Id { get; set; }

        // Data as a Dictionary, suitable for JSON serialization
        public Dictionary<string, CustomObject> Data { get; set; }

        // Constructor that takes in the original ResponseObject entity
        public ResponseObjectDto(ResponseObject responseObject)
        {
            Id = responseObject.Id;
            Data = responseObject.DeserializeData();
        }

        // Method to convert DTO to ResponseObject
        public ResponseObject ToEntity()
        {
            var responseObject = new ResponseObject
            {
                Id = this.Id
            };
            responseObject.SerializeData(this.Data);
            return responseObject;
        }
    }
}
