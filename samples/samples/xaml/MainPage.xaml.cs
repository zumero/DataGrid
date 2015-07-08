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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace samples.xaml
{
	public partial class MainPage
	{
		class SampleObject
		{
			public string SampleName { get; set; }
			public string SampleDescription { get; set; }

			public Func<Page> OpenCode { get; set; }
			public Func<Page> OpenXaml { get; set; }
				
		}
		public MainPage()
		{
			InitializeComponent();
		}

		void OnCodeButtonClicked(object sender, EventArgs args)
		{
			Button button = (Button)sender;
			this.Navigation.PushAsync(((SampleObject)button.BindingContext).OpenCode());
		}

		void OnXamlButtonClicked(object sender, EventArgs args)
		{
			Button button = (Button)sender;
		    this.Navigation.PushAsync(((SampleObject)button.BindingContext).OpenXaml());
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			datagrid.Rows = new ObservableCollection<object>
			{
				new SampleObject
				{
					SampleName = "Presidents",
					SampleDescription = "Shows images, dates. Includes headers and a frozen column.",
					OpenCode = () => { return new PresidentsCode(); },
					OpenXaml = () => { return new PresidentsXaml(); },
				},
				new SampleObject
				{
					SampleName = "Countries",
					SampleDescription = "Lots of rows. Unicode text.",
					OpenCode = () => { return new CountriesCode(); },
					OpenXaml = () => { return new CountriesXaml(); },
				},
				new SampleObject
				{
					SampleName = "Dynamic",
					SampleDescription = "Dynamic generation of one million 'virtual' rows.",
					OpenCode = () => { return new DynamicCode(); },
					OpenXaml = () => { return new DynamicXaml(); },
				},
				new SampleObject
				{
					SampleName = "XSquared",
					SampleDescription = "Includes slider controls with two way binding.",
					OpenCode = () => { return new XSquaredCode(); },
					OpenXaml = () => { return new XSquaredXaml(); },
				},
			};
		}
	}
}
