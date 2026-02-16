using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseModels.Models.Database
{
    public class GroupDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public DateTime CreationDate { get; set; }

        public virtual ICollection<StudentDb> Students { get; set; } = new List<StudentDb>();
        public virtual CuratorDb? Curator { get; set; }
    }
}