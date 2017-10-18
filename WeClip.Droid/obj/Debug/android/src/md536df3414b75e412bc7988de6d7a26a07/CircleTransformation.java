package md536df3414b75e412bc7988de6d7a26a07;


public class CircleTransformation
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.squareup.picasso.Transformation
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_key:()Ljava/lang/String;:GetGetKeyHandler:Square.Picasso.ITransformationInvoker, Square.Picasso\n" +
			"n_transform:(Landroid/graphics/Bitmap;)Landroid/graphics/Bitmap;:GetTransform_Landroid_graphics_Bitmap_Handler:Square.Picasso.ITransformationInvoker, Square.Picasso\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Helper.CircleTransformation, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", CircleTransformation.class, __md_methods);
	}


	public CircleTransformation () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CircleTransformation.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.CircleTransformation, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public java.lang.String key ()
	{
		return n_key ();
	}

	private native java.lang.String n_key ();


	public android.graphics.Bitmap transform (android.graphics.Bitmap p0)
	{
		return n_transform (p0);
	}

	private native android.graphics.Bitmap n_transform (android.graphics.Bitmap p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
