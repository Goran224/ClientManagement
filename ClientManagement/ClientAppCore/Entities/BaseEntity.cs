using System.ComponentModel.DataAnnotations;

namespace ClientAppCore.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
