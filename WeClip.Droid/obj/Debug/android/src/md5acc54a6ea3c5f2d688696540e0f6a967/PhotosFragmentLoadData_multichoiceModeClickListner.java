package md5acc54a6ea3c5f2d688696540e0f6a967;


public class PhotosFragmentLoadData_multichoiceModeClickListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.AbsListView.MultiChoiceModeListener,
		android.view.ActionMode.Callback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onItemCheckedStateChanged:(Landroid/view/ActionMode;IJZ)V:GetOnItemCheckedStateChanged_Landroid_view_ActionMode_IJZHandler:Android.Widget.AbsListView/IMultiChoiceModeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onActionItemClicked:(Landroid/view/ActionMode;Landroid/view/MenuItem;)Z:GetOnActionItemClicked_Landroid_view_ActionMode_Landroid_view_MenuItem_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onCreateActionMode:(Landroid/view/ActionMode;Landroid/view/Menu;)Z:GetOnCreateActionMode_Landroid_view_ActionMode_Landroid_view_Menu_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onDestroyActionMode:(Landroid/view/ActionMode;)V:GetOnDestroyActionMode_Landroid_view_ActionMode_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onPrepareActionMode:(Landroid/view/ActionMode;Landroid/view/Menu;)Z:GetOnPrepareActionMode_Landroid_view_ActionMode_Landroid_view_Menu_Handler:Android.Views.ActionMode/ICallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.PhotosFragmentLoadData+multichoiceModeClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", PhotosFragmentLoadData_multichoiceModeClickListner.class, __md_methods);
	}


	public PhotosFragmentLoadData_multichoiceModeClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PhotosFragmentLoadData_multichoiceModeClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.PhotosFragmentLoadData+multichoiceModeClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onItemCheckedStateChanged (android.view.ActionMode p0, int p1, long p2, boolean p3)
	{
		n_onItemCheckedStateChanged (p0, p1, p2, p3);
	}

	private native void n_onItemCheckedStateChanged (android.view.ActionMode p0, int p1, long p2, boolean p3);


	public boolean onActionItemClicked (android.view.ActionMode p0, android.view.MenuItem p1)
	{
		return n_onActionItemClicked (p0, p1);
	}

	private native boolean n_onActionItemClicked (android.view.ActionMode p0, android.view.MenuItem p1);


	public boolean onCreateActionMode (android.view.ActionMode p0, android.view.Menu p1)
	{
		return n_onCreateActionMode (p0, p1);
	}

	private native boolean n_onCreateActionMode (android.view.ActionMode p0, android.view.Menu p1);


	public void onDestroyActionMode (android.view.ActionMode p0)
	{
		n_onDestroyActionMode (p0);
	}

	private native void n_onDestroyActionMode (android.view.ActionMode p0);


	public boolean onPrepareActionMode (android.view.ActionMode p0, android.view.Menu p1)
	{
		return n_onPrepareActionMode (p0, p1);
	}

	private native boolean n_onPrepareActionMode (android.view.ActionMode p0, android.view.Menu p1);

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
