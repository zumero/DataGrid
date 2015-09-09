using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;
using demo;

namespace demo.Windows
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.HorizontalContentAlignment = global::Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.HorizontalAlignment = global::Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Stretch;
            this.VerticalContentAlignment = global::Windows.UI.Xaml.VerticalAlignment.Stretch;
            Zumero.DataGridComponent.Init();

            
            LoadApplication(new demo.App());
        }
    }
}
