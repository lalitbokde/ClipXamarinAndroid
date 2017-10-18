package md5acc54a6ea3c5f2d688696540e0f6a967;


public class StartPlayer_OnCompletionListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.MediaPlayer.OnCompletionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCompletion:(Landroid/media/MediaPlayer;)V:GetOnCompletion_Landroid_media_MediaPlayer_Handler:Android.Media.MediaPlayer/IOnCompletionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.StartPlayer+OnCompletionListener, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", StartPlayer_OnCompletionListener.class, __md_methods);
	}


	public StartPlayer_OnCompletionListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == StartPlayer_OnCompletionListener.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.StartPlayer+OnCompletionListener, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public StartPlayer_OnCompletionListener (android.app.Activity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == StartPlayer_OnCompletionListener.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.StartPlayer+OnCompletionListener, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onCompletion (android.media.MediaPlayer p0)
	{
		n_onCompletion (p0);
	}

	private native void n_onCompletion (android.media.MediaPlayer p0);

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
