using System.ComponentModel;

namespace Thunder.Standard.Lib.Model
{
    public class NotifyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void PropertyUpdate(object sender, string PropertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(PropertyName));
        }

    }
}
