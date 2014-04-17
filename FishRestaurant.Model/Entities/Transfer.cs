using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Transfer
    {
        public Transfer()
        {
            TransferDetails = new ObservableCollection<TransferDetail>();
        }
        public int Id { get; set; }
        public int Number { get; set; }        
        public DateTime Date { get; set; }
        public Transaction_Types Type { get; set; }
        public virtual ICollection<TransferDetail> TransferDetails { get; set; }
    }
}
