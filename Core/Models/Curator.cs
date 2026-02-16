namespace Core.Models.Business
{
    public class Curator
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}