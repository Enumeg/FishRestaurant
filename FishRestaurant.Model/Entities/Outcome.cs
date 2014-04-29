using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    class Outcome
    {

        public Outcome()
        { 
         


        }


        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Value { get; set; }


        public string Description { get; set; }


        public int OutcomeTypesId { get; set; }

        public virtual OutcomeType OutcomeType { get; set; }

    }

       

}
