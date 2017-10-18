package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetProfileDetails_GetAllEventDetails
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"n_onPostExecute:(Ljava/lang/Object;)V:GetOnPostExecute_Ljava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetProfileDetails+GetAllEventDetails, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetProfileDetails_GetAllEventDetails.class, __md_methods);
	}


	public GetProfileDetails_GetAllEventDetails () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetProfileDetails_GetAllEventDetails.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetProfileDetails+GetAllEventDetails, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetProfileDetails_GetAllEventDetails (android.view.View p0, android.support.v4.app.FragmentActivity p1, long p2, android.widget.ListView p3) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetProfileDetails_GetAllEventDetails.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetProfileDetails+GetAllEventDetails, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Widget.ListView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public java.lang.Object doInBackground (java.lang.Object[] p0)
	{
		return n_doInBackground (p0);
	}

	private native java.lang.Object n_doInBackground (java.lang.Object[] p0);


	public void onPostExecute (java.lang.Object p0)
	{
		n_onPostExecute (p0);
	}

	private native void n_onPostExecute (java.lang.Object p0);

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
