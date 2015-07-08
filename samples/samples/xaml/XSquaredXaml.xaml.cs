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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace samples
{
    public partial class XSquaredXaml : ContentPage
	{
        public static readonly BindableProperty DataRowsProperty =
            BindableProperty.Create<XSquaredXaml, ObservableCollection<object>>(
                p => p.DataRows, null);

        public ObservableCollection<object> DataRows
        {
            get { return (ObservableCollection<object>)GetValue(DataRowsProperty); }
            set { SetValue(DataRowsProperty, value); } // TODO disallow invalid values
        }

        public XSquaredXaml()
		{
			InitializeComponent ();
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (DataRows == null)
                DataRows = new ObservableCollection<object> {
                        new myRow {X = 1},
                        new myRow {X = 2},
                        new myRow {X = 3},
                        new myRow {X = 4},
                        new myRow {X = 5},
                        new myRow {X = 6},
                        new myRow {X = 7},
                };
        }
	}
}

