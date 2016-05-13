
using UnityEngine;
namespace nFury.Externals.EnvironmentManager
{
	public class EnvironmentManagerComponent : MonoBehaviour
	{
		private const string MUSIC_INTERRUPTED = "interrupted";
		private const string MUSIC_PAUSED = "paused";
		private const string MUSIC_PLAYING = "playing";
		private const string MUSIC_STOPPED = "stopped";
		public void OnNativeAlertDismissed(string buttonName)
		{
			
		}
		public void PlaybackStateChanged(string state)
		{
			
		}
		public void OnReferralStoreDismissed(string message)
		{
			
		}
	}
}
