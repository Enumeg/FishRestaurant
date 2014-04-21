using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class TransactionDetail : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        [NotMapped]
        public decimal Total
        {
            get { return Math.Round(Price * Amount, 2); }
            set { if (this.Total != value) this.Total = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
