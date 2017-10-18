package md536df3414b75e412bc7988de6d7a26a07;


public class VerticalSpaceItemDecoration
	extends android.support.v7.widget.RecyclerView.ItemDecoration
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemOffsets:(Landroid/graphics/Rect;Landroid/view/View;Landroid/support/v7/widget/RecyclerView;Landroid/support/v7/widget/RecyclerView$State;)V:GetGetItemOffsets_Landroid_graphics_Rect_Landroid_view_View_Landroid_support_v7_widget_RecyclerView_Landroid_support_v7_widget_RecyclerView_State_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Helper.VerticalSpaceItemDecoration, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", VerticalSpaceItemDecoration.class, __md_methods);
	}


	public VerticalSpaceItemDecoration () throws java.lang.Throwable
	{
		super ();
		if (getClass () == VerticalSpaceItemDecoration.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.VerticalSpaceItemDecoration, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public VerticalSpaceItemDecoration (int p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == VerticalSpaceItemDecoration.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.VerticalSpaceItemDecoration, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public void getItemOffsets (android.graphics.Rect p0, android.view.View p1, android.support.v7.widget.RecyclerView p2, android.support.v7.widget.RecyclerView.State p3)
	{
		n_getItemOffsets (p0, p1, p2, p3);
	}

	private native void n_getItemOffsets (android.graphics.Rect p0, android.view.View p1, android.support.v7.widget.RecyclerView p2, android.support.v7.widget.RecyclerView.State p3);

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
