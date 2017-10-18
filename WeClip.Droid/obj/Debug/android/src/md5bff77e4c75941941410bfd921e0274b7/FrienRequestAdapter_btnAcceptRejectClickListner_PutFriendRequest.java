package md5bff77e4c75941941410bfd921e0274b7;


public class FrienRequestAdapter_btnAcceptRejectClickListner_PutFriendRequest
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
		mono.android.Runtime.register ("WeClip.Droid.Adapters.FrienRequestAdapter+btnAcceptRejectClickListner+PutFriendRequest, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", FrienRequestAdapter_btnAcceptRejectClickListner_PutFriendRequest.class, __md_methods);
	}


	public FrienRequestAdapter_btnAcceptRejectClickListner_PutFriendRequest () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FrienRequestAdapter_btnAcceptRejectClickListner_PutFriendRequest.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Adapters.FrienRequestAdapter+btnAcceptRejectClickListner+PutFriendRequest, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
