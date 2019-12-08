using Sales.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class SaleOffer : ValidatableBindableBase
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

        private decimal? _price;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private decimal? _priceAfterDiscount;
        [Required]
        public decimal? PriceAfterDiscount
        {
            get { return _priceAfterDiscount; }
            set
            {
                SetProperty(ref _priceAfterDiscount, value);
            }
        }

        public virtual Client Client { get; set; }
        public virtual ICollection<SaleOfferCategory> SaleOfferCategories { get; set; }

    }
}
