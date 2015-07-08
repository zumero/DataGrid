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

using Zumero;

using Xamarin.Forms;

namespace demo
{
	#if not
	public class ColumnSize : BindableObject
	{
		public static readonly BindableProperty WidthProperty =
			BindableProperty.Create<ColumnSize,double>(
				p => p.Width, 80);

		public double Width {
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty InternalGapProperty =
			BindableProperty.Create<ColumnSize,double>(
				p => p.InternalGap, 2);

		public double InternalGap {
			get { return (double)GetValue(InternalGapProperty); }
			set { SetValue(InternalGapProperty, value); } // TODO disallow invalid values
		}
	}

	public class TabbyTest1 : DataGridCore
	{
		private int[] _colViewTypes;

		private const int cvt_Label = 0;
		private const int cvt_Entry = 1;
		private const int cvt_Switch = 2;
		private const int cvt_Image = 3;
		private const int cvt_FrozenRow = 4;
		private const int cvt_FrozenColumn = 5;

		public TabbyTest1()
		{
			// TODO this is hokey, since it assumes that ColumnSizes.Length will be 5
			const int COUNT_COLS = 5;

			_colViewTypes = new int[COUNT_COLS];
			for (int i = 0; i < _colViewTypes.Length; i++) {
				_colViewTypes [i] = cvt_Label;
			}
			_colViewTypes [1] = cvt_Entry;
			_colViewTypes [2] = cvt_Switch;
			_colViewTypes [3] = cvt_Image;
		}

		protected override int numberOfRows {
			get {
				return 1000;
			}
		}

		protected override double frozenColumnFullWidth {
			get {
				return FrozenColumnWidth;
			}
		}

		protected override double frozenColumnInnerWidth {
			get {
				return FrozenColumnWidth;
			}
		}

		protected override int getCellViewTemplateIDForColumn (int col)
		{
			return _colViewTypes [col];
		}

		protected override int frozenColumnCellViewTemplateID {
			get {
				return cvt_FrozenColumn;
			}
		}

		protected override int frozenRowCellViewTemplateID {
			get {
				return cvt_FrozenRow;
			}
		}

		protected override View createCellView (int cellViewType)
		{
			switch (cellViewType) {
			case cvt_Entry:
				{
					var v = new Entry ();
					v.BackgroundColor = Color.White;
					v.TextColor = Color.Black;
					return v;
				}
			case cvt_Switch:
				{
					var v = new Switch ();
					v.BackgroundColor = Color.White;
					return v;
				}
			case cvt_Image:
				{
					var v = new Image ();
					return v;
				}
			case cvt_Label:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Aqua;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			case cvt_FrozenRow:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Gray;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			case cvt_FrozenColumn:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Fuschia;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			default:
				{
					throw new Exception ();
				}
			}
		}

		private int getCellViewType(int col, int row)
		{
			if (row < 0)
				return frozenRowCellViewTemplateID;
			else if (col < 0)
				return frozenColumnCellViewTemplateID;
			else
				return _colViewTypes [col];
		}

		protected override void prepareCellView (View v, int col, int row)
		{
			switch (getCellViewType(col,row)) {
			case cvt_Entry:
				{
					var cv = v as Entry;
					cv.Text = string.Format ("{0},{1}", col, row);
				}
				break;
			case cvt_Switch:
				{
					var cv = v as Switch;
					cv.IsToggled = 0 == row % 5;
				}
				break;
			case cvt_Image:
				{
					var cv = v as Image;
					cv.Source = "breaking.png";
				}
				break;
			case cvt_Label:
				{
					var cv = v as Label;
					cv.Text = string.Format ("{0},{1}", col, row);
				}
				break;
			case cvt_FrozenRow:
				{
					var cv = v as Label;
					cv.Text = string.Format ("C{0}", col);
				}
				break;
			case cvt_FrozenColumn:
				{
					var cv = v as Label;
					cv.Text = string.Format ("R{0}", row);
				}
				break;
			default:
				{
					throw new Exception ();
				}
			}
		}

		protected override void unprepareCellView (View v, int col, int row)
		{
		}

		public static readonly BindableProperty ColumnSizesProperty =
			BindableProperty.Create<TabbyTest1,ObservableCollection<ColumnSize>>(
				p => p.ColumnSizes, null);

		public ObservableCollection<ColumnSize> ColumnSizes {
			get { return (ObservableCollection<ColumnSize>)GetValue(ColumnSizesProperty); }
			set { SetValue(ColumnSizesProperty, value); }
		}

		public static readonly BindableProperty FrozenColumnWidthProperty =
			BindableProperty.Create<TabbyTest1,double>(
				p => p.FrozenColumnWidth, 80);

