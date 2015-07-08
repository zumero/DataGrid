/*
   Copyright 2014-2015 Zumero, LLC

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace demo.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public bool NotifyIfPropertiesChange = true;
		
        public event Xamarin.Forms.PropertyChangingEventHandler PropertyChanging;

        public void OnPropertyChanging(string propertyName)
        {
            if (NotifyIfPropertiesChange == false || PropertyChanging == null)
                return;

            PropertyChanging(this, new Xamarin.Forms.PropertyChangingEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (NotifyIfPropertiesChange == false || PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {


            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }
		
		protected void SetProperty<U>(
            U backingStore, U value,
			Action<U> performChange,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {


            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            if (performChange != null)
                performChange(value);

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }
    }
}
