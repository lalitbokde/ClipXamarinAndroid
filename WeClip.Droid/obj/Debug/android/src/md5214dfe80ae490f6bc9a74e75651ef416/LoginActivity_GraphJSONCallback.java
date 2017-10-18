package md5214dfe80ae490f6bc9a74e75651ef416;


public class LoginActivity_GraphJSONCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.facebook.GraphRequest.GraphJSONObjectCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCompleted:(Lorg/json/JSONObject;Lcom/facebook/GraphResponse;)V:GetOnCompleted_Lorg_json_JSONObject_Lcom_facebook_GraphResponse_Handler:Xamarin.Facebook.GraphRequest/IGraphJSONObjectCallbackInvoker, Xamarin.Facebook\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.LoginActivity+GraphJSONCallback, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", LoginActivity_GraphJSONCallback.class, __md_methods);
	}


	public LoginActivity_GraphJSONCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginActivity_GraphJSONCallback.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.LoginActivity+GraphJSONCallback, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LoginActivity_GraphJSONCallback (com.facebook.login.LoginResult p0, android.app.Activity p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginActivity_GraphJSONCallback.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.LoginActivity+GraphJSONCallback, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Xamarin.Facebook.Login.LoginResult, Xamarin.Facebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onCompleted (org.json.JSONObject p0, com.facebook.GraphResponse p1)
	{
		n_onCompleted (p0, p1);
	}

	private native void n_onCompleted (org.json.JSONObject p0, com.facebook.GraphResponse p1);

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
