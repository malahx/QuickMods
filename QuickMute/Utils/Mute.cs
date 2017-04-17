/* 
QuickMute
Copyright 2017 Malah

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

namespace QuickMute.Object {
	static class QMute {

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
			QDebug.Log ("Verify", "QMute");
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
			QDebug.Log ("refreshAudioSource", "QMute");
		}
	}
}