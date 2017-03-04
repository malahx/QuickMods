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

using UnityEngine;

namespace QuickMute.QUtils {
    static class QTexture {
        
        internal static readonly string BLIZZY_PATH_SOUND = QVars.relativePath + "/Textures/BlizzyToolBar_sound";
        internal static readonly string BLIZZY_PATH_MUTE = QVars.relativePath + "/Textures/BlizzyToolBar_mute";
        internal static readonly string BLIZZY_PATH_CONF = QVars.relativePath + "/Textures/BlizzyConf";
        internal static readonly string STOCK_PATH_SOUND = QVars.relativePath + "/Textures/StockToolBar_sound";
        internal static readonly string STOCK_PATH_MUTE = QVars.relativePath + "/Textures/StockToolBar_mute";
        internal static readonly string ICON_PATH_SOUND = QVars.relativePath + "/Textures/Icon_sound";
        internal static readonly string ICON_PATH_MUTE = QVars.relativePath + "/Textures/Icon_mute";

        internal static string BlizzyTexturePath {
            get {
                return (QSettings.Instance.Muted ? BLIZZY_PATH_MUTE : BLIZZY_PATH_CONF);
            }
        }

        static Texture2D stockSound;
        static Texture2D stockMute;
        internal static Texture2D StockTexture {
            get {
                if (stockSound == null) {
                    stockSound = GameDatabase.Instance.GetTexture(STOCK_PATH_SOUND, false);
                }
                if (stockMute == null) {
                    stockMute = GameDatabase.Instance.GetTexture(STOCK_PATH_MUTE, false);
                }
                return (QSettings.Instance.Muted ? stockMute : stockSound);
            }
        }

        static Texture2D iconSound;
        static Texture2D iconMute;
        internal static Texture2D IconTexture {
            get {
                if (iconSound == null) {
                    iconSound = GameDatabase.Instance.GetTexture(ICON_PATH_SOUND, false);
                }
                if (iconMute == null) {
                    iconMute = GameDatabase.Instance.GetTexture(ICON_PATH_MUTE, false);
                }
                return (QSettings.Instance.Muted ? iconMute : iconSound);
            }
        }
    }
}