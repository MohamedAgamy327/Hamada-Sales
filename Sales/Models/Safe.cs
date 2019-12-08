using Sales.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    public class Safe : ValidatableBindableBase
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
        public decimal? Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private int? _source;
        public int? Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }
        //1. الخزنة
        //2.المصاريف
        //3. فاتورة المشتريات
        //4. فاتورة المبيعات
        //5. دفع حساب
        //6.قبض حساب
        //7.دفع سلف
        //8.قبض سلف
        //9. دفع قسط
    }
}
