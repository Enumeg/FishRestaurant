using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class OutcomeType
    {
        public OutcomeType()
        {           
            Types = new List<Outcome>();        
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Outcome> Types { get; set; }

    }


    
}
