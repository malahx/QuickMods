/* 
Quick0
Copyright 2016 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System.Collections;
using UnityEngine;


namespace QuickMute {
	public partial class QuickMute {
	
		string Icon_TexturePathSound = MOD + "/Textures/Icon_sound";
		string Icon_TexturePathMute = MOD + "/Textures/Icon_mute";

		Coroutine wait;

		//private Dictionary<string, float> audioVolume = new Dictionary<string, float>();

		protected Texture2D Icon_Texture {
			get {
				return GameDatabase.Instance.GetTexture((Muted ? Icon_TexturePathMute : Icon_TexturePathSound), false);
			}
		}

		protected bool Draw = false;

		protected bool Muted {
			get {
				return QSettings.Instance.Muted;
			}
			set {
				QSettings.Instance.Muted = value;
			}
		}

		protected void VerifyMute() {
			if (!Muted) {
				return;
			}
			audioSourceMute ();
			Log ("VerifyMute", "QuickMute");
		}

		void audioSourceMute() {
			AudioSource[] _audios = (AudioSource[])Resources.FindObjectsOfTypeAll (typeof(AudioSource));
			for (int _i = _audios.Length - 1; _i >= 0; --_i) {
				AudioSource _audio = _audios[_i];
				/*if (Muted) {
					audioVolume [_audio.name] = _audio.volume;
					_audio.volume = 0;
				} else {
					if (audioVolume.ContainsKey (_audio.name)) {
						_audio.volume = audioVolume [_audio.name];
					}
				}*/
				_audio.mute = Muted;
			}
			Log ("audioSourceMute", "QuickMute");
		}

		public void Mute() {
			Mute (!Muted);
			Draw = true;
			if (wait != null) {
				StopCoroutine (wait);
			}
			wait = StartCoroutine (Wait (5));
			QSettings.Instance.Save ();
			Log ("Mute()", "QuickMute");
		}

		IEnumerator Wait(int seconds) {
			yield return new WaitForSeconds (seconds);
			Draw = false;
			wait = null;
			Log ("Wait", "QuickMute");
		}

		public void Mute(bool mute) {
			Muted = mute;
			if (QuickMute.BlizzyToolbar != null) {
				QuickMute.BlizzyToolbar.Refresh ();
			}
			if (QStockToolbar.Instance != null) {
				QStockToolbar.Instance.Refresh ();
			}
			audioSourceMute ();
			if (Muted) {
				SaveSettingsVolume ();
				ResetSettingsVolume ();
				MusicLogic.SetVolume (0);
			} else {
				LoadSavedVolume ();
				ResetSavedVolume ();
				MusicLogic.SetVolume (GameSettings.MUSIC_VOLUME);
			}
			Log ((Muted ? "Mute" : "Unmute"));
		}

		bool VolumeSettingsIsZero {
			get {
				return GameSettings.AMBIENCE_VOLUME == 0 && GameSettings.MUSIC_VOLUME == 0 && GameSettings.SHIP_VOLUME == 0 && GameSettings.UI_VOLUME == 0 && GameSettings.VOICE_VOLUME == 0;
			}
		}
		bool VolumeSavedIsZero {
			get {
				return QSettings.Instance.AMBIENCE_VOLUME == 0 && QSettings.Instance.MUSIC_VOLUME == 0 && QSettings.Instance.SHIP_VOLUME == 0 && QSettings.Instance.UI_VOLUME == 0 && QSettings.Instance.VOICE_VOLUME == 0;
			}
		}
		void SaveSettingsVolume() {
			if (!VolumeSettingsIsZero) {
				QSettings.Instance.AMBIENCE_VOLUME = GameSettings.AMBIENCE_VOLUME;
				QSettings.Instance.MUSIC_VOLUME = GameSettings.MUSIC_VOLUME;
				QSettings.Instance.SHIP_VOLUME = GameSettings.SHIP_VOLUME;
				QSettings.Instance.UI_VOLUME = GameSettings.UI_VOLUME;
				QSettings.Instance.VOICE_VOLUME = GameSettings.VOICE_VOLUME;
			}
		}
		void LoadSavedVolume() {
			if (!VolumeSavedIsZero) {
				GameSettings.AMBIENCE_VOLUME = QSettings.Instance.AMBIENCE_VOLUME;
				GameSettings.MUSIC_VOLUME = QSettings.Instance.MUSIC_VOLUME;
				GameSettings.SHIP_VOLUME = QSettings.Instance.SHIP_VOLUME;
				GameSettings.UI_VOLUME = QSettings.Instance.UI_VOLUME;
				GameSettings.VOICE_VOLUME = QSettings.Instance.VOICE_VOLUME;
			}
		}
		void ResetSavedVolume() {
			if (!VolumeSettingsIsZero) {
				QSettings.Instance.AMBIENCE_VOLUME = 0;
				QSettings.Instance.MUSIC_VOLUME = 0;
				QSettings.Instance.SHIP_VOLUME = 0;
				QSettings.Instance.UI_VOLUME = 0;
				QSettings.Instance.VOICE_VOLUME = 0;
			}
		}
		void ResetSettingsVolume() {
			if (!VolumeSavedIsZero) {
				GameSettings.AMBIENCE_VOLUME = 0;
				GameSettings.MUSIC_VOLUME = 0;
				GameSettings.SHIP_VOLUME = 0;
				GameSettings.UI_VOLUME = 0;
				GameSettings.VOICE_VOLUME = 0;
			}
		}
	}
}