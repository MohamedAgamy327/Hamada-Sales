using Sales.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales.Models
{
    public class Company : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        [Required(ErrorMessage ="اسم الشركة مطلوب")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
