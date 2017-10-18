package md5acc54a6ea3c5f2d688696540e0f6a967;


public class postEventResponse
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPreExecute:()V:GetOnPreExecuteHandler\n" +
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"n_onPostExecute:(Ljava/lang/Object;)V:GetOnPostExecute_Ljava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.postEventResponse, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", postEventResponse.class, __md_methods);
	}


	public postEventResponse () throws java.lang.Throwable
	{
		super ();
		if (getClass () == postEventResponse.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.postEventResponse, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public postEventResponse (long p0, java.lang.String p1, android.support.v4.app.FragmentActivity p2, android.widget.Button p3, android.widget.Button p4, android.widget.Button p5, android.widget.TextView p6, android.widget.TextView p7) throws java.lang.Throwable
	{
		super ();
		if (getClass () == postEventResponse.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.postEventResponse, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Widget.Button, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.Button, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.Button, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}


	public void onPreExecute ()
	{
		n_onPreExecute ();
	}

	private native void n_onPreExecute ();


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
