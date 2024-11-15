using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Entities
{
    public class ResponseObject
    {
        public Guid Id { get; set; }

        public byte[] Data { get; set; } = new byte[0];

        public ResponseObject()
        {
            Id = Guid.NewGuid();
        }

        public Dictionary<string, CustomObject> DeserializeData()
        {
            var json = Encoding.UTF8.GetString(Data);
            var d = JsonSerializer.Deserialize<Dictionary<string, CustomObject>>(json);
            return d;
        }

        public static ResponseObject FromJson(JsonElement json)
        {
            var data = new Dictionary<string, CustomObject>();
            var responseObject = new ResponseObject();
            responseObject.AddElementsToDictionary(JObject.Parse(json.ToString()), data, "");
            var dataToBeBinary = JsonSerializer.Serialize(data);
            responseObject.Data = Encoding.UTF8.GetBytes(dataToBeBinary);
            return responseObject;
        }

        public void AddElementsToDictionary(JToken token, Dictionary<string, CustomObject> dictionary, string parentPath)
        {
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    string newPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";
                    AddElementsToDictionary(property.Value, dictionary, newPath);
                }
            }
            else if (token is JArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    string newPath = $"{parentPath}[{i}]";
                    AddElementsToDictionary(array[i], dictionary, newPath);
                }
            }
            else
            {
                dictionary[parentPath] = CustomObject.Create(token.ToObject<object>());
            }
        }
    }


    public class CustomObject
    {
        public object Value { get; set; }

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

        public Dictionary<string, CustomObject> Data { get; set; }

        public ResponseObjectDto(ResponseObject responseObject)
        {
            Id = responseObject.Id;
            Data = responseObject.DeserializeData();
        }

        public string ToJson()
        {
            string ogJson = ConvertDictionaryToJObject(Data).ToString();
            return ogJson;
        }

        private JObject ConvertDictionaryToJObject(Dictionary<string, CustomObject> dictionary)
        {
            JObject jObject = new JObject();
            foreach (var kvp in dictionary)
            {
                AddElementToJObject(jObject, kvp.Key, kvp.Value.Value);
            }
            return jObject;
        }

        private void AddElementToJObject(JObject jObject, string path, object value)
        {
            string[] pathParts = path.Split('.');
            JToken currentToken = jObject;

            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                string part = pathParts[i];
                if (part.Contains("["))
                {
                    string[] arrayParts = part.Split('[', ']');
                    string arrayName = arrayParts[0];
                    int index = int.Parse(arrayParts[1]);

                    if (currentToken[arrayName] == null)
                    {
                        currentToken[arrayName] = new JArray();
                    }

                    JArray array = (JArray)currentToken[arrayName];
                    while (array.Count <= index)
                    {
                        array.Add(new JObject());
                    }

                    currentToken = array[index];
                }
                else
                {
                    if (currentToken[part] == null)
                    {
                        currentToken[part] = new JObject();
                    }

                    currentToken = currentToken[part];
                }
            }

            string lastPart = pathParts[^1];
            if (lastPart.Contains("["))
            {
                string[] arrayParts = lastPart.Split('[', ']');
                string arrayName = arrayParts[0];
                int index = int.Parse(arrayParts[1]);

                if (currentToken[arrayName] == null)
                {
                    currentToken[arrayName] = new JArray();
                }

                JArray array = (JArray)currentToken[arrayName];
                while (array.Count <= index)
                {
                    array.Add(new JObject());
                }

                array[index] = JToken.FromObject(value);
            }
            else
            {
                currentToken[lastPart] = JToken.FromObject(value);
            }
        }
    }
}
