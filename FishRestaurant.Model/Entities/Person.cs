using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishRestaurant.Model.Entities
{
    public class Person
    {
        public Person()
        {
            Installments = new List<Installment>();
            Transactions = new List<Transaction>();
        }   
        public int Id { get; set; }
        [Required, MaxLength(500), Column(TypeName = "varchar")]
        public string Name { get; set; }
        [MaxLength(1000), Column(TypeName = "varchar")]
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public decimal Balance { get; set; }
        public PersonTypes Type { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Installment> Installments { get; set; }
    }
}
