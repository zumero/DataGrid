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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Zumero;

using Xamarin.Forms;

namespace demo
{
	public class myList : IList<object>
	{
		private class myRow
		{
			private readonly double v;

			public double X { get { return v; } }
			public double Sqrt { get { return Math.Round(Math.Sqrt(v),3); } }

			public myRow(double _v) { v = _v; }
		}

		private readonly int count;

		public myList(int _count)
		{
			count = _count;
		}

		public object this[int index]
		{
			get { return new myRow(index); }
			set { throw new NotImplementedException (); }
		}

		public int Count
		{
			get { return count; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public void Add(object item) { throw new NotImplementedException (); }
		public void Clear() { throw new NotImplementedException (); }
		public void Insert(int index, object item) { throw new NotImplementedException (); }
		public void RemoveAt(int index) { throw new NotImplementedException (); }
		public bool Remove(object item) { throw new NotImplementedException (); }

		public int IndexOf(object item) { throw new NotImplementedException (); }
		public bool Contains(object item) { throw new NotImplementedException (); }
		public void CopyTo(object[] array, int arrayIndex) { throw new NotImplementedException (); }

		private class myEnumerator : IEnumerator<object>
		{
			private int count;
			private int curIndex;

			public myEnumerator(int _count)
			{
				count = _count;
				curIndex = -1;
			}

			public bool MoveNext()
			{
				if (++curIndex >= count)
				{
					return false;
				}
				else
				{
				}
				return true;
			}

			public void Reset() { curIndex = -1; }

			void IDisposable.Dispose() { }

			public object Current
			{
				get { return new myRow(curIndex); }
			}


			object IEnumerator.Current
			{
				get { return Current; }
			}

		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator ();
		}

		public IEnumerator<object> GetEnumerator()
		{
			return new myEnumerator (count);
		}

	}

	public class myButton : Button
	{
		public myButton()
		{
			this.Clicked += (object sender, EventArgs e) => {
				Action a = OneAction;
				if (a != null) {
					a();
				}
			};
		}

		public static readonly BindableProperty OneActionProperty =
			BindableProperty.Create<myButton,Action>(
				p => p.OneAction, null);

		public Action OneAction {
			get { return (Action)GetValue(OneActionProperty); }
			set { SetValue(OneActionProperty, value); }
		}
	}

	public class ColumnHeader : BindableObject
	{
		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create<ColumnHeader,string>(
				p => p.Title, null);

		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
	}

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

	public class WordPair : BindableObject
	{
		public static readonly BindableProperty EnglishProperty =
			BindableProperty.Create<WordPair,string>(
				p => p.English, null);

		public string English {
			get { return (string)GetValue(EnglishProperty); }
			set { SetValue(EnglishProperty, value); }
		}

		public static readonly BindableProperty SpanishProperty =
			BindableProperty.Create<WordPair,string>(
				p => p.Spanish, null);

		public string Spanish {
			get { return (string)GetValue(SpanishProperty); }
			set { SetValue(SpanishProperty, value); }
		}

		public static readonly BindableProperty SpanishBackgroundColorProperty =
			BindableProperty.Create<WordPair,Color>(
				p => p.SpanishBackgroundColor, Color.White);

		public Color SpanishBackgroundColor {
			get { return (Color)GetValue(SpanishBackgroundColorProperty); }
			set { SetValue(SpanishBackgroundColorProperty, value); }
		}

		public static readonly BindableProperty ButtonTextProperty =
			BindableProperty.Create<WordPair,string>(
				p => p.ButtonText, null);

		public string ButtonText {
			get { return (string)GetValue(ButtonTextProperty); }
			set { SetValue(ButtonTextProperty, value); }
		}

		public static readonly BindableProperty ActionProperty =
			BindableProperty.Create<WordPair,Action>(
				p => p.Action, null);

		public Action Action {
			get { return (Action)GetValue(ActionProperty); }
			set { SetValue(ActionProperty, value); }
		}

		public static readonly BindableProperty ImageNameProperty =
			BindableProperty.Create<WordPair,string>(
				p => p.ImageName, null);

		public string ImageName {
			get { return (string)GetValue(ImageNameProperty); }
			set { SetValue(ImageNameProperty, value); }
		}

		public static readonly BindableProperty XProperty =
			BindableProperty.Create<WordPair,int>(
				p => p.X, 0);

		public int X {
			get { return (int)GetValue(XProperty); }
			set { SetValue(XProperty, value); }
		}

		public int DoubleX {
			get {
				return X * 2;
			}
		}

		public WordPair()
		{
			this.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == XProperty.PropertyName) {
					this.OnPropertyChanged("DoubleX");
				}
			};
		}
	}

