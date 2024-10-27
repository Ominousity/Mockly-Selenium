using Infrastructure.Enums;

namespace Infrastructure.Entities
{
    public class Endpoint
    {
        public Guid Id { get; set; } = Guid.Empty;
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required HttpMethods Method { get; set; }
        public int Delay { get; set; } = 0;
        public bool shouldFail { get; set; } = false;
        public Guid? ResponseObject { get; set; }
        public bool RandomizeResponse { get; set; } = false;
    }

    public class EndpointDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public string Path { get; set; }
        public HttpMethods Method { get; set; }
        public int Delay { get; set; } = 0;
        public ResponseObject? ResponseObject { get; set; }

        public EndpointDTO(Endpoint endpoint)
        {
            Id = endpoint.Id;
            Name = endpoint.Name;
            Path = endpoint.Path;
            Method = endpoint.Method;
            Delay = endpoint.Delay;
        }
    }
}
