using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Identity
{
    public enum Tag { None, Home, Work, Individual };

    public class TypedParameter<T> : INotifyPropertyChanged
    {
        private T value;
        public T Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        private Tag tag;
        public Tag Tag
        {
            get { return this.tag; }
            set
            {
                this.tag = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
