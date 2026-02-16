namespace Core.Models.Business
{
    public class Student
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}