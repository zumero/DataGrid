
Zumero.DataGrid is a Xamarin.Forms control for displaying data in rows and columns.

## Features:

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

## XAML Snippet showing a column definition

```xml
<d:Column Width="120">
  <d:Column.HeaderView>
    <Label Text="Adjust X"></Label>
  </d:Column.HeaderView>
  <d:Column.Template>
    <DataTemplate>
      <Slider Value="{Binding X, Mode=TwoWay}" Minimum="-20" Maximum="20"></Slider>
    </DataTemplate>
  </d:Column.Template>
</d:Column>
```

