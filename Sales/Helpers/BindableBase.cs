using GalaSoft.MvvmLight;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sales.Helpers
{
    public class BindableBase : ViewModelBase, INotifyPropertyChanged
    {
        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
#pragma warning disable CS0108 // 'BindableBase.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.
        public event PropertyChangedEventHandler  PropertyChanged =  delegate { };
#pragma warning restore CS0108 // 'BindableBase.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.

    }
}