		public double FrozenColumnWidth {
			get { return (double)GetValue(FrozenColumnWidthProperty); }
			set { SetValue(FrozenColumnWidthProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty FrozenRowHeightProperty =
			BindableProperty.Create<TabbyTest1,double>(
				p => p.FrozenRowHeight, 50);

		public double FrozenRowHeight {
			get { return (double)GetValue(FrozenRowHeightProperty); }
			set { SetValue(FrozenRowHeightProperty, value); } // TODO disallow invalid values
		}

		protected override double frozenRowHeight {
			get {
				return FrozenRowHeight;
			}
		}

		protected override int numberOfColumns {
			get {
				if (null == ColumnSizes) {
					return 0;
				}
				return ColumnSizes.Count;
			}
		}

		protected override double getColumnFullWidth (int col)
		{
			return ColumnSizes [col].Width;
		}

		protected override double getColumnInnerWidth (int col)
		{
			return ColumnSizes [col].Width - ColumnSizes [col].InternalGap;
		}

	}

	public class TabbyTest2 : DataGridCore
	{
		private const int cvt_Label = 0;
		private const int cvt_FrozenRow = 4;
		private const int cvt_FrozenColumn = 5;

		public TabbyTest2()
		{
		}

		protected override int numberOfRows {
			get {
				return 10000;
			}
		}

		protected override int getCellViewTemplateIDForColumn (int col)
		{
			return cvt_Label;
		}

		protected override double frozenColumnFullWidth {
			get {
				return FrozenColumnWidth;
			}
		}

		protected override double frozenColumnInnerWidth {
			get {
				return FrozenColumnWidth;
			}
		}

		protected override int frozenColumnCellViewTemplateID {
			get {
				return cvt_FrozenColumn;
			}
		}

		protected override int frozenRowCellViewTemplateID {
			get {
				return cvt_FrozenRow;
			}
		}

		protected override View createCellView (int cellViewType)
		{
			switch (cellViewType) {
			case cvt_Label:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.White;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.End;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (14);
					return v;
				}
			case cvt_FrozenRow:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Gray;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			case cvt_FrozenColumn:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Gray;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			default:
				{
					throw new Exception ();
				}
			}
		}

		private int getCellViewType(int col, int row)
		{
			if (row < 0)
				return frozenRowCellViewTemplateID;
			else if (col < 0)
				return frozenColumnCellViewTemplateID;
			else
				return cvt_Label;
		}

		protected override void prepareCellView (View v, int col, int row)
		{
			switch (getCellViewType(col,row)) {
			case cvt_Label:
				{
					var cv = v as Label;
					switch (col) {
					case 0:
						cv.Text = string.Format ("{0}", row*row);
						break;
					case 1:
						cv.Text = string.Format ("{0}", row*row*row);
						break;
					case 2:
						cv.Text = string.Format ("{0}", row*row*row*row);
						break;
					}
				}
				break;
			case cvt_FrozenRow:
				{
					var cv = v as Label;
					switch (col) {
					case 0:
						cv.Text = "^2";
						break;
					case 1:
						cv.Text = "^3";
						break;
					case 2:
						cv.Text = "^4";
						break;
					}
				}
				break;
			case cvt_FrozenColumn:
				{
					var cv = v as Label;
					cv.Text = string.Format ("{0}", row);
				}
				break;
			default:
				{
					throw new Exception ();
				}
			}
		}

		protected override void unprepareCellView (View v, int col, int row)
		{
		}

		public static readonly BindableProperty ColumnSizesProperty =
			BindableProperty.Create<TabbyTest2,ObservableCollection<ColumnSize>>(
				p => p.ColumnSizes, null);

		public ObservableCollection<ColumnSize> ColumnSizes {
			get { return (ObservableCollection<ColumnSize>)GetValue(ColumnSizesProperty); }
			set { SetValue(ColumnSizesProperty, value); }
		}

		public static readonly BindableProperty FrozenColumnWidthProperty =
			BindableProperty.Create<TabbyTest2,double>(
				p => p.FrozenColumnWidth, 80);

		public double FrozenColumnWidth {
			get { return (double)GetValue(FrozenColumnWidthProperty); }
			set { SetValue(FrozenColumnWidthProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty FrozenRowHeightProperty =
			BindableProperty.Create<TabbyTest2,double>(
				p => p.FrozenRowHeight, 50);

		public double FrozenRowHeight {
			get { return (double)GetValue(FrozenRowHeightProperty); }
			set { SetValue(FrozenRowHeightProperty, value); } // TODO disallow invalid values
		}

		protected override double frozenRowHeight {
			get {
				return FrozenRowHeight;
			}
		}

		protected override int numberOfColumns {
			get {
				if (null == ColumnSizes) {
					return 0;
				}
				return ColumnSizes.Count;
			}
		}

		protected override double getColumnFullWidth (int col)
		{
			return ColumnSizes [col].Width;
		}

		protected override double getColumnInnerWidth (int col)
		{
			return ColumnSizes [col].Width - ColumnSizes [col].InternalGap;
		}

	}


	public class TabbyTest3 : DataGridCore
	{
		// TODO the cellview types want to be exposed as some kind of
		// a template or pattern

		private const int cvt_Label = 0;
		private const int cvt_FrozenRow = 4;
		private const int cvt_FrozenColumn = 5;

		public TabbyTest3()
		{
			this.PropertyChanged += handlePropertyChanged;
		}

		void handlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == TabbyTest3.ColumnSizesProperty.PropertyName) {
				// the entire ColumnSizes collection got replaced

				// setup new listeners
				if (this.ColumnSizes != null) {
					this.ColumnSizes.CollectionChanged += handleColumnSizesCollectionChanged;
					foreach (var cs in this.ColumnSizes) {
						cs.PropertyChanged += handleColumnSizePropertyChanged;
					}
				}

				this.numberOfColumnsChanged ();
			} 
		}

		protected void handleColumnSizePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (
				(e.PropertyName == ColumnSize.WidthProperty.PropertyName) 
				|| (e.PropertyName == ColumnSize.InternalGapProperty.PropertyName)
			)
			{
				this.columnSizesChanged ();
			}
		}

		private void handleColumnSizesCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			// a column got added or deleted.  redo the cell inventory.
			this.numberOfColumnsChanged ();
		}

		protected override int numberOfRows {
			get {
				return 10000;
			}
		}

		protected override double frozenColumnFullWidth {
			get {
				return FrozenColumnWidth;
			}
		}

		protected override double frozenColumnInnerWidth {
			get {
				return FrozenColumnWidth;
			}
		}

		protected override int frozenColumnCellViewTemplateID {
			get {
				return cvt_FrozenColumn;
			}
		}

		protected override int frozenRowCellViewTemplateID {
			get {
				return cvt_FrozenRow;
			}
		}

		protected override int getCellViewTemplateIDForColumn (int col)
		{
			return cvt_Label;
		}

		protected override View createCellView (int cellViewType)
		{
			switch (cellViewType) {
			case cvt_Label:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.White;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.End;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (14);
					return v;
				}
			case cvt_FrozenRow:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Gray;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			case cvt_FrozenColumn:
				{
					var v = new Label ();
					v.BackgroundColor = Xamarin.Forms.Color.Gray;
					v.TextColor = Color.Black;
					v.XAlign = Xamarin.Forms.TextAlignment.Center;
					v.YAlign = Xamarin.Forms.TextAlignment.Center;
					v.Font = Xamarin.Forms.Font.SystemFontOfSize (18, FontAttributes.Bold);
					return v;
				}
			default:
				{
					throw new Exception ();
				}
			}
		}

