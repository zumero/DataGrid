# Getting Started with Zumero.DataGrid

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

## Referencing the component

Add the Zumero.DataGrid component to the Components folder of
each of your platform projects using the Xamarin Components
feature of your IDE.

Since PCL projects currently do not have a Xamarin Components
folder, if you are using a PCL project for portable parts of
your Xamarin.Forms project (and you probably are), you will
need to manually add a Reference to Zumero.DataGrid.dll.

To find this file, look (on the file system) in the directory containing your
.sln file and find the Components subdirectory, wherein the file you
want should be found at ZumeroDataGrid-VERSION/lib/pcl/Zumero.DataGrid.dll.

## Initialization

In order to use Zumero.DataGrid, you will need to call Zumero.DataGridComponent.Init()
from the platform-specific app code on each platform.  

We recommend that you simply insert the needed initialization call
directly below the call to Xamarin.Forms.Forms.Init().

For example, on iOS:

```csharp
public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
    Forms.Init ();
    Zumero.DataGridComponent.Init ();
    ...
```

## Configuring a DataGrid control

The two most important properties of the DataGrid control are
its Rows and Columns.

 - The Rows are the data.
 - The Columns describe *how* to display the data.

### Data Row Objects

The data for each row come from the properties of an object.
You can use any class you want to represent a row, but we
recommend using
a class that implements INotifyPropertyChanged.
This allows two-way binding, so that view controls
in your grid can update values in your row objects.

Here is an example of a class to represent a data row:

```csharp
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
```

The example above includes two properties, one of which is
read-only.

Here is the same example in F#:

```fsharp
type myRow() =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    let mutable x = 0.0

    member this.XSquared  
        with get() = x * x

    member this.X
        with get() = x
        and set(value:double) = 
            if x <> value then
                x <- value
                propertyChanged.Trigger(this, new PropertyChangedEventArgs("X"))
                propertyChanged.Trigger(this, new PropertyChangedEventArgs("XSquared"))

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChanged.Publish
```

The Rows property of DataGrid is IList&lt;object&gt;, so any
IList-conforming .NET collection can be used to represent
your list of row objects.  However, we recommend using
an ObservableCollecton so that DataGrid can listen
and automatically update things when rows are replaced,
deleted or added.

### Columns

The Zumero.Column class is used to represent a Column of
a DataGrid.  You can specify a Width for the column, as well
as a HeaderView (a view to be shown in the frozen row at
the top).  But the most important property of Zumero.Column
is the Template, which specifies what kind of view should be
used for the cells of this columna and how to bind that view
to properties of the data row.

Here is a C# example of a Column which takes the X property
from the data row and displays it in a Label view:

```csharp
using Zumero;
...
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
```

Note that the height of all the header views is
controlled by the HeaderHeight property of the DataGrid.

If you are trying to set headers for your columns and
they are not showing up, verify that the HeaderHeight of the DataGrid
is set appropriately (something greater than 0).

The default value of the DataGrid HeaderHeight is 50.  If you do not 
set the HeaderView property of any Columns, you will end up with 50 pixels 
of empty space at the top unless you override this default.  
Set the HeaderHeight to 0 if you don't want the header row.

### Putting Rows and Columns together

Below is a more complete example (in both C# and XAML) which 
constructs an entire DataGrid with three columns:

 - A Label bound to X (just like the example above)
 - A Label bound to XSquared
 - A Slider bound to X

Moving the Slider automatically changes the value of
the X property in the corresponding data row object,
which, in turn, sends notifications about its changed
properties, which causes the other two columns to be
updated as well.

```csharp
using Zumero;
...
new DataGrid {
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
```

XAML

```xml
<d:DataGrid x:Name="datagrid" Rows="{Binding DataRows}" BackgroundColor="Black" RowHeight="80">
  <d:DataGrid.Columns>

    <d:Column Width="80">
      <d:Column.HeaderView>
        <Label Text="X" BackgroundColor="Gray" TextColor="Black" XAlign="Center" YAlign="Center"></Label>
      </d:Column.HeaderView>
      <d:Column.Template>
        <DataTemplate>
          <Label Text="{Binding X}" BackgroundColor="White" TextColor="Black" XAlign="Center" YAlign="Center"></Label>
        </DataTemplate>
      </d:Column.Template>
    </d:Column>
  
    <d:Column Width="80">
      <d:Column.HeaderView>
        <Label Text="X^2" BackgroundColor="Gray" TextColor="Black" XAlign="Center" YAlign="Center"></Label>
      </d:Column.HeaderView>
      <d:Column.Template>
        <DataTemplate>
          <Label Text="{Binding XSquared}" BackgroundColor="White" TextColor="Black" XAlign="Center" YAlign="Center"></Label>
        </DataTemplate>
      </d:Column.Template>
    </d:Column>

    <d:Column Width="120">
      <d:Column.HeaderView>
        <Label Text="Slider" BackgroundColor="Gray" TextColor="Black" XAlign="Center" YAlign="Center"></Label>
      </d:Column.HeaderView>
      <d:Column.Template>
        <DataTemplate>
          <Slider Value="{Binding X, Mode=TwoWay}" BackgroundColor="White" Minimum="-20" Maximum="20"></Slider>
        </DataTemplate>
      </d:Column.Template>
    </d:Column>

  </d:DataGrid.Columns>
</d:DataGrid>
```

