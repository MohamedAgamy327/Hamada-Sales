using Sales.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class SaleOfferCategory : ValidatableBindableBase
    {
        private int _categoryID;
        [Key, Column(Order = 1)]
        public int CategoryID
        {
            get { return _categoryID; }
            set { SetProperty(ref _categoryID, value); }
        }

        private int _saleOfferID;
        [Key, Column(Order = 2)]
        public int SaleOfferID
        {
            get { return _saleOfferID; }
            set { SetProperty(ref _saleOfferID, value); }
        }

        private decimal? _qty;
        public decimal? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _price;
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
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

        private decimal? _priceAfterDiscount;
        public decimal? PriceAfterDiscount
        {
            get { return _priceAfterDiscount; }
            set { SetProperty(ref _priceAfterDiscount, value); }
        }

        private decimal? _priceTolal;
        public decimal? PriceTotal
        {
            get { return _priceTolal; }
            set { SetProperty(ref _priceTolal, value); }
        }

        private decimal? _priceTotalAfterDiscount;
        public decimal? PriceTotalAfterDiscount
        {
            get { return _priceTotalAfterDiscount; }
            set { SetProperty(ref _priceTotalAfterDiscount, value); }
        }

        public virtual SaleOffer SaleOffer { get; set; }
        public virtual Category Category { get; set; }
    }
    public class SaleOfferCategoryVM : ValidatableBindableBase
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

        private int _saleOfferID;
        public int SaleOfferID
        {
            get { return _saleOfferID; }
            set { SetProperty(ref _saleOfferID, value); }
        }

        private decimal? _qty;
        [Required(ErrorMessage = "الكمية مطلوبة")]
        public decimal? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _price;
        [Required(ErrorMessage = "السعر مطلوب")]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private decimal? _priceTolal;
        public decimal? PriceTotal
        {
            get { return _priceTolal = Qty * Price; }
            set { SetProperty(ref _priceTolal, value); }
        }

        private decimal? _discount;
        [Required(ErrorMessage = "الخصم مطلوب")]
        public decimal? Discount
        {
            get { return _discount; }
            set { SetProperty(ref _discount, value); }
        }

        private decimal? _discountValue;
        public decimal? DiscountValue
        {
            get { return _discountValue = (Price * Discount) / 100; }
            set { SetProperty(ref _discountValue, value); }
        }

        private decimal? _discountValueTotal;
        public decimal? DiscountValueTotal
        {
            get { return _discountValueTotal = DiscountValue * Qty; }
            set { SetProperty(ref _discountValueTotal, value); }
        }

        private decimal? _priceAfterDiscount;
        public decimal? PriceAfterDiscount
        {
            get { return _priceAfterDiscount = Price - ((Discount * Price) / 100); }
            set { SetProperty(ref _priceAfterDiscount, value); }
        }

        private decimal? _priceTotalAfterDiscount;
        public decimal? PriceTotalAfterDiscount
        {
            get { return _priceTotalAfterDiscount = PriceAfterDiscount * Qty; }
            set { SetProperty(ref _priceTotalAfterDiscount, value); }
        }
    }
}
