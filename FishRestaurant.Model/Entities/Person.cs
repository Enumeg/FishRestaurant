using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishRestaurant.Model.Entities
{
    public class Person
    {
        public int Id { get; set; }
        [Required, MaxLength(500), Column(TypeName = "varchar")]
        public string Name { get; set; }
        [MaxLength(1000), Column(TypeName = "varchar")]
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
    }
}
