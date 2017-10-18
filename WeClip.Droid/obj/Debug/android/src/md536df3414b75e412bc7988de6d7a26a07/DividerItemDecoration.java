package md536df3414b75e412bc7988de6d7a26a07;


public class DividerItemDecoration
	extends android.support.v7.widget.RecyclerView.ItemDecoration
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;Landroid/support/v7/widget/RecyclerView;Landroid/support/v7/widget/RecyclerView$State;)V:GetOnDraw_Landroid_graphics_Canvas_Landroid_support_v7_widget_RecyclerView_Landroid_support_v7_widget_RecyclerView_State_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Helper.DividerItemDecoration, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", DividerItemDecoration.class, __md_methods);
	}


	public DividerItemDecoration () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DividerItemDecoration.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.DividerItemDecoration, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DividerItemDecoration (android.content.Context p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DividerItemDecoration.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.DividerItemDecoration, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onDraw (android.graphics.Canvas p0, android.support.v7.widget.RecyclerView p1, android.support.v7.widget.RecyclerView.State p2)
	{
		n_onDraw (p0, p1, p2);
	}

	private native void n_onDraw (android.graphics.Canvas p0, android.support.v7.widget.RecyclerView p1, android.support.v7.widget.RecyclerView.State p2);

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
