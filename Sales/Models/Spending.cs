using Sales.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class Spending : ValidatableBindableBase
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

        private string _statement;
        [Required(ErrorMessage = "البيان مطلوب")]
        public string Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

        private decimal? _amount;
        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من صفر")]
        public decimal? Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
    }
}
