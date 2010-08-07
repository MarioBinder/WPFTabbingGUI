using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WPFTabbingGUI.Common
{
/// <summary>
/// the viewmodelbase implemented INotifyPropertyChanged
/// with type-save INotifyProperty
/// idea from jp.hamilton: http://www.jphamilton.net/blog/post/MVVM-with-Type-Safe-INotifyPropertyChanged.aspx
/// </summary>
/// <typeparam name="T"></typeparam>
public class ViewModelBase<T> : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged<R>(Expression<Func<T, R>> x)
    {
        var body = x.Body as MemberExpression;
        if (body == null)
            throw new ArgumentException("'x' should be a member expression");

        string propertyName = body.Member.Name;

        PropertyChangedEventHandler handler = this.PropertyChanged;

        if (handler != null)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}
}


