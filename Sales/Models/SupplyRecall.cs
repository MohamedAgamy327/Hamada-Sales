using Sales.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class SupplyRecall : ValidatableBindableBase
    {
        private DateTime _registrationDate;
        [Key]
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

        private int _categoryID;
        public int CategoryID
        {
            get { return _categoryID; }
            set { SetProperty(ref _categoryID, value); }
        }

        private int _supplyID;
        public int SupplyID
        {
            get { return _supplyID; }
            set { SetProperty(ref _supplyID, value); }
        }

        private decimal? _qty;
        public decimal? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _costAfterTax;
        public decimal? CostAfterTax
        {
            get { return _costAfterTax; }
            set { SetProperty(ref _costAfterTax, value); }
        }

        private decimal? _costTotalAfterTax;
        public decimal? CostTotalAfterTax
        {
            get { return _costTotalAfterTax; }
            set { SetProperty(ref _costTotalAfterTax, value); }
        }

        private decimal? _discountValue;
        public decimal? DiscountValue
        {
            get { return _discountValue; }
            set { SetProperty(ref _discountValue, value); }
        }

        private decimal? _discountValueTotal;
        public decimal? DiscountValueTotal
        {
            get { return _discountValueTotal; }
            set { SetProperty(ref _discountValueTotal, value); }
        }

        private decimal? _costAfterDiscount;
        public decimal? CostAfterDiscount
        {
            get { return _costAfterDiscount; }
            set { SetProperty(ref _costAfterDiscount, value); }
        }

        private decimal? _costTotalAfterDiscount;
        public decimal? CostTotalAfterDiscount
        {
            get { return _costTotalAfterDiscount; }
            set { SetProperty(ref _costTotalAfterDiscount, value); }
        }

        public virtual Supply Supply { get; set; }
        public virtual Category Category { get; set; }
    }
    public class SupplyRecallVM : ValidatableBindableBase
    {
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

        private int _categoryID;
        public int CategoryID
        {
            get { return _categoryID; }
            set { SetProperty(ref _categoryID, value); }
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        private int _supplyID;
        public int SupplyID
        {
            get { return _supplyID; }
            set { SetProperty(ref _supplyID, value); }
        }

        private decimal? _qty;
        [Required(ErrorMessage = "الكمية مطلوبة")]
        public decimal? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _costAfterTax;
        public decimal? CostAfterTax
        {
            get { return _costAfterTax; }
            set { SetProperty(ref _costAfterTax, value); }
        }

        private decimal? _costTotalAfterTax;
        public decimal? CostTotalAfterTax
        {
            get { return _costTotalAfterTax = CostAfterTax * Qty; }
            set { SetProperty(ref _costTotalAfterTax, value); }
        }

        private decimal? _discountValue;
        public decimal? DiscountValue
        {
            get { return _discountValue; }
            set { SetProperty(ref _discountValue, value); }
        }

        private decimal? _discountValueTotal;
        public decimal? DiscountValueTotal
        {
            get { return _discountValueTotal = Qty * DiscountValue; }
            set { SetProperty(ref _discountValueTotal, value); }
        }

        private decimal? _costAfterDiscount;
        public decimal? CostAfterDiscount
        {
            get { return _costAfterDiscount; }
            set { SetProperty(ref _costAfterDiscount, value); }
        }

        private decimal? _costTotalAfterDiscount;
        [Required]
        public decimal? CostTotalAfterDiscount
        {
            get { return _costTotalAfterDiscount = CostAfterDiscount * Qty; }
            set { SetProperty(ref _costTotalAfterDiscount, value); }
        }
    }

}
