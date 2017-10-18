package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetDefultEventFile_btnUploadPhotoClick
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
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetDefultEventFile+btnUploadPhotoClick, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetDefultEventFile_btnUploadPhotoClick.class, __md_methods);
	}


	public GetDefultEventFile_btnUploadPhotoClick () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetDefultEventFile_btnUploadPhotoClick.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetDefultEventFile+btnUploadPhotoClick, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetDefultEventFile_btnUploadPhotoClick (android.widget.ImageView p0, android.app.Activity p1, long p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetDefultEventFile_btnUploadPhotoClick.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetDefultEventFile+btnUploadPhotoClick, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.ImageView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
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
