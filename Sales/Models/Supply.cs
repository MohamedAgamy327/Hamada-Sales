using Sales.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class Supply : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _clientID;
        [Required]
        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
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
        [Required]
        [Range(0.01,double.MaxValue)]
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
                OnPropertyChanged("Change");
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _totalDiscount;
        public decimal? TotalDiscount
        {
            get { return _totalDiscount; }
            set { SetProperty(ref _totalDiscount, value); }
        }

        private decimal? _oldDebt;
        [Required]
        public decimal? OldDebt
        {
            get { return _oldDebt; }
            set
            {
                SetProperty(ref _oldDebt, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _cashPaid;
        [Required (ErrorMessage ="المدفوع نقداً مطلوب")]
        public decimal? CashPaid
        {
            get { return _cashPaid; }
            set
            {
                SetProperty(ref _cashPaid, value);
                OnPropertyChanged("Change");
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _discountPaid;
        [Required(ErrorMessage = "المدفوع من خلال الخصم مطلوب")]
        public decimal? DiscountPaid
        {
            get { return _discountPaid; }
            set
            {
                SetProperty(ref _discountPaid, value);
                OnPropertyChanged("Change");
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _change;
        public decimal? Change
        {
            get { return _change = CostAfterTax - (CashPaid + DiscountPaid); }
            set
            {
                SetProperty(ref _change, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _newDebt;
        public decimal? NewDebt
        {
            get { return _newDebt = OldDebt + Change; }
            set { SetProperty(ref _newDebt, value); }
        }

        private bool _future;
        public bool Future
        {
            get { return _future; }
            set { SetProperty(ref _future, value); }
        }

        public virtual Client Client { get; set; }
        public virtual SupplyFuture SupplyFuture { get; set; }
        public virtual ICollection<SupplyCategory> SupplyCategories { get; set; }
        public virtual ICollection<SupplyRecall> SupplyRecalls { get; set; }
        public virtual ICollection<SupplyPremium> SupplyPremiums { get; set; }
    }
}
