using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Entities
{
    public class ResponseObject
    {
        public Guid Id { get; set; }
        public Dictionary<string, ObjectType> Data { get; set; } = new Dictionary<string, ObjectType>();

        public ResponseObject()
        {
            Id = Guid.NewGuid();
            Data = new Dictionary<string, ObjectType>();
        }

        public static byte[] SerializeHeaders(Dictionary<string, ObjectType> headers)
        {
            var json = JsonSerializer.Serialize(headers);
            return Encoding.UTF8.GetBytes(json);
        }

        public static Dictionary<string, ObjectType> DeserializeHeaders(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<Dictionary<string, ObjectType>>(json)
                   ?? new Dictionary<string, ObjectType>();
        }

        public static ResponseObject FromJson(string json)
        {
            var responseObject = new ResponseObject();
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            if (dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    responseObject.Data[kvp.Key] = ObjectType.Create(kvp.Value);
                }
            }

            return responseObject;
        }

        public string ToJson()
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var kvp in Data)
            {
                dictionary[kvp.Key] = kvp.Value.Return();
            }

            return JsonSerializer.Serialize(dictionary);
        }
    }

    public class ObjectType
    {
        public object Value { get; set; }

        [JsonIgnore] // Exclude Type from serialization
        public Type Type { get; set; }

        // Store the type name as a string for serialization
        public string TypeName
        {
            get => Type.FullName; // Full name with assembly for better type resolution
            set => Type = Type.GetType(value) ?? throw new ArgumentException($"Invalid type: {value}");
        }

        // Parameterless constructor for deserialization
        public ObjectType()
        {
            Value = null;
            Type = typeof(object);
        }

        private ObjectType(object value, Type type)
        {
            Value = value;
            Type = type;
        }

        public static ObjectType Create(object value)
        {
            if (value == null)
            {
                return new ObjectType(null, typeof(object));
            }

            Type type = value.GetType();

            if (type == typeof(JsonElement))
            {
                JsonElement jsonElement = (JsonElement)value;

                return jsonElement.ValueKind switch
                {
                    JsonValueKind.Object => CreateNestedObject(jsonElement),
                    JsonValueKind.Array => CreateArray(jsonElement),
                    JsonValueKind.String => new ObjectType(jsonElement.GetString(), typeof(string)),
                    JsonValueKind.Number => CreateNumber(jsonElement),
                    JsonValueKind.True or JsonValueKind.False => new ObjectType(jsonElement.GetBoolean(), typeof(bool)),
                    _ => new ObjectType(null, typeof(object))
                };
            }

            return new ObjectType(value, type);
        }

        private static ObjectType CreateNestedObject(JsonElement jsonElement)
        {
            var nestedDict = new Dictionary<string, ObjectType>();

            foreach (var property in jsonElement.EnumerateObject())
            {
                nestedDict[property.Name] = Create(property.Value);
            }

            return new ObjectType(nestedDict, typeof(Dictionary<string, ObjectType>));
        }

        private static ObjectType CreateArray(JsonElement jsonElement)
        {
            var elements = new List<ObjectType>();

            foreach (var element in jsonElement.EnumerateArray())
            {
                elements.Add(Create(element));
            }

            return new ObjectType(elements, typeof(List<ObjectType>));
        }

        private static ObjectType CreateNumber(JsonElement jsonElement)
        {
            if (jsonElement.TryGetInt32(out int intValue))
            {
                return new ObjectType(intValue, typeof(int));
            }
            if (jsonElement.TryGetDouble(out double doubleValue))
            {
                return new ObjectType(doubleValue, typeof(double));
            }

            return new ObjectType(null, typeof(object));
        }

        public object? Return()
        {
            // Handle IConvertible types (like int, string, etc.)
            if (Value is IConvertible || Value == null)
            {
                return Convert.ChangeType(Value, Type);
            }
            // Handle Dictionary<string, ObjectType> specifically
            if (Value is Dictionary<string, ObjectType> dictionary)
            {
                var convertedDict = new Dictionary<string, object>();
                foreach (var kvp in dictionary)
                {
                    convertedDict[kvp.Key] = kvp.Value.Return();
                }
                return convertedDict;
            }
            // Handle List<ObjectType> specifically
            if (Value is List<ObjectType> list)
            {
                var convertedList = new List<object>();
                foreach (var item in list)
                {
                    convertedList.Add(item.Return());
                }
                return convertedList;
            }

            if (Value is ObjectType[] array)
            {
                var convertedArray = new List<object>();
                foreach (var item in array)
                {
                    convertedArray.Add(item.Return());
                }
                return convertedArray.ToArray();
            }

            // If the type is unhandled, return Value directly
            return Value;
        }
    }
}
