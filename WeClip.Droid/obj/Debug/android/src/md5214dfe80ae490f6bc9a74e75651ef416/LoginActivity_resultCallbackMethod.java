package md5214dfe80ae490f6bc9a74e75651ef416;


public class LoginActivity_resultCallbackMethod
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.common.api.ResultCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onResult:(Lcom/google/android/gms/common/api/Result;)V:GetOnResult_Lcom_google_android_gms_common_api_Result_Handler:Android.Gms.Common.Apis.IResultCallbackInvoker, Xamarin.GooglePlayServices.Basement\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.LoginActivity+resultCallbackMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", LoginActivity_resultCallbackMethod.class, __md_methods);
	}


	public LoginActivity_resultCallbackMethod () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginActivity_resultCallbackMethod.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.LoginActivity+resultCallbackMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LoginActivity_resultCallbackMethod (com.google.android.gms.common.api.GoogleApiClient p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginActivity_resultCallbackMethod.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.LoginActivity+resultCallbackMethod, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Gms.Common.Apis.GoogleApiClient, Xamarin.GooglePlayServices.Basement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onResult (com.google.android.gms.common.api.Result p0)
	{
		n_onResult (p0);
	}

	private native void n_onResult (com.google.android.gms.common.api.Result p0);

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
