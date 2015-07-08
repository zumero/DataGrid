
using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.WinRT;
using System.Windows;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


[assembly: ExportRenderer(typeof(Zumero.DataGrid), typeof(Zumero.DataGridRenderer))]

namespace Zumero
{
	public class DataGridComponent
	{
		public static void Init()
		{
		}
	}

    public class DataGridRenderer : ViewRenderer<Zumero.DataGrid, Windows.UI.Xaml.Controls.Canvas>
	{
        public DataGridRenderer()
        {
            this.ManipulationMode = ManipulationModes.All;
            this.ManipulationStarted += TabularRenderer_ManipulationStarted;
            this.ManipulationDelta += TabularRenderer_ManipulationDelta;
            //this.SizeChanged += DataGridRenderer_SizeChanged;
            this.Tapped += DataGridRenderer_Tap;
            //this.LayoutUpdated += DataGridRenderer_LayoutUpdated;
            //this.Clip = null;
            //this.Loaded += DataGridRenderer_Loaded;
        }

        void DataGridRenderer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            //TODO: This hack will trigger a redraw cycle to make sure that all of
            //the grid parts get drawn correctly.
            /*tab.RowHeight += 1;
            tab.RowHeight -= 1;
            tab.ColumnSpacing += 1;
            tab.ColumnSpacing -= 1;*/
            
            if (tab != null)
            {
                //tab.stackTrace.Push("render Loaded");
                //tab.Layout(new Rectangle(tab.X, tab.Y, tab.Width, tab.Height));
                //tab.DumpCurrentSizes("Loaded");
                //tab.stackTrace.Pop();
            }
/*            tab.FrozenColumn = new Column()
            {
                Width = 80,
                HeaderView = new Label
                {
                    Text = "Corner",
                    BackgroundColor = Color.Yellow,
                },
                Template = new DataTemplate(() =>
                {
                    var v = new Label
                    {
                        BackgroundColor = Color.Gray,
                        TextColor = Color.Black,
                        XAlign = TextAlignment.Center,
                        YAlign = TextAlignment.Center,
                    };
                    v.SetBinding(Label.TextProperty, "Spanish");
                    return v;
                }),
            };*/
            //tab.DumpCurrentSizes("Loaded");

        }

        void DataGridRenderer_LayoutUpdated(object sender, object e)
        {
            if (tab != null)
            {
                //tab.stackTrace.Push("render LayoutUpdated");
                //tab.Layout(new Rectangle(tab.X, tab.Y, tab.Width, tab.Height));
                //tab.stackTrace.Pop();
            //    tab.DumpCurrentSizes("Layout updated");
            }
        }

        void DataGridRenderer_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            //tab.stackTrace.Push("render SizeChanged");
            //tab.Layout(new Rectangle(tab.X, tab.Y, e.NewSize.Width, e.NewSize.Height));
            //tab.stackTrace.Pop();
            //this.UpdateNativeControl();
        }
        
		private DataGrid tab
        {
            get
            {
				return (Zumero.DataGrid)Element;
            }
        }

        private double _began_x;
        private double _began_y;
        private bool bFirstTime;

        
        void TabularRenderer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            tab.SetContentOffset(_began_x - e.Cumulative.Translation.X, _began_y - e.Cumulative.Translation.Y);
        }

        void TabularRenderer_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            //if (this.Control != null && this.Control.CacheMode == null)
                //this.Control.CacheMode = new BitmapCache();
            tab.GetContentOffset(out _began_x, out _began_y);
        }
        

        void DataGridRenderer_Tap(object sender, TappedRoutedEventArgs e)
        {
            tab.SingleTap(e.GetPosition(this).X, e.GetPosition(this).Y);
            e.Handled = false;
        }
                
	}

}