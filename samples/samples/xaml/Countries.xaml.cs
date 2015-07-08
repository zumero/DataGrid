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
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace samples
{
    public partial class CountriesXaml : ContentPage
	{
        public static readonly BindableProperty DataRowsProperty =
            BindableProperty.Create<CountriesXaml, demo.Models.countries[]>(
                p => p.DataRows, null);

        public demo.Models.countries[] DataRows
        {
            get { return (demo.Models.countries[])GetValue(DataRowsProperty); }
            set { SetValue(DataRowsProperty, value); } // TODO disallow invalid values
        }

        public CountriesXaml()
		{
			InitializeComponent ();
            this.BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (DataRows == null)
                DataRows = await Task.Run(() =>
                {
                    return samples.SampleData.GetCountries();
                });
        }
	}
}

