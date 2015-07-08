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

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Zumero.DataGrid), typeof(Zumero.DataGridRenderer))]

namespace Zumero
{
	public class DataGridComponent
	{
		public static void Init()
		{
		}
	}

    public class DataGridRenderer : ViewRenderer
	{
        public DataGridRenderer()
        {
            this.ManipulationStarted += TabularRenderer_ManipulationStarted;
            this.ManipulationDelta += TabularRenderer_ManipulationDelta;
			this.Tap += DataGridRenderer_Tap;
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
        
        void TabularRenderer_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            tab.SetContentOffset(_began_x - e.CumulativeManipulation.Translation.X, _began_y - e.CumulativeManipulation.Translation.Y);
        }
        
        void TabularRenderer_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (this.Control != null && this.Control.CacheMode == null)
                this.Control.CacheMode = new BitmapCache();
            tab.GetContentOffset(out _began_x, out _began_y); 
        }

		void DataGridRenderer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			tab.SingleTap(e.GetPosition(this).X, e.GetPosition(this).Y);
			e.Handled = false;
		}
	}

}

