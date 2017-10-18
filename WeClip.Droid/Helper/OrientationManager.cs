using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Android.Content.PM;

namespace WeClip.Droid.Helper
{
    public class OrientationManager : OrientationEventListener
    {
        public enum ScreenOrientation
        {
            REVERSED_LANDSCAPE, LANDSCAPE, PORTRAIT, REVERSED_PORTRAIT
        }

        public ScreenOrientation screenOrientation;
        private OrientationListener listener;

        public OrientationManager(Context context) : base(context)
        {
        }

        public OrientationManager(Context context, [GeneratedEnum] SensorDelay rate) : base(context, rate)
        {
        }

        public OrientationManager(Context context, [GeneratedEnum] SensorDelay rate, OrientationListener listener) : base(context, rate)
        {
            SetListener(listener);
        }

        public void SetListener(OrientationListener listener)
        {
            this.listener = listener;
        }

        public override void OnOrientationChanged(int orientation)
        {
            if (orientation == -1)
            {
                return;
            }

            ScreenOrientation newOrientation;

            if (orientation >= 60 && orientation <= 140)
            {
                newOrientation = ScreenOrientation.REVERSED_LANDSCAPE;
            }
            else if (orientation >= 140 && orientation <= 220)
            {
                newOrientation = ScreenOrientation.REVERSED_PORTRAIT;
            }
            else if (orientation >= 220 && orientation <= 300)
            {
                newOrientation = ScreenOrientation.LANDSCAPE;
            }
            else
            {
                newOrientation = ScreenOrientation.PORTRAIT;
            }

            if (newOrientation != screenOrientation)
            {
                screenOrientation = newOrientation;
                if (listener != null)
                {
                    listener.OnOrientationChange(screenOrientation);
                }
            }
        }

        public ScreenOrientation GetScreenOrientation()
        {
            return screenOrientation;
        }

        public interface OrientationListener
        {
            void OnOrientationChange(ScreenOrientation screenOrientation);
        }
    }
}