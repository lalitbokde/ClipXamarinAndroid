package md5214dfe80ae490f6bc9a74e75651ef416;


public class CommentActivity_GetEventFeed
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
		mono.android.Runtime.register ("WeClip.Droid.CommentActivity+GetEventFeed, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", CommentActivity_GetEventFeed.class, __md_methods);
	}


	public CommentActivity_GetEventFeed () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CommentActivity_GetEventFeed.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.CommentActivity+GetEventFeed, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CommentActivity_GetEventFeed (android.widget.ListView p0, md5214dfe80ae490f6bc9a74e75651ef416.CommentActivity p1, long p2, android.widget.Button p3, android.widget.EditText p4) throws java.lang.Throwable
	{
		super ();
		if (getClass () == CommentActivity_GetEventFeed.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.CommentActivity+GetEventFeed, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.ListView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:WeClip.Droid.CommentActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Widget.Button, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.EditText, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3, p4 });
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
