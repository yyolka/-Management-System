using System;
namespace Core.Models.Business

{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}