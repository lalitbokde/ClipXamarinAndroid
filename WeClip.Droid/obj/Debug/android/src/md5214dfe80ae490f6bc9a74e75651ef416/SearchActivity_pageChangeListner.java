package md5214dfe80ae490f6bc9a74e75651ef416;


public class SearchActivity_pageChangeListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.v4.view.ViewPager.OnPageChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageScrollStateChanged:(I)V:GetOnPageScrollStateChanged_IHandler:Android.Support.V4.View.ViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Support.v4\n" +
			"n_onPageScrolled:(IFI)V:GetOnPageScrolled_IFIHandler:Android.Support.V4.View.ViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Support.v4\n" +
			"n_onPageSelected:(I)V:GetOnPageSelected_IHandler:Android.Support.V4.View.ViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Support.v4\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.SearchActivity+pageChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", SearchActivity_pageChangeListner.class, __md_methods);
	}


	public SearchActivity_pageChangeListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SearchActivity_pageChangeListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.SearchActivity+pageChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public SearchActivity_pageChangeListner (md5214dfe80ae490f6bc9a74e75651ef416.SearchActivity p0, android.support.v4.view.ViewPager p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == SearchActivity_pageChangeListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.SearchActivity+pageChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "WeClip.Droid.SearchActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.View.ViewPager, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1 });
	}

	public SearchActivity_pageChangeListner (md5214dfe80ae490f6bc9a74e75651ef416.SearchActivity p0, android.support.v4.view.ViewPager p1, android.support.v4.app.FragmentPagerAdapter p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == SearchActivity_pageChangeListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.SearchActivity+pageChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "WeClip.Droid.SearchActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.View.ViewPager, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.App.FragmentPagerAdapter, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onPageScrollStateChanged (int p0)
	{
		n_onPageScrollStateChanged (p0);
	}

	private native void n_onPageScrollStateChanged (int p0);


	public void onPageScrolled (int p0, float p1, int p2)
	{
		n_onPageScrolled (p0, p1, p2);
	}

	private native void n_onPageScrolled (int p0, float p1, int p2);


	public void onPageSelected (int p0)
	{
		n_onPageSelected (p0);
	}

	private native void n_onPageSelected (int p0);

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
