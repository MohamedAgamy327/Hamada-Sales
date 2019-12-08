using Sales.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Models
{
    public class User : ValidatableBindableBase
    {
        private string _id;
        [Required(ErrorMessage ="اسم المستخدم مطلوب")]
        public string ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _password;
        [Required(ErrorMessage ="الرقم السرى مطلوب")]
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
    }
}
