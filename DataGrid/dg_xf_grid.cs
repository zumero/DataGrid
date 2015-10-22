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

using Xamarin.Forms;

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Zumero.DataGrid.iOS")]
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Zumero.DataGrid.Unified")]
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Zumero.DataGrid.Android")]
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Zumero.DataGrid.WinPhone")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Zumero.DataGrid.Windows")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Zumero.DataGrid.WinPhone81")]

namespace Zumero
{
	public enum SelMode
	{
		None,
		Row,
	}

	public class Column : BindableObject
	{
		public static readonly BindableProperty WidthProperty =
			BindableProperty.Create<Column,double>(
				p => p.Width, 80);

		public double Width {
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty TemplateProperty =
			BindableProperty.Create<Column,DataTemplate>(
				p => p.Template, null);

		public DataTemplate Template {
			get { return (DataTemplate)GetValue(TemplateProperty); }
			set { SetValue(TemplateProperty, value); }
		}

		public static readonly BindableProperty HeaderViewProperty =
			BindableProperty.Create<Column,View>(
				p => p.HeaderView, null);

		public View HeaderView {
			get { return (View)GetValue(HeaderViewProperty); }
			set { SetValue(HeaderViewProperty, value); }
		}

	}

	public class DataGrid : ContentView
	{
		private struct CoordPair
		{
			private int _col;
			private int _row;
			public int col { get { return _col; } }
			public int row { get { return _row; } }
			public CoordPair(int c, int r)
			{
				_col = c;
				_row = r;
			}
		};

		private readonly AbsoluteLayout container;
		private readonly ContentView cornerViewContainer;
		private readonly myLayout mainPanel;
		private readonly myLayout frozenRowPanel;
		private readonly myLayout frozenColumnPanel;

		public DataGrid()
		{
			cornerViewContainer = new ContentView();
			mainPanel = new myLayout(mainGetBox);
			frozenRowPanel = new myLayout(this.frozenRowGetBox);
			frozenColumnPanel = new myLayout(this.frozenColumnGetBox);
			cellInventoryByColumn = new Dictionary<int, List<View>> ();
			coordsToBoundView = new Dictionary<int, Dictionary<int, View>> ();
			boundViewToCoords = new Dictionary<View, CoordPair> ();

			container = new AbsoluteLayout ();
            if (Device.OS != TargetPlatform.Windows &&
                Device.OS != TargetPlatform.WinPhone)
            {
                //Setting these flags was causing problems 
                frozenRowPanel.IsClippedToBounds = true;
                frozenColumnPanel.IsClippedToBounds = true;
                mainPanel.IsClippedToBounds = true;
            }
            else
            {
                frozenRowPanel.CallOnChildMeasureInvalidated = true;
            }

            // note that the ordering of the following statements is
            // significant, but only because IsClippedToBounds doesn't
            // seem to work properly in the WP8 version of Xamarin.Forms.

			container.Children.Add(mainPanel); 
			container.Children.Add(frozenColumnPanel);
			container.Children.Add (frozenRowPanel);
			container.Children.Add(cornerViewContainer);

			this.PropertyChanged += handlePropertyChanged;
			this.PropertyChanging += handlePropertyChanging;

			this.Columns = new ObservableCollection<Column> ();

			Content = container;
		}

		private double frozenRowHeightPlusSpacing { get { return HeaderHeight + RowSpacing; } }
		private double rowHeightPlusSpacing { get { return RowHeight + RowSpacing; } }
		private double getColumnWidthPlusSpacing(int col)
		{
			var w = getColumnWidth (col);
			if (w < 0) {
				w = 0;
			} else if (w > 0) {
				w += ColumnSpacing;
			}
			return w;
		}

		// ----------------

		private void columnHeaderViewChanged(int col)
		{
			if (col >= 0) {
				// getFrozenRowView is not allowed to return null.
				// frozenRowPanel.Children is required to have
				// exactly as many items as columns, and they must
				// be in the right order.

#if true
				frozenRowPanel.Children.Clear ();
				for (int c = 0; c < numberOfColumns; c++) {
					View v = getFrozenRowView(c);
					frozenRowPanel.Children.Add (v);
				}
				frozenRowPanel.LayoutAllChildren ();
#else
				var v = getFrozenRowView (col);
				// TODO the following line crashes on Android.  in EnsureChildOrder.
				frozenRowPanel.Children [col] = v;
				frozenRowPanel.LayoutOneChild (v);
#endif
			} else {
				// the frozen col headerview is the corner view
				cornerViewContainer.Content = getFrozenRowView(-1);
			}
		}