	public class App : Application
	{
		public App ()
		{
			Dictionary<string, ContentPage> pages = new Dictionary<string, ContentPage> ();

			#if not
			pages["Test 1"] = new ContentPage {
				Content = new TabbyTest1() {
					BackgroundColor = Color.Yellow,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					ColumnSizes = new ObservableCollection<ColumnSize> {
						new ColumnSize { Width = 80, InternalGap = 4 },
						new ColumnSize { Width = 80, InternalGap = 0 },
						new ColumnSize { Width = 100, InternalGap = 0 },
						new ColumnSize { Width = 80, InternalGap = 0 },
						new ColumnSize { Width = 80, InternalGap = 0 },
					},
				}
			};
				
			pages["Test 2"] = new ContentPage {
				Content = new TabbyTest2() {
					BackgroundColor = Color.Black,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					ColumnSizes = new ObservableCollection<ColumnSize> {
						new ColumnSize { Width = 80, InternalGap = 2 },
						new ColumnSize { Width = 80, InternalGap = 2 },
						new ColumnSize { Width = 80, InternalGap = 2 },
					},
				}
			};

			var t3 = new TabbyTest3 () {
				BackgroundColor = Color.Black,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSizes = new ObservableCollection<ColumnSize> {
					new ColumnSize { Width = 80, InternalGap = 2 },
					new ColumnSize { Width = 80, InternalGap = 2 },
					new ColumnSize { Width = 80, InternalGap = 2 },
					new ColumnSize { Width = 80, InternalGap = 2 },
				},
			};

			var t3Button = new Button {
				BackgroundColor = Color.Blue,
				TextColor = Color.White,
				Font = Font.SystemFontOfSize (16, FontAttributes.Bold),
				Text = "+"
			};

			#if not
			t3Button.Clicked += (object sender, EventArgs e) => {
				var a = new Animation ((x) => {
					t3.RowHeight = x;
				}, t3.RowHeight, t3.RowHeight * 1.5, null, null);
				t3.Animate ("foo", a, 16, 1000);
			};
			#endif

			#if false
			t3Button.Clicked += (object sender, EventArgs e) => {
				var a = new Animation ((x) => {
					t3.FrozenColumnWidth = x;
				}, t3.FrozenColumnWidth, t3.FrozenColumnWidth * 1.5, null, null);
				t3.Animate ("foo", a, 16, 1000);
			};
			#endif

			#if false
			t3Button.Clicked += (object sender, EventArgs e) => {
				var a = new Animation ((x) => {
					t3.ColumnSizes[1].Width = x;
				}, t3.ColumnSizes[1].Width, t3.ColumnSizes[1].Width * 1.5, null, null);
				t3.Animate ("foo", a, 16, 1000);
			};
			#endif

			#if true
			t3Button.Clicked += (object sender, EventArgs e) => {
				t3.ColumnSizes.Add(new ColumnSize());
			};
			#endif

			t3.CornerView = t3Button;

			pages["Test 3"] = new ContentPage {
				Content = t3
			};

			pages["Test 3a"] = new ContentPage {
				Content = new TabbyTest3() {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = Color.Black,
					FrozenRowHeight = -1,
					FrozenColumnWidth = -1,
					RowInternalGap = 2,
					ColumnSizes = new ObservableCollection<ColumnSize> {
						new ColumnSize { Width = 80, InternalGap = 2 },
						new ColumnSize { Width = 80, InternalGap = 2 },
						new ColumnSize { Width = 80, InternalGap = 2 },
					},
				}
			};
			#endif

			// --------------------------------

			var dg = new DataGrid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,

				BackgroundColor = Color.Black,

				RowHeight = 50,
				RowSpacing = 2,
				ColumnSpacing = 2,
			};

