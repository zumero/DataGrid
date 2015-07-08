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

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Zumero.DataGrid), typeof(Zumero.DataGridRenderer))]

namespace Zumero
{
	public class DataGridComponent
	{
		public static void Init()
		{
		}
	}

	public class DataGridRenderer : VisualElementRenderer<Zumero.DataGrid>
	{

		private DataGrid tab // TODO
		{
			get {
				return Element;
			}
		}

		private void on_singletap(UITapGestureRecognizer gr)
		{
			var wh = gr.LocationInView (this);
			tab.SingleTap (wh.X, wh.Y); // TODO do we need to use the return value of this call?
		}

		private class pan_gr : UIPanGestureRecognizer
		{
			DataGridRenderer foo;
			double _began_x;
			double _began_y;

			public pan_gr(DataGridRenderer _foo) : base(on_fire)
			{
				foo = _foo;
			}

			private static void on_fire(UIPanGestureRecognizer gr)
			{
				pan_gr me = gr as pan_gr;
				if (gr.State == UIGestureRecognizerState.Began) {
					me.foo.tab.GetContentOffset (out me._began_x, out me._began_y);
				} else if (gr.State == UIGestureRecognizerState.Changed) {
					var pt = gr.TranslationInView (me.foo);

					double x = me._began_x - pt.X;
					double y = me._began_y - pt.Y;

					me.foo.tab.SetContentOffset (x, y);
				}
			}
		}

		public DataGridRenderer() : base()
		{
			var gr_pan = new pan_gr(this);
			gr_pan.MaximumNumberOfTouches = 1;
			// TODO or maybe two fingers should be scroll but one finger is select?
			this.AddGestureRecognizer(gr_pan);

			var gr_singletap_datacells = new UITapGestureRecognizer(on_singletap);
			gr_singletap_datacells.NumberOfTapsRequired = 1;

			#if not // TODO worth noting that we don't have double tap or long press

			var gr_doubletap_datacells = new UITapGestureRecognizer(on_doubletap);
			gr_doubletap_datacells.NumberOfTapsRequired = 2;

			gr_singletap_datacells.RequireGestureRecognizerToFail(gr_doubletap_datacells);

			var gr_longpress_datacells = new UILongPressGestureRecognizer(on_longpress);

			this.AddGestureRecognizer(gr_longpress_datacells);
			this.AddGestureRecognizer(gr_doubletap_datacells);

			#endif

			this.AddGestureRecognizer(gr_singletap_datacells);
		}

	}
}