		private void columnsAdded(int start, int count)
		{
			if ((start + count) == numberOfColumns) {
				// the new column appeared at the end
				// nothing to do here
			} else {
				// shift everything over
				for (int col = numberOfColumns-1; col >= start + count; col--) {
					cellInventoryByColumn [col] = cellInventoryByColumn [col - count];
					cellInventoryByColumn.Remove (col - count);
					coordsToBoundView [col] = coordsToBoundView [col - count];
					coordsToBoundView.Remove (col - count);
					foreach (View v in cellInventoryByColumn [col]) {
						CoordPair t;
						if (boundViewToCoords.TryGetValue (v, out t)) {
							// assert t.col (item1) is col - count
							boundViewToCoords [v] = new CoordPair (col, t.row);
						}
					}
				}
			}

			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;

			rebuildFrozenRow ();

			updateVisibility ();

			mainPanel.LayoutAllChildren();
			frozenRowPanel.LayoutAllChildren();
			frozenColumnPanel.LayoutAllChildren();
		}

		private void columnsRemoved(int start, int count)
		{
			// can't be the frozen col.  start=-1 is disallowed here.
			for (int i = 0; i < count; i++) {
				discardCellInventoryForColumn (start + i, true);
				frozenRowPanel.Children.RemoveAt (start + i);
			}

			if (start == numberOfColumns) {
				// the remove occurred at the end
				// nothing to do here
			} else {
				// shift everything over
				for (int col = start; col < numberOfColumns; col++) {
					// assert !cellInventoryByColumn.Contains(col)
					cellInventoryByColumn [col] = cellInventoryByColumn [col + count];
					cellInventoryByColumn.Remove (col + count);
					// assert !coordsToBoundView.Contains(col)
					coordsToBoundView [col] = coordsToBoundView [col + count];
					coordsToBoundView.Remove (col + count);
					foreach (View v in cellInventoryByColumn [col]) {
						CoordPair t;
						if (boundViewToCoords.TryGetValue (v, out t)) {
							// assert t.col (item1) is col + count
							boundViewToCoords [v] = new CoordPair (col, t.row);
						}
					}
				}
			}

			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;
			ensureColumnEdgesAreCached ();

			if (ContentOffset_X > ScrollXMax) {
				SetContentOffset (ScrollXMax, ContentOffset_Y);
			} else {
				updateVisibility ();
				mainPanel.LayoutAllChildren ();
				frozenRowPanel.LayoutAllChildren ();
			}
		}

		private void columnsReplaced(int start, int count)
		{
			if (start < 0) {
				count = 1;
			} else if (count < 1) {
				count = 1;
			}

			for (int i = 0; i < count; i++) {
				discardCellInventoryForColumn (start + i, true);
			}

			if (start < 0) {
				cornerViewContainer.Content = getFrozenRowView (-1);
			}

			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;
			rebuildFrozenRow ();
			updateVisibility ();
			mainPanel.LayoutAllChildren();
			frozenRowPanel.LayoutAllChildren();
			frozenColumnPanel.LayoutAllChildren();
		}

		private void allColumnsReplaced()
		{
			foreach (var col in cellInventoryByColumn.Keys) {
				discardCellInventoryForColumn (col, false);
			}

			frozenColumnPanel.Children.Clear ();
			frozenRowPanel.Children.Clear ();
			mainPanel.Children.Clear ();

			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;
			rebuildFrozenRow ();
			updateVisibility ();
			mainPanel.LayoutAllChildren();
			frozenRowPanel.LayoutAllChildren();
			frozenColumnPanel.LayoutAllChildren();
		}

		private void allRowsReplaced()
		{
			if (ContentOffset_X > ScrollXMax) {
				SetContentOffset (ScrollXMax, ContentOffset_Y);
			} else {
				if (Content != null) {
					updateVisibility ();
					mainPanel.LayoutAllChildren ();
					frozenColumnPanel.LayoutAllChildren ();
				}
			}
		}

		private static void setBindingContext(View v, object obj)
		{
			//var prev = v.BindingContext;
            // TODO not actually sure we want to do the check in the next line
			if (v.BindingContext != obj) 
            {
				v.BindingContext = obj;
			}
		}

		private void fixSelection(View v, int col, int row)
		{
			if (SelMode.Row == SelectionMode) {
				if ( (v is Xamarin.Forms.Label) || (v is Xamarin.Forms.Layout) ) {
					if (row == SelectedRowIndex) {
						v.BackgroundColor = SelectedBackgroundColor;
					} else {
						v.BackgroundColor = UnselectedBackgroundColor;
					}
				}
			}
		}

		private void fixSelection(int row)
		{
			foreach (var col in coordsToBoundView.Keys) {
				var rowsForThisCol = coordsToBoundView [col];
				View v;
				if (rowsForThisCol.TryGetValue (row, out v)) {
					fixSelection (v, col, row);
				}
			}
        }

		private void rowsReplaced(int start, int count)
		{
			foreach (var col in coordsToBoundView.Keys) {
				var rowsForThisCol = coordsToBoundView [col];
				for (int i = 0; i < count; i++) {
					int row = start + i;
					View v;
					if (rowsForThisCol.TryGetValue (row, out v)) {
						setBindingContext (v, getDataForCell (col, row));
						fixSelection (v, col, row);
					}
				}
			}
		}

