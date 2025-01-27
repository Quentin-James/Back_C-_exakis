using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("firstName")]
        public required string FirstName { get; set; }

        [Column("lastName")]
        public required string LastName { get; set; }
    }
}