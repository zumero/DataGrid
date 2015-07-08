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

using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Widget;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Zumero.DataGrid), typeof(Zumero.DataGridRenderer))]

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
        public DataGridRenderer()
        {
            ViewConfiguration configuration = ViewConfiguration.Get(this.Context);
            mTouchSlop = configuration.ScaledTouchSlop;
        }


        private DataGrid tab // TODO
        {
            get
            {
                return Element;
            }
        }

        private double _began_x;
        private double _began_y;

        bool mIsBeingDragged = false;
        private float mLastMotionY;
        private float mLastMotionX;
        private float mFirstMotionY;
        private float mFirstMotionX;
        private int mTouchSlop;
        public override bool OnInterceptTouchEvent(MotionEvent e)
        {
            //Android.Util.Log.Debug("OnInterceptTouchEvent", "OnInterceptTouchEvent " + e.ToString());

            if ((e.Action == MotionEventActions.Move) && (mIsBeingDragged))
            {
                return true;
            }

            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Move:
                    {
                        float y = e.GetY();
                        float x = e.GetX();
                        int yDiff = (int)Math.Abs(y - mLastMotionY);
                        int xDiff = (int)Math.Abs(x - mLastMotionX);
                        if (yDiff > mTouchSlop || xDiff > mTouchSlop)
                        {
                            mIsBeingDragged = true;
                            mLastMotionX = x;
                            mLastMotionY = y;
                        }
                        break;
                    }

                case MotionEventActions.Down:
                    {
                        mLastMotionX = mFirstMotionX = e.GetX();
                        mLastMotionY = mFirstMotionY = e.GetY();

                        tab.GetContentOffset(out _began_x, out _began_y);
                        break;
                    }

                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    /* Release the drag */
                    mIsBeingDragged = false;
                    break;
            }

            /*
            * The only time we want to intercept motion events is if we are in the
            * drag mode.
            */
            return mIsBeingDragged;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            //Android.Util.Log.Debug("OnTouchEvent", "OnTouchEvent " + e.ToString());

            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    {
                        // Remember where the motion event started
                        mLastMotionY = e.GetY();
                        mLastMotionX = e.GetX();

                        tab.GetContentOffset(out _began_x, out _began_y);

                        break;
                    }
                case MotionEventActions.Move:

                    float y = e.GetY();
                    float x = e.GetX();
                    float deltaY = mLastMotionY - y;
                    float deltaX = mLastMotionX - x;
                    if (!mIsBeingDragged && (Math.Abs(deltaY) > mTouchSlop || Math.Abs(deltaX) > mTouchSlop))
                    {
                        mIsBeingDragged = true;
                    }

                    if (mIsBeingDragged)
                    {
                        //Actually perform the scroll
                        double tr_x = Xamarin.Forms.Platform.Android.ContextExtensions.FromPixels(this.Context, x - mFirstMotionX);
                        double tr_y = Xamarin.Forms.Platform.Android.ContextExtensions.FromPixels(this.Context, y - mFirstMotionY);

                        double newx = _began_x - tr_x;
                        double newy = _began_y - tr_y;

                        //Android.Util.Log.Debug("ActualScroll", "deltaX " + deltaX + " deltaY " + deltaY + " newx " + newx + " newy " + newy);
                        //Android.Util.Log.Debug("ActualScroll", "began_x " + _began_x + " tr_x " + tr_x +" began_y " + _began_y + " tr_y " + tr_y);
                        //Android.Util.Log.Debug("ActualScroll", "mLastMotionX " + mLastMotionX + " x " + x + " mLastMotionY " + mLastMotionY + " y " + y);

                        mLastMotionY = y;
                        mLastMotionX = x;

                        tab.SetContentOffset(newx, newy);
                    }
                    break;
                case MotionEventActions.Up:
                    if (mIsBeingDragged)
                    {
                        mIsBeingDragged = false;
                        mLastMotionY = 0;
                        mLastMotionX = 0;
                    }
                    else
                    {
                        //If they are releasing the touch, they didn't drag, and none of our children caught it, call it a tap event.
                        double touch_x = Xamarin.Forms.Platform.Android.ContextExtensions.FromPixels(this.Context, e.GetX());
                        double touch_y = Xamarin.Forms.Platform.Android.ContextExtensions.FromPixels(this.Context, e.GetY());

                        tab.SingleTap(touch_x, touch_y); // TODO shouldn't we use the return value of this call?
                    }
                    break;
                case MotionEventActions.Cancel:
                    if (mIsBeingDragged)
                    {
                        mIsBeingDragged = false;
                        mLastMotionY = 0;
                        mLastMotionX = 0;
                    }
                    break;
            }
            return true;
        }
    }
}

