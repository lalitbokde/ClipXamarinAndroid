package md5db1088f0bca3a95a44585b5bf51de6f6;


public class AudioViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AudioViewHolder, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", AudioViewHolder.class, __md_methods);
	}


	public AudioViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AudioViewHolder.class)
			mono.android.TypeManager.Activate ("AudioViewHolder, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
