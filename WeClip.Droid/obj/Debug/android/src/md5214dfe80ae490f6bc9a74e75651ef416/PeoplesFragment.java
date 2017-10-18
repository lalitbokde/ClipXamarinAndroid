package md5214dfe80ae490f6bc9a74e75651ef416;


public class PeoplesFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer,
		android.support.v7.widget.SearchView.OnQueryTextListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"n_onStart:()V:GetOnStartHandler\n" +
			"n_onCreateOptionsMenu:(Landroid/view/Menu;Landroid/view/MenuInflater;)V:GetOnCreateOptionsMenu_Landroid_view_Menu_Landroid_view_MenuInflater_Handler\n" +
			"n_onQueryTextChange:(Ljava/lang/String;)Z:GetOnQueryTextChange_Ljava_lang_String_Handler:Android.Support.V7.Widget.SearchView/IOnQueryTextListenerInvoker, Xamarin.Android.Support.v7.AppCompat\n" +
			"n_onQueryTextSubmit:(Ljava/lang/String;)Z:GetOnQueryTextSubmit_Ljava_lang_String_Handler:Android.Support.V7.Widget.SearchView/IOnQueryTextListenerInvoker, Xamarin.Android.Support.v7.AppCompat\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.PeoplesFragment, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", PeoplesFragment.class, __md_methods);
	}


	public PeoplesFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PeoplesFragment.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.PeoplesFragment, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);


	public void onStart ()
	{
		n_onStart ();
	}

	private native void n_onStart ();


	public void onCreateOptionsMenu (android.view.Menu p0, android.view.MenuInflater p1)
	{
		n_onCreateOptionsMenu (p0, p1);
	}

	private native void n_onCreateOptionsMenu (android.view.Menu p0, android.view.MenuInflater p1);


	public boolean onQueryTextChange (java.lang.String p0)
	{
		return n_onQueryTextChange (p0);
	}

	private native boolean n_onQueryTextChange (java.lang.String p0);


	public boolean onQueryTextSubmit (java.lang.String p0)
	{
		return n_onQueryTextSubmit (p0);
	}

	private native boolean n_onQueryTextSubmit (java.lang.String p0);

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