		private void rebindRowsFrom(int start)
		{
			foreach (var col in coordsToBoundView.Keys) {
				var rowsForThisColumn = coordsToBoundView [col];
				List<int> rm = new List<int> ();
				foreach (var row in rowsForThisColumn.Keys) {
					if (row >= start) {
						View v = rowsForThisColumn [row];
						if (row >= numberOfRows) {
							setBindingContext (v, null);
							boundViewToCoords.Remove (v);
							rm.Add (row);
							v.Layout(new Rectangle (-1000, -1000, 0, 0));
						} else {
							setBindingContext (v, getDataForCell (col, row));
							fixSelection (v, col, row);
						}
					}
				}
				foreach (var row in rm) {
					rowsForThisColumn.Remove (row);
				}
			}
		}

		private void rowsAdded(int start, int count)
		{
			rebindRowsFrom (start);
			updateVisibility ();
			mainPanel.LayoutAllChildren ();
			frozenColumnPanel.LayoutAllChildren ();
		}

		private void rowsRemoved(int start, int count)
		{
			// TODO why is count param unused?

			rebindRowsFrom (start);
			if (ContentOffset_Y > ScrollYMax) {
				SetContentOffset (ContentOffset_X, ScrollYMax);
			}
		}

		private void discardCellInventoryForColumn(int col, bool removeFromPanel)
		{
			// no changes to size or position of cells

			List<View> inventoryForThisColumn;
			if (cellInventoryByColumn.TryGetValue (col, out inventoryForThisColumn)) {
				foreach (View v in inventoryForThisColumn) {
					setBindingContext (v, null);
					CoordPair t;
					// assert t.Item1 == col
					if (boundViewToCoords.TryGetValue (v, out t)) {
						coordsToBoundView [t.col].Remove (t.row);
						boundViewToCoords.Remove (v);
					}
					if (removeFromPanel) {
						myLayout panel = (col < 0) ? frozenColumnPanel : mainPanel;
						panel.Children.Remove (v);
					}
				}
				cellInventoryByColumn.Remove (col);
				coordsToBoundView.Remove (col);
			}
		}

		private void columnTemplateChanged(int col)
		{
			discardCellInventoryForColumn (col, true);
			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;
			rebuildFrozenRow ();
			updateVisibility ();
			mainPanel.LayoutAllChildren();
			frozenRowPanel.LayoutAllChildren();
			frozenColumnPanel.LayoutAllChildren();
		}

		private void columnWidthChanged(int col)
		{
			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;
			ensureColumnEdgesAreCached ();

			if (col < 0) {
				layoutPanels ();
			}
			if (ContentOffset_X > ScrollXMax) {
				SetContentOffset (ScrollXMax, ContentOffset_Y);
			} else {
				updateVisibility ();
				mainPanel.LayoutAllChildren ();
				frozenRowPanel.LayoutAllChildren ();
				frozenColumnPanel.LayoutAllChildren ();
			}
		}

		private void layoutVisibleCellsInOneColumn(int col)
		{
			myLayout panel = (col < 0) ? frozenColumnPanel : mainPanel;
			Dictionary<int,View> rows;
			if (coordsToBoundView.TryGetValue (col, out rows)) {
				foreach (int row in rows.Keys) {
					View v = rows [row];
					panel.LayoutOneChild (v);
				}
			}
		}

		private void layoutLowerVisibleChildren()
		{
			foreach (var col in coordsToBoundView.Keys) {
				layoutVisibleCellsInOneColumn (col);
			}
		}

		private void layoutFrozenRow()
		{
			foreach (View v in frozenRowPanel.Children) {
				frozenRowPanel.LayoutOneChild (v);
			}
		}

		private void rebuildFrozenRow()
		{
			// the frozen row does not do cell recycling
			frozenRowPanel.Children.Clear ();
			for (int col = 0; col < numberOfColumns; col++) {
				View v = getFrozenRowView(col);
				frozenRowPanel.Children.Add (v);
			}
		}

		private void rowSpacingChanged()
		{
			layoutPanels ();
			rowHeightChanged ();
		}

		private void columnSpacingChanged()
		{
			this.cachedColumnEdges = null;
			this.cachedTotalWidth = 0;
			ensureColumnEdgesAreCached ();

			layoutPanels ();

			if (ContentOffset_X > ScrollXMax) {
				SetContentOffset (ScrollXMax, ContentOffset_Y);
			} else {
				updateVisibility ();
				mainPanel.LayoutAllChildren ();
				frozenRowPanel.LayoutAllChildren ();
				frozenColumnPanel.LayoutAllChildren ();
			}
		}

		private void rowHeightChanged()
		{
			// FWIW, column visibility cannot change here.

			updateVisibility ();

			// anytime visibility changes, we probably need to layout all
			// cells, not just the visible ones
			mainPanel.LayoutAllChildren ();
			frozenColumnPanel.LayoutAllChildren ();
			frozenRowPanel.LayoutAllChildren ();
		}

