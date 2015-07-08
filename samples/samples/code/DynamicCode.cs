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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zumero;

namespace samples
{
	// This class implements a list of rows that is "virtual".
	// It can claim to have a million rows but it doesn't store
	// them.  It just generates a row object when you ask it
	// for one.  It is pretending to be a collection even though
	// it really is not.
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

	class DynamicCode : ContentPage
	{
		private DataGrid dg = null;
		public DynamicCode()
		{
			dg = new DataGrid () {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,

				BackgroundColor = Color.Black,

				RowHeight = 40,
				RowSpacing = 2,
				ColumnSpacing = 2,
				HeaderHeight = 40,
				SelectionMode = SelMode.Row,
				SelectedBackgroundColor = Color.Aqua,
				UnselectedBackgroundColor = Color.White,

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

				Rows = new myList (1000000),
			};

			#if not
			this.Content = new StackLayout {
				Children = {
					new Label {
						BackgroundColor = Color.Olive,
						Text = "This sample shows a million virtual rows.",
						HeightRequest = 50,
						Font = Font.SystemFontOfSize(14, FontAttributes.Italic),
					},
					dg,
				}
			};
			#endif
			this.Content = dg;
		}
	}
}

