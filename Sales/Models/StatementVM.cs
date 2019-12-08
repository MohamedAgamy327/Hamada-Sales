using Sales.Helpers;

namespace Sales.Models
{
    public class StatementVM : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _statement;
        public string Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

    }
}
