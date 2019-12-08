using Sales.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class SupplyFuture : ValidatableBindableBase
    {
        private int _supplyID;
        [Key, ForeignKey("Supply")]
        public int SupplyID
        {
            get { return _supplyID; }
            set { SetProperty(ref _supplyID, value); }
        }

        private DateTime _date;
        [Column(TypeName = "Date")]
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private decimal? _Change;
        public decimal? Change
        {
            get { return _Change; }
            set { SetProperty(ref _Change, value); }
        }

        private string _place;
        public string Place
        {
            get { return _place; }
            set { SetProperty(ref _place, value); }
        }

        private bool _cheque;
        public bool Cheque
        {
            get { return _cheque; }
            set { SetProperty(ref _cheque, value); }
        }

        private string _checqueNumber;
        public string  ChequeNumber
        {
            get { return _checqueNumber; }
            set { SetProperty(ref _checqueNumber, value); }
        }

        public virtual Supply Supply { get; set; }
    }
}
