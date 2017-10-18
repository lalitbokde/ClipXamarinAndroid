package md5bff77e4c75941941410bfd921e0274b7;


public class Contact_FriendAdapter_btnInviteClick
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Adapters.Contact_FriendAdapter+btnInviteClick, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", Contact_FriendAdapter_btnInviteClick.class, __md_methods);
	}


	public Contact_FriendAdapter_btnInviteClick () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Contact_FriendAdapter_btnInviteClick.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Adapters.Contact_FriendAdapter+btnInviteClick, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
