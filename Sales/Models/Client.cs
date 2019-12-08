using Sales.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales.Models
{
    public class Client : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        [Required(ErrorMessage ="اسم العميل مطلوب")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _telephone;
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private decimal? _accountStart;
        public decimal? AccountStart
        {
            get { return _accountStart; }
            set { SetProperty(ref _accountStart, value); }
        }

        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Supply> Supplies { get; set; }
    }
    public class ClientVM : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _telephone;
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private decimal? _account;
        public decimal? Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private decimal? _clientAccounts;
        public decimal? ClientAccounts
        {
            get { return _clientAccounts; }
            set { SetProperty(ref _clientAccounts, value); }
        }

    }
}
