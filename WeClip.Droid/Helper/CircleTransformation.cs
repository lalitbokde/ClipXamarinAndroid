using System;
using Android.Graphics;
using Android.Runtime;
using Android.Views.Animations;
using Square.Picasso;

namespace WeClip.Droid.Helper
{
    public class CircleTransformation : Java.Lang.Object, ITransformation
    {
        public string Key
        {
            get
            {
                return "circle";
            }
        }

        public Bitmap Transform(Bitmap source)
        {
            int size = Math.Min(source.Width, source.Height);

            int x = (source.Width - size) / 2;
            int y = (source.Height - size) / 2;

            Bitmap squaredBitmap = Bitmap.CreateBitmap(source, x, y, size, size);
            if (squaredBitmap != source)
            {
                source.Recycle();
            }

            Bitmap bitmap = Bitmap.CreateBitmap(size, size, source.GetConfig());

            Canvas canvas = new Canvas(bitmap);
            Paint paint = new Paint();
            BitmapShader shader = new BitmapShader(squaredBitmap,
                    BitmapShader.TileMode.Clamp, BitmapShader.TileMode.Clamp);
            paint.SetShader(shader);
            paint.AntiAlias = (true);

            float r = size / 2f;
            canvas.DrawCircle(r, r, r, paint);

            squaredBitmap.Recycle();
            return bitmap;
        }
    }
}
