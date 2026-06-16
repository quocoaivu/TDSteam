using System;

namespace Services.PlatformSpecific
{
	internal enum SignInPhase
	{
		None,
		NewFacebookLogin,
		NewGoogleLogin,
		FacebookLinking,
		GoogleLinking,
		GoogleLogin,
		FacebookLogin
	}
}
