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

using System.Collections.Generic;
using UnityEngine;


namespace QuickMute {
	public class QMute : QuickMute {

		static Dictionary<string, float> audioVolume = new Dictionary<string, float> ();
		static AudioSource[] audioSources;
		static AudioSource[] AudioSources {
			get {
				if (audioSources == null) {
					audioSources = (AudioSource[])Resources.FindObjectsOfTypeAll (typeof (AudioSource));
				}
				return audioSources;
			}
		}

		internal static void Verify() {
			if (!QSettings.Instance.Muted) {
				return;
			}
			refreshAudioSource ();
			Log ("Verify", "QMute");
		}

		internal static void refreshAudioSource() {
			AudioSource[] _audios = AudioSources;
			for (int _i = _audios.Length - 1; _i >= 0; --_i) {
				AudioSource _audio = _audios[_i];
				if (QSettings.Instance.Muted) {
					audioVolume [_audio.name] = _audio.volume;
					_audio.volume = 0;
				} else {
					if (audioVolume.ContainsKey (_audio.name)) {
						_audio.volume = audioVolume [_audio.name];
					}
				}
			}
			Log ("refreshAudioSource", "QMute");
		}

		internal static void refresh(bool mute) {
			refreshAudioSource ();
			if (mute) {
				SaveSettingsVolume ();
				ResetSettingsVolume ();
				MusicLogic.SetVolume (0);
			} else {
				LoadSavedVolume ();
				ResetSavedVolume ();
				MusicLogic.SetVolume (GameSettings.MUSIC_VOLUME);
			}
			Log ("refresh: " + (QSettings.Instance.Muted ? "Mute" : "Unmute"), "QMute");
		}

		static bool VolumeSettingsIsZero {
			get {
				return GameSettings.AMBIENCE_VOLUME == 0 && GameSettings.MUSIC_VOLUME == 0 && GameSettings.SHIP_VOLUME == 0 && GameSettings.UI_VOLUME == 0 && GameSettings.VOICE_VOLUME == 0;
			}
		}
		static bool VolumeSavedIsZero {
			get {
				return QSettings.Instance.AMBIENCE_VOLUME == 0 && QSettings.Instance.MUSIC_VOLUME == 0 && QSettings.Instance.SHIP_VOLUME == 0 && QSettings.Instance.UI_VOLUME == 0 && QSettings.Instance.VOICE_VOLUME == 0;
			}
		}
		static void SaveSettingsVolume() {
			if (!VolumeSettingsIsZero) {
				QSettings.Instance.AMBIENCE_VOLUME = GameSettings.AMBIENCE_VOLUME;
				QSettings.Instance.MUSIC_VOLUME = GameSettings.MUSIC_VOLUME;
				QSettings.Instance.SHIP_VOLUME = GameSettings.SHIP_VOLUME;
				QSettings.Instance.UI_VOLUME = GameSettings.UI_VOLUME;
				QSettings.Instance.VOICE_VOLUME = GameSettings.VOICE_VOLUME;
			}
		}
		static void LoadSavedVolume() {
			if (!VolumeSavedIsZero) {
				GameSettings.AMBIENCE_VOLUME = QSettings.Instance.AMBIENCE_VOLUME;
				GameSettings.MUSIC_VOLUME = QSettings.Instance.MUSIC_VOLUME;
				GameSettings.SHIP_VOLUME = QSettings.Instance.SHIP_VOLUME;
				GameSettings.UI_VOLUME = QSettings.Instance.UI_VOLUME;
				GameSettings.VOICE_VOLUME = QSettings.Instance.VOICE_VOLUME;
			}
		}
		static void ResetSavedVolume() {
			if (!VolumeSettingsIsZero) {
				QSettings.Instance.AMBIENCE_VOLUME = 0;
				QSettings.Instance.MUSIC_VOLUME = 0;
				QSettings.Instance.SHIP_VOLUME = 0;
				QSettings.Instance.UI_VOLUME = 0;
				QSettings.Instance.VOICE_VOLUME = 0;
			}
		}
		static void ResetSettingsVolume() {
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