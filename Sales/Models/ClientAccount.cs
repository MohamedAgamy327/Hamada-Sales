using Sales.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class ClientAccount : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
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

        private int _clientID;
        [Required(ErrorMessage ="العميل مطلوب")]
        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

        private string _statement;
        public string Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private string _details;
        public string Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }

        private decimal? _debit;
        public decimal? Debit
        {
            get { return _debit; }
            set { SetProperty(ref _debit, value); }
        }

        private decimal? _credit;
        public decimal? Credit
        {
            get { return _credit; }
            set { SetProperty(ref _credit, value); }
        }

        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        public virtual Client Client { get; set; }

    }

    public class ClientAccountVM : ValidatableBindableBase
    {
        private decimal? _withoutTotalDebit;
        public decimal? WithoutTotalDebit
        {
            get { return _withoutTotalDebit; }
            set { SetProperty(ref _withoutTotalDebit, value); }
        }

        private decimal? _withoutTotalCredit;
        public decimal? WithoutTotalCredit
        {
            get { return _withoutTotalCredit; }
            set { SetProperty(ref _withoutTotalCredit, value); }
        }

        private decimal? _withoutDuringAccount;
        public decimal? WithoutDuringAccount
        {
            get { return _withoutDuringAccount; }
            set { SetProperty(ref _withoutDuringAccount, value); }
        }

        private decimal? _withoutOldAccount;
        public decimal? WithoutOldAccount
        {
            get { return _withoutOldAccount; }
            set { SetProperty(ref _withoutOldAccount, value); }
        }

        private decimal? _withoutCurrentAccount;
        public decimal? WithoutCurrentAccount
        {
            get { return _withoutCurrentAccount; }
            set { SetProperty(ref _withoutCurrentAccount, value); }
        }

        private decimal? _discountTotalDebit;
        public decimal? DiscountTotalDebit
        {
            get { return _discountTotalDebit; }
            set { SetProperty(ref _discountTotalDebit, value); }
        }

        private decimal? _discountTotalCredit;
        public decimal? DiscountTotalCredit
        {
            get { return _discountTotalCredit; }
            set { SetProperty(ref _discountTotalCredit, value); }
        }

        private decimal? _discountDuringAccount;
        public decimal? DiscountDuringAccount
        {
            get { return _discountDuringAccount; }
            set { SetProperty(ref _discountDuringAccount, value); }
        }

        private decimal? _discountOldAccount;
        public decimal? DiscountOldAccount
        {
            get { return _discountOldAccount; }
            set { SetProperty(ref _discountOldAccount, value); }
        }

        private decimal? _discountCurrentAccount;
        public decimal? DiscountCurrentAccount
        {
            get { return _discountCurrentAccount; }
            set { SetProperty(ref _discountCurrentAccount, value); }
        }

        private decimal? _currentAccount;
        public decimal? CurrentAccount
        {
            get { return _currentAccount; }
            set { SetProperty(ref _currentAccount, value); }
        }


    }
}