		private void frozenRowHeightChanged()
		{
			layoutPanels ();

			// visibility could have changed
			updateVisibility ();

			// anytime visibility changes, we probably need to layout all
			// cells, not just the visible ones
			mainPanel.LayoutAllChildren ();
			frozenColumnPanel.LayoutAllChildren ();
			frozenRowPanel.LayoutAllChildren ();
		}

		private double[] cachedColumnEdges;
		private double cachedTotalWidth;

		private double ContentOffset_X;
		private double ContentOffset_Y;

		private class myLayout : Layout<View>
		{
			Func<View,Rectangle> getBox;

			public myLayout(Func<View,Rectangle> f)
			{
				getBox = f;
			}

			public void LayoutOneChild(View v)
			{
				Rectangle r = getBox (v);
				v.Layout (r);
			}

			public void LayoutAllChildren()
			{
				foreach (View v in Children) {
					LayoutOneChild (v);
				}
			}

			protected override bool ShouldInvalidateOnChildAdded (View child)
			{
				return false;
			}

			protected override bool ShouldInvalidateOnChildRemoved (View child)
			{
				return false;
			}

            public bool CallOnChildMeasureInvalidated = false;
			protected override void OnChildMeasureInvalidated ()
			{
                if (CallOnChildMeasureInvalidated)
                    base.OnChildMeasureInvalidated();
			}

			protected override void LayoutChildren (double x, double y, double width, double height)
			{
				// TODO consider a flag here to suspend/resume.
				// maybe implement this by requiring all children insertions to be done
				// through a method?
				LayoutAllChildren ();
			}
		}

		private readonly Dictionary<int,List<View>> cellInventoryByColumn;
		private readonly Dictionary<View,CoordPair> boundViewToCoords;
		private readonly Dictionary<int,Dictionary<int,View>> coordsToBoundView;

		private View createCellView (DataTemplate dt)
		{
			object obj = dt.CreateContent ();
			if (obj == null) {
				throw new ArgumentNullException ("The DataTemplate returned null.");
			}
			if (obj is Cell) {
				throw new InvalidOperationException ("The DataTemplate returned a Xamarin.Forms.Cell (which is unsupported) instead of a View.");
			}
			if (!(obj is View)) {
				throw new InvalidOperationException ("The DataTemplate must return a Xamarin.Forms.View.");
			}
			var v = (View)obj;
			setBindingContext (v, null);
			return v;
		}

		private bool getCoordsForBoundView(View v, out int col, out int row)
		{
			CoordPair t;
			if (boundViewToCoords.TryGetValue (v, out t)) {
				col = t.col;
				row = t.row;
				return true;
			} else {
				col = -1;
				row = -1;
				return false;
			}
		}

		private bool getBoundViewForCoords(int col, int row, out View v)
		{
			Dictionary<int,View> d;
			if (!coordsToBoundView.TryGetValue (col, out d)) {
				v = null;
				return false;
			}
			return d.TryGetValue (row, out v);
		}

		private View findAvailableCellView(int col)
		{
			List<View> vlist;
			if (!cellInventoryByColumn.TryGetValue (col, out vlist)) {
				vlist = new List<View> ();
				cellInventoryByColumn [col] = vlist;
			}
			// TODO linear search?  really?
			foreach (var cv in vlist) {
				if (!boundViewToCoords.ContainsKey(cv)) {
					return cv;
				}
			}

			var cvt = getTemplateForColumn (col);
			View v = createCellView (cvt);
			vlist.Add (v);
			if (col < 0) {
				frozenColumnPanel.Children.Add (v);
			} else {
				mainPanel.Children.Add (v);
			}
			return v;
		}

		private void layoutPanels()
		{
			var fcw = getColumnWidthPlusSpacing(-1);
			if (fcw < 0) {
				fcw = 0;
			}
			var frh = frozenRowHeightPlusSpacing;
			if (frh < 0) {
				frh = 0;
			}

			AbsoluteLayout.SetLayoutFlags (frozenRowPanel, AbsoluteLayoutFlags.None);
			AbsoluteLayout.SetLayoutBounds (frozenRowPanel, new Rectangle (
				fcw,
				0,
				Width - fcw,
				frh
			));
			AbsoluteLayout.SetLayoutFlags (frozenColumnPanel, AbsoluteLayoutFlags.None);
			AbsoluteLayout.SetLayoutBounds (frozenColumnPanel, new Rectangle (
				0,
				frh,
				fcw,
				Height - frh
			));
			AbsoluteLayout.SetLayoutFlags (cornerViewContainer, AbsoluteLayoutFlags.None);
			AbsoluteLayout.SetLayoutBounds (cornerViewContainer, new Rectangle (
				0,
				0,
				fcw,
				frh
			));
			AbsoluteLayout.SetLayoutFlags (mainPanel, AbsoluteLayoutFlags.None);
			AbsoluteLayout.SetLayoutBounds (mainPanel, new Rectangle (
				fcw,
				frh,
				Width - fcw,
				Height - frh
			));
		}

