using System.ComponentModel.DataAnnotations;

namespace Hero_Api.Model
{
    public class Hero
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
