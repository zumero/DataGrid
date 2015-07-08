
# Zumero DataGrid for Xamarin.Forms

A Xamarin.Forms control for displaying data in rows and columns.

## Is this open source?

Yes.  Apache License v2.

## What are its features?

- Support for cell contents to be any Xamarin.Forms.View
- Scrolling, both horizontal and vertical
- Optional top frozen header row
- Optional left frozen column
- Separate View template for each column
- View recycling (If you have 1000 rows but only 10 can be visible, DataGrid creates 10 Views and reuses them.)
- Data row objects (provided to the DataGrid as an IList&lt;object&gt;)
- Strong integration with the Xamarin.Forms binding mechanism
- Automatic updates if an ObservableCollection is used for rows and columns
- Configurable row height (for all rows in the grid)
- Configurable column widths (can be different for each column)
- Configurable spacing between rows and columns
- Row selections
- Works with C#, F#, and XAML
- Support for iOS, Android, and Windows Phone
- Full API documentation in MonoDoc

## Is there documentation?

Yes:

In the component directory, see GettingStarted.md.

In the DataGrid directory, see the subdirectory called docs, which contains full API documentation in monodoc format.

## Does this support for the WinRT-flavored platforms?

Not yet.  This work is in progress.  Initial code is in DataGrid.Windows and DataGrid.WinPhone81.


