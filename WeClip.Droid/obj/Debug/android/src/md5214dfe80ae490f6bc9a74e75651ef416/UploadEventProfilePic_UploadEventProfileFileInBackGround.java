package md5214dfe80ae490f6bc9a74e75651ef416;


public class UploadEventProfilePic_UploadEventProfileFileInBackGround
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
		mono.android.Runtime.register ("WeClip.Droid.UploadEventProfilePic+UploadEventProfileFileInBackGround, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", UploadEventProfilePic_UploadEventProfileFileInBackGround.class, __md_methods);
	}


	public UploadEventProfilePic_UploadEventProfileFileInBackGround () throws java.lang.Throwable
	{
		super ();
		if (getClass () == UploadEventProfilePic_UploadEventProfileFileInBackGround.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.UploadEventProfilePic+UploadEventProfileFileInBackGround, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public UploadEventProfilePic_UploadEventProfileFileInBackGround (md5214dfe80ae490f6bc9a74e75651ef416.UploadEventProfilePic p0, long p1, java.lang.String p2, long p3) throws java.lang.Throwable
	{
		super ();
		if (getClass () == UploadEventProfilePic_UploadEventProfileFileInBackGround.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.UploadEventProfilePic+UploadEventProfileFileInBackGround, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "WeClip.Droid.UploadEventProfilePic, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
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