		protected override void OnSizeAllocated (double width, double height)
		{
            if (width < 0 || height < 0)
                return;
            base.OnSizeAllocated(width, height);

			layoutPanels ();

			updateVisibility ();

			// TODO should the following be necessary?  or are these calls implied by
			// the call to layoutPanels above?

			mainPanel.LayoutAllChildren();
			frozenRowPanel.LayoutAllChildren();
			frozenColumnPanel.LayoutAllChildren(); 
		}

		private void ensureColumnEdgesAreCached()
		{
			// TODO also maybe check if cachedColumnEdges.Length != numberOfColumns?
			if (cachedColumnEdges == null) {
				cachedColumnEdges = new double[numberOfColumns];
				double sofar = 0;
				for (int i = 0; i < cachedColumnEdges.Length; i++) {
					cachedColumnEdges [i] = sofar;
					sofar += getColumnWidthPlusSpacing(i);
				}
				cachedTotalWidth = sofar;
			}
		}

		private Rectangle mainGetBox(View v)
		{
			int col;
			int row;
			if (getCoordsForBoundView(v, out col, out row)) {
				ensureColumnEdgesAreCached ();
				double x = cachedColumnEdges[col];
				double y = row * rowHeightPlusSpacing;
				double width = getColumnWidth(col);
				double height = RowHeight;
				return new Rectangle (x - ContentOffset_X, y - ContentOffset_Y, width, height);
			} else {
				return new Rectangle (-1000, -1000, 0, 0);
			}
		}

		private Rectangle frozenColumnGetBox(View v)
		{
			int col;
			int row;
			if (getCoordsForBoundView(v, out col, out row)) {
				// assert col == -1
				double x = 0;
				double y = row * rowHeightPlusSpacing;
				double width = getColumnWidth(-1);
				double height = RowHeight;
				return new Rectangle (x, y - ContentOffset_Y, width, height);
			} else {
				return new Rectangle (-1000, -1000, 0, 0);
			}
		}

		private Rectangle frozenRowGetBox(View v)
		{
			int col = frozenRowPanel.Children.IndexOf(v);
			ensureColumnEdgesAreCached ();
			double x = cachedColumnEdges [col];
			double y = 0;
			double width = getColumnWidth(col);
			double height = HeaderHeight;
			return new Rectangle (x - ContentOffset_X, y, width, height);
		}

		private void bindCellView(View v, int col, int row)
		{
			if (!coordsToBoundView.ContainsKey (col)) {
				coordsToBoundView [col] = new Dictionary<int, View> ();
			}
			coordsToBoundView [col] [row] = v;
			boundViewToCoords [v] = new CoordPair (col, row);
			setBindingContext (v, getDataForCell (col, row));
			fixSelection (v, col, row);
		}

		private void bindVisibleCellsInColumn(int col, int row_first, int row_last)
		{
			for (int row=row_first; row<=row_last; row++)
			{
				View v;

				if (getBoundViewForCoords (col, row, out v)) {
					// v should already be prepped
				} else {
					v = findAvailableCellView (col);
					bindCellView (v, col, row);
				}
			}
		}
        bool bAlreadyUpdatingVisibility = false;
		private void updateVisibility()
		{
            if (bAlreadyUpdatingVisibility == true)
                return;

            bAlreadyUpdatingVisibility = true;
			int col_first;
			int col_last;
			int row_first;
			int row_last;

			calcVisibleColumns (
				ContentOffset_X,
				this.Width,
				out col_first,
				out col_last
			);

			calcVisibleRows (
				ContentOffset_Y,
				this.Height,
				out row_first,
				out row_last
			);

			// TODO handle cases where col/row first/last are not valid because
			// the width/height were 0 so nothing is visible

			// unbind anything that is not visible
			foreach (var col in coordsToBoundView.Keys) {
				var rowsForThisColumn = coordsToBoundView [col];
				if (
					(col >= 0)
					&& (
						(col < col_first)
						|| (col > col_last)
					)
				) 
				{
					// everything in this column is invisible.
					foreach (var row in rowsForThisColumn.Keys) {
						View v = rowsForThisColumn [row];
						boundViewToCoords.Remove (v);
						setBindingContext (v, null);
					}
					rowsForThisColumn.Clear ();
				} else {
					List<int> rm = new List<int> ();
					foreach (var row in rowsForThisColumn.Keys) {
						if (
							   (row < row_first)
							|| (row > row_last)
						) 
						{
							rm.Add (row);
							View v = rowsForThisColumn [row];
							boundViewToCoords.Remove (v);
							setBindingContext (v, null);
						}
					}
					foreach (var row in rm) {
						rowsForThisColumn.Remove (row);
					}
				}
			}
				
			// bind everything that is visible (if not bound already)
			if ((col_last >= col_first) && (col_first >= 0)) {
				for (int col = col_first; col <= col_last; col++) {
					bindVisibleCellsInColumn (col, row_first, row_last);
				}
			}
			if (getColumnWidthPlusSpacing(-1) > 0) {
				bindVisibleCellsInColumn (-1, row_first, row_last);
			}
            bAlreadyUpdatingVisibility = false;
		}

