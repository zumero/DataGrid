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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zumero;

namespace samples
{
    public class myRow : INotifyPropertyChanged
    {
        private double x = 0.0;
        public event PropertyChangedEventHandler PropertyChanged;

        public double XSquared
        {
            get { return x * x; }
        }

        public double X
        {
            get { return x; }
			set {
				if (x != value) {
					x = value;
					if (PropertyChanged != null) {
						PropertyChanged (this, new PropertyChangedEventArgs ("X"));
						PropertyChanged (this, new PropertyChangedEventArgs ("XSquared"));
					}
				}
			}
        }
    }

    class XSquaredCode : ContentPage
    {
    
        private DataGrid dg = null;
        public XSquaredCode()
        {
            dg = new DataGrid()
            {
                BackgroundColor = Xamarin.Forms.Color.Black,
                RowHeight = 80,

                Columns = new List<Column> {
					new Column {
						Width = 80,
						HeaderView = new Label {
							Text = "X",
							BackgroundColor = Color.Gray,
							XAlign = TextAlignment.Center,
							YAlign = TextAlignment.Center,
						},
						Template = new DataTemplate (() => {
							var v = new Label {
								BackgroundColor = Color.White,
								TextColor = Color.Black,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							};
							v.SetBinding (Label.TextProperty, "X");
							return v;
						}),
					},
					new Column {
						Width = 80,
						HeaderView = new Label {
							Text = "X^2",
							BackgroundColor = Color.Gray,
							XAlign = TextAlignment.Center,
							YAlign = TextAlignment.Center,
						},
						Template = new DataTemplate (() => {
							var v = new Label {
								BackgroundColor = Color.White,
								TextColor = Color.Black,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							};
							v.SetBinding (Label.TextProperty, "XSquared");
							return v;
						}),
					},
					new Column {
						Width = 120,
						HeaderView = new Label {
							Text = "Slider",
							BackgroundColor = Color.Gray,
							XAlign = TextAlignment.Center,
							YAlign = TextAlignment.Center,
						},
						Template = new DataTemplate (() => {
							var s = new Slider
							{

								VerticalOptions = LayoutOptions.Center,
								BackgroundColor = Color.White,
								Minimum = -20,
								Maximum = 20,
							};
							//The slider is wrapped in a ContentView, because Android sliders
							//do not cope well with having their height set by the 
							//grid control.
							var v = new ContentView
							{
								BackgroundColor = Color.White,
								Content = s
							};
							s.SetBinding (Slider.ValueProperty, "X", BindingMode.TwoWay);
							return v;
						}),
					},
                }
            };
            
            this.Content = dg;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (dg.Rows == null)
                dg.Rows = new ObservableCollection<object> {
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
