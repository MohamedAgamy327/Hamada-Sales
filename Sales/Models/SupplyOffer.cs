using Sales.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class SupplyOffer: ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _clientID;
        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

        private DateTime _date;
        [Column(TypeName = "Date")]
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private decimal? _cost;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }

        private decimal? _costAfterTax;
        public decimal? CostAfterTax
        {
            get { return _costAfterTax; }
            set
            {
                SetProperty(ref _costAfterTax, value);
            }
        }

        private decimal? _totalDiscount;
        public decimal? TotalDiscount
        {
            get { return _totalDiscount; }
            set { SetProperty(ref _totalDiscount, value); }
        }

        public virtual Client Client { get; set; }
        public virtual ICollection<SupplyOfferCategory> SupplyOfferCategories { get; set; }
    }
}
