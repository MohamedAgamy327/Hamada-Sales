using Sales.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class Sale : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _printingMan;
        public string PrintingMan
        {
            get { return _printingMan; }
            set { SetProperty(ref _printingMan, value); }
        }

        private int _clientID;
        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

        private int _salespersonID;
        public int SalespersonID
        {
            get { return _salespersonID; }
            set { SetProperty(ref _salespersonID, value); }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { SetProperty(ref _registrationDate, value); }
        }

        private DateTime _date;
        [Column(TypeName = "Date")]
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private decimal? _cost;
        public decimal? Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }

        private decimal? _price;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private decimal? _priceAfterDiscount;//اجمالى الفاتورة
        [Required]
        public decimal? PriceAfterDiscount
        {
            get { return _priceAfterDiscount; }
            set
            {
                SetProperty(ref _priceAfterDiscount, value);
                OnPropertyChanged("PriceTotal");
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _transportCost;//مصاريف النقل
        [Required(ErrorMessage ="مصاريف النقل مطلوبة")]
        public decimal? TransportCost
        {
            get { return _transportCost; }
            set
            {
                SetProperty(ref _transportCost, value);
                OnPropertyChanged("PriceTotal");
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _oldDebt;//المتبقى من قبل
        [Required]
        public decimal? OldDebt
        {
            get { return _oldDebt; }
            set
            {
                SetProperty(ref _oldDebt, value);
                OnPropertyChanged("PriceTotal");
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _priceTotal; // اجمالى المطلوب
        public decimal? PriceTotal
        {
            get { return _priceTotal = PriceAfterDiscount + TransportCost - OldDebt; }
            set
            {
                SetProperty(ref _priceTotal, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _paid;//ما تم سداده
        [Required(ErrorMessage ="ما تم سداده مطلوب")]
        public decimal? Paid
        {
            get { return _paid; }
            set
            {
                SetProperty(ref _paid, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _newDebt;
        public decimal? NewDebt
        {
            get { return _newDebt = Paid - PriceTotal; }
            set { SetProperty(ref _newDebt, value); }
        }

        public virtual Client Client { get; set; }
        public virtual Salesperson Salesperson { get; set; }
        public virtual ICollection<SaleCategory> SaleCategories { get; set; }
        public virtual ICollection<SaleRecall> SaleRecalls { get; set; }

    }
}