		private int getCellViewType(int col, int row)
		{
			if (row < 0)
				return frozenRowCellViewTemplateID;
			else if (col < 0)
				return frozenColumnCellViewTemplateID;
			else
				return cvt_Label;
		}

		protected override void prepareCellView (View v, int col, int row)
		{
			switch (getCellViewType(col,row)) {
			case cvt_Label:
				{
					var cv = v as Label;
					long val = row;
					for (int i = 0; i < col + 1; i++) {
						val = val * row;
					}
					cv.Text = string.Format ("{0}", val);
				}
				break;
			case cvt_FrozenRow:
				{
					var cv = v as Label;
					cv.Text = "^" + (col + 2).ToString ();
				}
				break;
			case cvt_FrozenColumn:
				{
					var cv = v as Label;
					cv.Text = string.Format ("{0}", row);
				}
				break;
			default:
				{
					throw new Exception ();
				}
			}
		}

		protected override void unprepareCellView (View v, int col, int row)
		{
		}

		public static readonly BindableProperty ColumnSizesProperty =
			BindableProperty.Create<TabbyTest3,ObservableCollection<ColumnSize>>(
				p => p.ColumnSizes, null);

		public ObservableCollection<ColumnSize> ColumnSizes {
			get { return (ObservableCollection<ColumnSize>)GetValue(ColumnSizesProperty); }
			set { SetValue(ColumnSizesProperty, value); }
		}

		public static readonly BindableProperty FrozenColumnWidthProperty =
			BindableProperty.Create<TabbyTest3,double>(
				p => p.FrozenColumnWidth, 80);

		public double FrozenColumnWidth {
			get { return (double)GetValue(FrozenColumnWidthProperty); }
			set { SetValue(FrozenColumnWidthProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty FrozenRowHeightProperty =
			BindableProperty.Create<TabbyTest3,double>(
				p => p.FrozenRowHeight, 50);

		public double FrozenRowHeight {
			get { return (double)GetValue(FrozenRowHeightProperty); }
			set { SetValue(FrozenRowHeightProperty, value); } // TODO disallow invalid values
		}

		protected override double frozenRowHeight {
			get {
				return FrozenRowHeight;
			}
		}

		protected override int numberOfColumns {
			get {
				if (null == ColumnSizes) {
					return 0;
				}
				return ColumnSizes.Count;
			}
		}

		protected override double getColumnFullWidth (int col)
		{
			return ColumnSizes [col].Width;
		}

		protected override double getColumnInnerWidth (int col)
		{
			return ColumnSizes [col].Width - ColumnSizes [col].InternalGap;
		}

	}
	#endif
}