			dg.Columns = new ObservableCollection<Column> {
				new Column {
					Width = 100,
					HeaderView = new Label {
						Text = "English",
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
						v.SetBinding(Label.TextProperty, "English");
						return v;
					}),
				},
				new Column {
					Width = 100,
					HeaderView = new Label {
						Text = "Spanish",
						BackgroundColor = Color.Gray,
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
					},
					Template = new DataTemplate (() => {
						var v = new Label {
							TextColor = Color.Black,
							XAlign = TextAlignment.Center,
							YAlign = TextAlignment.Center,
						};
						v.SetBinding(Label.TextProperty, "Spanish");
						v.SetBinding(Label.BackgroundColorProperty, "SpanishBackgroundColor");
						return v;
					}),
				},
				new Column {
					Width = 100,
					Template = new DataTemplate (() => {
						var v = new Image();
						v.SetBinding(Image.SourceProperty, "ImageName");
						return v;
					}),
				},
				new Column {
					Width = 200,
					HeaderView = new Label {
						Text = "Button",
						BackgroundColor = Color.Gray,
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
					},
					Template = new DataTemplate (() => {
						var v = new myButton {
							BackgroundColor = Color.Green,
							TextColor = Color.White,
							Font = Font.SystemFontOfSize(18, FontAttributes.Bold),
						};
						v.SetBinding(myButton.TextProperty, "ButtonText");
						v.SetBinding(myButton.OneActionProperty, "Action");
						return v;
					}),
				},
				new Column {
					Width = 100,
					HeaderView = new Label {
						Text = "X",
						BackgroundColor = Color.Gray,
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
					},
					Template = new DataTemplate (() => {
						var v = new Entry();
						v.SetBinding(Entry.TextProperty, "X");
						return v;
					}),
				},
				new Column {
					Width = 100,
					HeaderView = new Label {
						Text = "X*2",
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
						v.SetBinding(Label.TextProperty, "DoubleX");
						return v;
					}),
				},
			};

			dg.FrozenColumn = new Column {
				Width = 80,
				HeaderView = new Label {
					Text = "Corner",
					BackgroundColor = Color.Yellow,
				},
				Template = new DataTemplate (() => {
					var v = new Label {
						BackgroundColor = Color.Gray,
						TextColor = Color.Black,
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
					};
					v.SetBinding(Label.TextProperty, "Spanish");
					return v;
				}),
			};

			dg.HeaderHeight = 50;
			//dg.SelectionMode = SelMode.Row;

			dg.Rows = testData.createLotsOfWordPairsWithButtons (dg);

			pages ["Test 4"] = new ContentPage {
				Content = dg
			};
			dg = null;

            pages["Test Xaml"] = new xaml.XamlTest();

			pages ["No Columns, No Rows"] = new ContentPage {
				Content = new DataGrid {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,

					BackgroundColor = Color.Black,

					RowHeight = 50,
					RowSpacing = 2,
					ColumnSpacing = 2,
					HeaderHeight = 50,
				}
			};

			pages ["No Columns, Four Rows"] = new ContentPage {
				Content = new DataGrid {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,

					BackgroundColor = Color.Black,

					RowHeight = 50,
					RowSpacing = 2,
					ColumnSpacing = 2,
					HeaderHeight = 50,

					Rows = testData.createFourSimpleWordPairs(),
				}
			};

			pages ["Two Columns, Four Rows"] = new ContentPage {
				Content = new DataGrid {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,

					BackgroundColor = Color.Black,

					RowHeight = 50,
					RowSpacing = 2,
					ColumnSpacing = 2,
					HeaderHeight = 50,

					Columns = testData.createTwoSimpleColumns(),
					Rows = testData.createFourSimpleWordPairs(),
				}
			};

			pages ["Two Columns, No Rows"] = new ContentPage {
				Content = new DataGrid {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,

					BackgroundColor = Color.Black,

					RowHeight = 50,
					RowSpacing = 2,
					ColumnSpacing = 2,
					HeaderHeight = 50,

					Columns = testData.createTwoSimpleColumns(),
				}
			};

			pages ["XSquared"] = 
			new ContentPage {
				Content = new DataGrid {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,

					BackgroundColor = Color.Black,

					RowHeight = 50,
					RowSpacing = 2,
					ColumnSpacing = 2,
					HeaderHeight = 50,

					Columns = new ObservableCollection<Column> {
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
								var v = new Slider {
									BackgroundColor = Color.White,
									Minimum = -20,
									Maximum = 20,
								};
								v.SetBinding (Slider.ValueProperty, "X", BindingMode.TwoWay);
								return v;
							}),
						},
					},

					Rows = new ObservableCollection<object> {
						new myRow {X = 1},
						new myRow {X = 2},
						new myRow {X = 3},
						new myRow {X = 4},
						new myRow {X = 5},
						new myRow {X = 6},
						new myRow {X = 7},
					},
				}
			};

			pages ["Million"] = 
				new ContentPage {
				Content = new DataGrid {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,

					BackgroundColor = Color.Black,

					RowHeight = 40,
					RowSpacing = 2,
					ColumnSpacing = 2,
					HeaderHeight = 40,

					SelectionMode = SelMode.Row,

					Columns = new ObservableCollection<Column> {
						new Column {
							Width = 120,
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
									XAlign = TextAlignment.End,
									YAlign = TextAlignment.Center,
								};
								v.SetBinding (Label.TextProperty, "X");
								return v;
							}),
						},
						new Column {
							Width = 120,
							HeaderView = new Label {
								Text = "Sqrt(X)",
								BackgroundColor = Color.Gray,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							},
							Template = new DataTemplate (() => {
								var v = new Label {
									BackgroundColor = Color.White,
									TextColor = Color.Black,
									XAlign = TextAlignment.End,
									YAlign = TextAlignment.Center,
								};
								v.SetBinding (Label.TextProperty, "Sqrt");
								return v;
							}),
						},
					},

					Rows = new myList(1000000),
				}
			};

			var lst = new ListView ();
			lst.ItemsSource = pages.Keys;

			var mainPage = new ContentPage {
				Content = lst
			};

			var nav = new NavigationPage (mainPage);

			lst.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
				nav.PushAsync(pages[e.SelectedItem.ToString()]);
			};

			MainPage = nav;
		}
	}
}

