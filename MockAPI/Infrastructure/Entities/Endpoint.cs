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
        public string? MockResponse { get; set; }
    }
}
