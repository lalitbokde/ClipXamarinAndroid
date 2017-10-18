package md5214dfe80ae490f6bc9a74e75651ef416;


public class CommentActivity_GetEventFeed_PostFeedData
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
		mono.android.Runtime.register ("WeClip.Droid.CommentActivity+GetEventFeed+PostFeedData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", CommentActivity_GetEventFeed_PostFeedData.class, __md_methods);
	}


	public CommentActivity_GetEventFeed_PostFeedData () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CommentActivity_GetEventFeed_PostFeedData.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.CommentActivity+GetEventFeed+PostFeedData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
