using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Identity
{
    public class Gender
    {
        public enum Sex { Unknown, Female, Male, Other };

        private Sex sexComponent = Sex.Unknown;
        public Sex SexComponent
        {
            get { return this.sexComponent; }
            set
            {
                this.sexComponent = value;
                OnPropertyChanged();
            }
        }

        private string identityComponent = null;
        public string IndentityComponent
        {
            get { return this.identityComponent; }
            set
            {
                this.identityComponent = value;
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
