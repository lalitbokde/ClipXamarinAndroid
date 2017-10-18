package md5db1088f0bca3a95a44585b5bf51de6f6;


public class PackageViewAdapter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PackageViewAdapter, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", PackageViewAdapter.class, __md_methods);
	}


	public PackageViewAdapter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PackageViewAdapter.class)
			mono.android.TypeManager.Activate ("PackageViewAdapter, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
