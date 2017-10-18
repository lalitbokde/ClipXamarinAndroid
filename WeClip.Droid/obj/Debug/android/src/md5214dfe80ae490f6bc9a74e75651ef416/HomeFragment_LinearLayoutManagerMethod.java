package md5214dfe80ae490f6bc9a74e75651ef416;


public class HomeFragment_LinearLayoutManagerMethod
	extends android.support.v7.widget.LinearLayoutManager
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_canScrollVertically:()Z:GetCanScrollVerticallyHandler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.HomeFragment+LinearLayoutManagerMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", HomeFragment_LinearLayoutManagerMethod.class, __md_methods);
	}


	public HomeFragment_LinearLayoutManagerMethod (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == HomeFragment_LinearLayoutManagerMethod.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.HomeFragment+LinearLayoutManagerMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public HomeFragment_LinearLayoutManagerMethod (android.content.Context p0, int p1, boolean p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == HomeFragment_LinearLayoutManagerMethod.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.HomeFragment+LinearLayoutManagerMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Boolean, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public HomeFragment_LinearLayoutManagerMethod (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == HomeFragment_LinearLayoutManagerMethod.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.HomeFragment+LinearLayoutManagerMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public boolean canScrollVertically ()
	{
		return n_canScrollVertically ();
	}

	private native boolean n_canScrollVertically ();

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
