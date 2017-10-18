package md5214dfe80ae490f6bc9a74e75651ef416;


public class VideoPlayerActivity_RunnableMediaControllerHelper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Runnable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler:Java.Lang.IRunnableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.VideoPlayerActivity+RunnableMediaControllerHelper, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", VideoPlayerActivity_RunnableMediaControllerHelper.class, __md_methods);
	}


	public VideoPlayerActivity_RunnableMediaControllerHelper () throws java.lang.Throwable
	{
		super ();
		if (getClass () == VideoPlayerActivity_RunnableMediaControllerHelper.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.VideoPlayerActivity+RunnableMediaControllerHelper, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public VideoPlayerActivity_RunnableMediaControllerHelper (android.content.Context p0, android.widget.MediaController p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == VideoPlayerActivity_RunnableMediaControllerHelper.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.VideoPlayerActivity+RunnableMediaControllerHelper, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.MediaController, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
