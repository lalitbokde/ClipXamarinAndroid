package md5eb1a5ae087265d49fede0aa723ec4408;


public class ExpandableTextView_LessClickableSpan
	extends android.text.style.ClickableSpan
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Controls.ExpandableTextView+LessClickableSpan, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", ExpandableTextView_LessClickableSpan.class, __md_methods);
	}


	public ExpandableTextView_LessClickableSpan () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ExpandableTextView_LessClickableSpan.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Controls.ExpandableTextView+LessClickableSpan, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ExpandableTextView_LessClickableSpan (md5eb1a5ae087265d49fede0aa723ec4408.ExpandableTextView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ExpandableTextView_LessClickableSpan.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Controls.ExpandableTextView+LessClickableSpan, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "WeClip.Droid.Controls.ExpandableTextView, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
