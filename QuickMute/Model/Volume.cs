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

namespace QuickMute {
    public class Volume {
        float ambience, music, ship, ui, voice, level;
        bool mute;

        public Volume() {
            ambience = 1;
            music = 1;
            ship = 1;
            ui = 1;
            voice = 1;
            level = 1;
            mute = false;
        }

        public Volume(float ambience, float music, float ship, float ui, float voice) {
            this.ambience = ambience;
            this.music = music;
            this.ship = ship;
            this.ui = ui;
            this.voice = voice;
            level = 1;
            mute = false;
        }

        public Volume(float ambience, float music, float ship, float ui, float voice, float level) {
            this.ambience = ambience;
            this.music = music;
            this.ship = ship;
            this.ui = ui;
            this.voice = voice;
            this.level = level;
            mute = false;
        }

        public bool IsMuted() {
            return mute;
        }

        public float Level {
            get {
                return level;
            }
            set {
                if (System.Math.Abs(value) < float.Epsilon) {
                    mute = true;
                    return;
                }
                level = value;
            }
        }

        public void Apply() {
            if (mute) {
                GameSettings.AMBIENCE_VOLUME = 0;
                GameSettings.MUSIC_VOLUME = 0;
                GameSettings.SHIP_VOLUME = 0;
                GameSettings.UI_VOLUME = 0;
                GameSettings.VOICE_VOLUME = 0;
                MusicLogic.SetVolume(0);
                return;
            }
            GameSettings.AMBIENCE_VOLUME = ambience * level;
            GameSettings.MUSIC_VOLUME = music * level;
            GameSettings.SHIP_VOLUME = ship * level;
            GameSettings.UI_VOLUME = ui * level;
            GameSettings.VOICE_VOLUME = voice * level;
            MusicLogic.SetVolume(music * level);
        }

        public void Restore() {
            GameSettings.AMBIENCE_VOLUME = ambience;
            GameSettings.MUSIC_VOLUME = music;
            GameSettings.SHIP_VOLUME = ship;
            GameSettings.UI_VOLUME = ui;
            GameSettings.VOICE_VOLUME = voice ;
            MusicLogic.SetVolume(music);
        }
    }
}