		internal void GetContentOffset(out double x, out double y)
		{
			x = ContentOffset_X;
			y = ContentOffset_Y;
		}

		private void my_SetContentOffset(double x, double y)
		{
			if (
				(ContentOffset_X != x) 
				|| (ContentOffset_Y != y)
			)
			{
				ContentOffset_X = x;
				ContentOffset_Y = y;

				updateVisibility ();

				mainPanel.LayoutAllChildren ();
				frozenRowPanel.LayoutAllChildren ();
				frozenColumnPanel.LayoutAllChildren ();
			}
		}

		private double ScrollXMax
		{
			get {
				var q = cachedTotalWidth;
				var fcw = getColumnWidthPlusSpacing(-1);
				if (fcw < 0) {
					fcw = 0;
				}
				q -= (this.Width - fcw);
				if (q < 0) {
					q = 0;
				}
				return q;
			}
		}

		private double ScrollYMax
		{
			get {
				var q = rowHeightPlusSpacing * numberOfRows;
				var frh = frozenRowHeightPlusSpacing;
				if (frh < 0) {
					frh = 0;
				}
				q -= (this.Height - frh);
				if (q < 0) {
					q = 0;
				}
				return q;
			}
		}

		internal void SetContentOffset(double x, double y)
		{
			// fix for constrained scroll range

			if (x < 0) {
				x = 0;
			} else {
				x = Math.Min (x, ScrollXMax);
			}
			if (y < 0) {
				y = 0;
			} else {
				y = Math.Min (y, ScrollYMax);
			}

			my_SetContentOffset (x, y);
		}

		private int findFirstVisibleColumn(double left)
		{
			for (int col = 0; col < cachedColumnEdges.Length; col++) {
				if ((cachedColumnEdges [col] + getColumnWidthPlusSpacing (col)) > left) {
					return col;
				}
			}
			return -1;
		}

		private int findLastVisibleColumn(int first, double end)
		{
			int last = first;
			for (int col = first+1; col < cachedColumnEdges.Length; col++) {
				if (cachedColumnEdges [col] > end) {
					break;
				} else {
					last = col;
				}
			}
			return last;
		}

		private void calcVisibleColumns(
			double left,
			double width,
			out int first, 
			out int last
		)
		{
			// assert left >= 0

			if (width <= 0) {
				first = -1;
				last = -1;
			}

			ensureColumnEdgesAreCached ();

			first = findFirstVisibleColumn (left);
			last = findLastVisibleColumn (first, left + width);
		}

		private void calcVisibleRows(
			double top,
			double height,
			out int first, 
			out int last
		)
		{
			// assert top >= 0

			if (height <= 0) {
				first = -1;
				last = -1;
			}

			double w = rowHeightPlusSpacing;
			first = (int)(top / w);
			last = (int)((top + height) / w);
			int num = numberOfRows;
			if (last > (num - 1)) {
				last = num - 1;
			}
		}

	#if TODO
	// App developers should use the method below in production code for 
	// better performance
	public static readonly BindableProperty BoundNameProperty =
		BindableProperty.Create ("Foo", typeof (string),
			typeof (MockBindableObject),
			default(string));
	#endif

		private void addListenersToColumnsCollection()
		{
			if (Columns != null) {
				if (Columns is ObservableCollection<Column>) {
					var ob = Columns as ObservableCollection<Column>;
					ob.CollectionChanged += handleColumnsCollectionChanged;
				}
				foreach (var c in Columns) {
					c.PropertyChanged += handleColumnPropertyChanged;
				}
			}
		}

		private void addListenersToFrozenColumn()
		{
			if (FrozenColumn != null) {
				FrozenColumn.PropertyChanged += handleFrozenColumnPropertyChanged;
			}
		}

		private void addListenersToRowsCollection()
		{
			if (Rows != null) {
				if (Rows is ObservableCollection<object>) {
					var ob = Rows as ObservableCollection<object>;
					ob.CollectionChanged += handleRowsCollectionChanged;
				}
			}
		}

		private int previousSelectedRowIndex = -1;
		void handlePropertyChanging (object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == DataGrid.RowsProperty.PropertyName) {
				if (Rows != null) {
					if (Rows is ObservableCollection<object>) {
						var ob = Rows as ObservableCollection<object>;
						ob.CollectionChanged -= handleRowsCollectionChanged;
					}
				}
			} else if (e.PropertyName == DataGrid.FrozenColumnProperty.PropertyName) {
				if (FrozenColumn != null) {
					FrozenColumn.PropertyChanged -= handleFrozenColumnPropertyChanged;
				}
			} else if (e.PropertyName == DataGrid.ColumnsProperty.PropertyName) {
				if (Columns != null) {
					if (Columns is ObservableCollection<Column>) {
						var ob = Columns as ObservableCollection<Column>;
						ob.CollectionChanged -= handleColumnsCollectionChanged;
					}
					foreach (var c in Columns) {
						c.PropertyChanged -= handleColumnPropertyChanged;
					}
				}
			} else if (e.PropertyName == DataGrid.SelectedRowIndexProperty.PropertyName) {
				previousSelectedRowIndex = SelectedRowIndex;
			}
		}

