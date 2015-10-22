
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
        public DataGridRenderer() : base()
        {
            this.ManipulationMode = ManipulationModes.All;
            this.ManipulationStarted += TabularRenderer_ManipulationStarted;
            this.ManipulationDelta += TabularRenderer_ManipulationDelta;

            this.Tapped += DataGridRenderer_Tap;
            this.PointerWheelChanged += Control_PointerWheelChanged;
            this.SizeChanged += DataGridRenderer_SizeChanged;
        }

        void DataGridRenderer_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            var r = new RectangleGeometry();
            Windows.Foundation.Rect rect = new Windows.Foundation.Rect(0, 0, this.ActualWidth, this.ActualHeight);
            r.Rect = rect;
            this.Clip = r;
        }

        void Control_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            tab.GetContentOffset(out _began_x, out _began_y);
            tab.SetContentOffset(_began_x, _began_y - e.GetCurrentPoint(this.Control).Properties.MouseWheelDelta);
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