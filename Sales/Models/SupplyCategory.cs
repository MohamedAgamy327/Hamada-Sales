using Sales.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class SupplyCategory : ValidatableBindableBase
    {
        private int _categoryID;
        [Key, Column(Order = 1)]
        public int CategoryID
        {
            get { return _categoryID; }
            set { SetProperty(ref _categoryID, value); }
        }

        private int _supplyID;
        [Key, Column(Order = 2)]
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

        private decimal? _cost;
        public decimal? Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }

        private decimal? _price;
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private decimal? _costTolal;
        public decimal? CostTotal
        {
            get { return _costTolal; }
            set { SetProperty(ref _costTolal, value); }
        }

        private decimal? _tax;
        public decimal? Tax
        {
            get { return _tax; }
            set { SetProperty(ref _tax, value); }
        }

        private decimal? _taxvalue;
        public decimal? TaxValue
        {
            get { return _taxvalue; }
            set { SetProperty(ref _taxvalue, value); }
        }

        private decimal? _taxValueTotal;
        public decimal? TaxValueTotal
        {
            get { return _taxValueTotal; }
            set { SetProperty(ref _taxValueTotal, value); }
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

        private decimal? _discount;
        public decimal? Discount
        {
            get { return _discount; }
            set { SetProperty(ref _discount, value); }
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
    public class SupplyCategoryVM : ValidatableBindableBase
    {
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

        private string _company;
        public string Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
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

        private decimal? _cost;
        [Required(ErrorMessage = "التكلفة مطلوبة")]
        public decimal? Cost
        {
            get { return _cost; }
            set
            {
                SetProperty(ref _cost, value);
                OnPropertyChanged("CostAfterDiscount");
            }
        }

        private decimal? _price;
        [Required(ErrorMessage = "السعر  مطلوب")]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private decimal? _costTolal;
        public decimal? CostTotal
        {
            get { return _costTolal = Qty * Cost; }
            set { SetProperty(ref _costTolal, value); }
        }

        private decimal? _tax;
        [Required(ErrorMessage = "الإضافة مطلوبة")]
        public decimal? Tax
        {
            get { return _tax; }
            set
            {
                SetProperty(ref _tax, value);
                OnPropertyChanged("CostAfterDiscount");
            }
        }

        private decimal? _taxvalue;
        public decimal? TaxValue
        {
            get { return _taxvalue = (Tax * Cost) / 100; }
            set { SetProperty(ref _taxvalue, value); }
        }

        private decimal? _taxValueTotal;
        public decimal? TaxValueTotal
        {
            get { return _taxValueTotal = TaxValue * Qty; }
            set { SetProperty(ref _taxValueTotal, value); }
        }

        private decimal? _costAfterTax;
        public decimal? CostAfterTax
        {
            get { return _costAfterTax = ((Tax * Cost) / 100) + Cost; }
            set { SetProperty(ref _costAfterTax, value); }
        }

        private decimal? _costTotalAfterTax;
        public decimal? CostTotalAfterTax
        {
            get { return _costTotalAfterTax = CostAfterTax * Qty; }
            set { SetProperty(ref _costTotalAfterTax, value); }
        }

        private decimal? _discount;
        [Required(ErrorMessage = "الخصم مطلوب")]
        public decimal? Discount
        {
            get { return _discount; }
            set
            {
                SetProperty(ref _discount, value);
                OnPropertyChanged("CostAfterDiscount");
            }
        }

        private decimal? _discountValue;
        public decimal? DiscountValue
        {
            get { return _discountValue = (CostAfterTax * Discount) / 100; }
            set { SetProperty(ref _discountValue, value); }
        }

        private decimal? _discountValueTotal;
        public decimal? DiscountValueTotal
        {
            get { return _discountValueTotal = DiscountValue * Qty; }
            set { SetProperty(ref _discountValueTotal, value); }
        }

        private decimal? _costAfterDiscount;
        public decimal? CostAfterDiscount
        {
            get { return _costAfterDiscount = CostAfterTax - ((Discount * CostAfterTax) / 100); }
            set { SetProperty(ref _costAfterDiscount, value); }
        }

        private decimal? _costTotalAfterDiscount;
        public decimal? CostTotalAfterDiscount
        {
            get { return _costTotalAfterDiscount = CostAfterDiscount * Qty; }
            set { SetProperty(ref _costTotalAfterDiscount, value); }
        }

    }

}