		void handlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == DataGrid.RowHeightProperty.PropertyName) {
				this.rowHeightChanged ();
			} else if (e.PropertyName == DataGrid.RowSpacingProperty.PropertyName) {
				this.rowSpacingChanged ();
			} else if (e.PropertyName == DataGrid.ColumnSpacingProperty.PropertyName) {
				this.columnSpacingChanged ();
			} else if (e.PropertyName == DataGrid.HeaderHeightProperty.PropertyName) {
				frozenRowHeightChanged ();
			} else if (e.PropertyName == DataGrid.RowsProperty.PropertyName) {
				addListenersToRowsCollection ();
				allRowsReplaced ();
			} else if (e.PropertyName == DataGrid.FrozenColumnProperty.PropertyName) {
				// the frozen column was replaced completely
				addListenersToFrozenColumn ();
				columnsReplaced (-1, 1);
			} else if (e.PropertyName == DataGrid.ColumnsProperty.PropertyName) {
				// the entire columns collection got replaced
				addListenersToColumnsCollection ();
				allColumnsReplaced ();
			} else if (e.PropertyName == DataGrid.SelectedRowIndexProperty.PropertyName) {
				if (previousSelectedRowIndex >= 0) {
					fixSelection (previousSelectedRowIndex);
				}
				fixSelection (SelectedRowIndex);
				// TODO fire an event?  Or just let somebody bind to this property?
			}
		}

		void handleColumnPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			int col = this.Columns.IndexOf ((Column) sender);
			if (e.PropertyName == Column.HeaderViewProperty.PropertyName) {
				columnHeaderViewChanged (col);
			} else if (e.PropertyName == Column.TemplateProperty.PropertyName) {
				columnTemplateChanged (col);
			} else if (e.PropertyName == Column.WidthProperty.PropertyName) {
				this.columnWidthChanged (col);
			}
		}

		void handleFrozenColumnPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Column.HeaderViewProperty.PropertyName) {
				this.columnHeaderViewChanged (-1);
			} else if (e.PropertyName == Column.TemplateProperty.PropertyName) {
				columnTemplateChanged (-1);
			} else if (e.PropertyName == Column.WidthProperty.PropertyName) {
				this.columnWidthChanged (-1);
			}
		}

		void handleColumnsCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
				columnsAdded (e.NewStartingIndex, e.NewItems.Count);
				foreach (Column c in e.NewItems) {
					c.PropertyChanged += handleColumnPropertyChanged;
				}
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
				// TODO
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
				columnsRemoved (e.OldStartingIndex, e.OldItems.Count);
				foreach (Column c in e.OldItems) {
					c.PropertyChanged -= handleColumnPropertyChanged;
				}
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
				if (e.OldStartingIndex >= 0) {
					columnsReplaced (e.OldStartingIndex, e.OldItems.Count);
				} else {
					// heavy handed, but this is all we can do.  if OldStartingIndex is not
					// valid, columnsReplaced() will misinterpret as replacing the
					// frozen col.
					allColumnsReplaced ();
				}
				foreach (Column c in e.OldItems) {
					c.PropertyChanged -= handleColumnPropertyChanged;
				}
				foreach (Column c in e.NewItems) {
					c.PropertyChanged += handleColumnPropertyChanged;
				}
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
				allColumnsReplaced ();
				#if not
				// TODO how to remove handleColumnPropertyChanged from old?
				// http://blog.stephencleary.com/2009/07/interpreting-notifycollectionchangedeve.html
				// says for reset, all fields not valid
				foreach (Column c in e.OldItems) {
					c.PropertyChanged -= handleColumnPropertyChanged;
				}
				#endif
				foreach (var c in Columns) {
					c.PropertyChanged += handleColumnPropertyChanged;
				}
				break;
			}
		}

		void handleRowsCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
				rowsAdded (e.NewStartingIndex, e.NewItems.Count);
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
				// TODO
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
				rowsRemoved (e.OldStartingIndex, e.OldItems.Count);
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
				rowsReplaced (e.OldStartingIndex, e.OldItems.Count);
				break;
			case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
				allRowsReplaced ();
				break;
			}
		}

		public static readonly BindableProperty RowHeightProperty =
			BindableProperty.Create<DataGrid,double>(
				p => p.RowHeight, 50);

		public double RowHeight {
			get { return (double)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty RowSpacingProperty =
			BindableProperty.Create<DataGrid,double>(
				p => p.RowSpacing, 2);

		public double RowSpacing {
			get { return (double)GetValue(RowSpacingProperty); }
			set { SetValue(RowSpacingProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty ColumnSpacingProperty =
			BindableProperty.Create<DataGrid,double>(
				p => p.ColumnSpacing, 2);

		public double ColumnSpacing {
			get { return (double)GetValue(ColumnSpacingProperty); }
			set { SetValue(ColumnSpacingProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty ColumnsProperty =
			BindableProperty.Create<DataGrid,IList<Column>>(
				p => p.Columns, null);

		public IList<Column> Columns {
			get { return (IList<Column>)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		public static readonly BindableProperty RowsProperty =
			BindableProperty.Create<DataGrid,IList<object>>(
				p => p.Rows, null);

		public IList<object> Rows {
			get { return (IList<object>)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

		public static readonly BindableProperty FrozenColumnProperty =
			BindableProperty.Create<DataGrid,Column>(
				p => p.FrozenColumn, null);

		public Column FrozenColumn {
			get { return (Column)GetValue(FrozenColumnProperty); }
			set { SetValue(FrozenColumnProperty, value); }
		}

		public static readonly BindableProperty HeaderHeightProperty =
			BindableProperty.Create<DataGrid,double>(
				p => p.HeaderHeight, 50);

		public double HeaderHeight {
			get { return (double)GetValue(HeaderHeightProperty); }
			set { SetValue(HeaderHeightProperty, value); } // TODO disallow invalid values
		}

		public static readonly BindableProperty SelectionModeProperty =
			BindableProperty.Create<DataGrid, SelMode>(
				p => p.SelectionMode, SelMode.None);

		public SelMode SelectionMode
		{
			get { return (SelMode)GetValue(SelectionModeProperty); }
			set { SetValue(SelectionModeProperty, value); }
		}

		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create<DataGrid, Color>(
				p => p.SelectedBackgroundColor, Color.Accent);

		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}

		public static readonly BindableProperty UnselectedBackgroundColorProperty =
			BindableProperty.Create<DataGrid, Color>(
				p => p.UnselectedBackgroundColor, Color.White);

		public Color UnselectedBackgroundColor
		{
			get { return (Color)GetValue(UnselectedBackgroundColorProperty); }
			set { SetValue(UnselectedBackgroundColorProperty, value); }
		}

		// the following property only makes sense when SelectionMode is
		// SelMode.Row, or some other future mode which allows selection
		// of at most one row.
		public static readonly BindableProperty SelectedRowIndexProperty =
			BindableProperty.Create<DataGrid, int>(
				p => p.SelectedRowIndex, -1);

		public int SelectedRowIndex
		{
			get { return (int)GetValue(SelectedRowIndexProperty); }
			set { SetValue(SelectedRowIndexProperty, value); } // TODO disallow invalid values
		}

        // note that the _defaultCornerView code below is only here
        // as an attempted workaround because IsClippedToBounds doesn't
        // seem to work properly in the WP8 version of Xamarin.Forms.

		View _defaultCornerView = null;
		private View cornerView {
			get {
				if (FrozenColumn != null) {
					if (FrozenColumn.HeaderView != null)
						return FrozenColumn.HeaderView;
					else
					{
						if (_defaultCornerView == null)
						{
							_defaultCornerView = new BoxView
							{
								Color = this.BackgroundColor,
								BackgroundColor = this.BackgroundColor
							};
						}
						return _defaultCornerView;
					}
				} else {
					return null;
				}
			}
		}

		private int numberOfColumns {
			get {
				if (null == Columns) {
					return 0;
				}
				return Columns.Count;
			}
		}

		private View getFrozenRowView (int col)
		{
			if (col >= 0) {
				var v = Columns [col].HeaderView;
				if (v == null) {
					// this function is not allowed to return null.
					v = new BoxView (); // TODO a friendlier default?
				}
				return v;
			} else {
				return cornerView;
			}
		}

		private double getColumnWidth (int col)
		{
			if (col < 0) {
				if (null == FrozenColumn) {
					return 0;
				}
				return FrozenColumn.Width;
			} else {
				return Columns [col].Width;
			}
		}

		private int numberOfRows {
			get {
				return Rows == null ? 0 : Rows.Count;
			}
		}

		private DataTemplate getTemplateForColumn (int col)
		{
			if (col < 0) {
				if (FrozenColumn != null) {
					return FrozenColumn.Template;
				} else { 
					return null;
				}
			} else {
				return Columns [col].Template;
			}
		}

		private object getDataForCell (int col, int row)
		{
			// TODO isn't it weird that col isn't used here anymore?
			return Rows [row];
		}

		internal bool SingleTap(double x, double y)
		{
			// TODO pretend that there is a setting which binds the "select row" command
			// to the "single tap " action.

			if (SelMode.Row == SelectionMode) {
				var x2 = x - mainPanel.X + ContentOffset_X;
				if ((x2 < 0) || (x2 > cachedTotalWidth)) {
					return false;
				}
				var y2 = y - mainPanel.Y + ContentOffset_Y;
				int row = (int)(y2 / rowHeightPlusSpacing);
				if ((row < 0) || (row >= numberOfRows)) {
					return false;
				}
				// when we get a tap on the row that is already selected, we unselect it.
				// TODO this could be a matter of policy
				if (row == SelectedRowIndex) {
					this.SelectedRowIndex = -1;
				} else {
					this.SelectedRowIndex = row;
				}
				return true;
			} else {
				return false;
			}
		}
	}
}


