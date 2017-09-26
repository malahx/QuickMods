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

namespace QuickScience.Utils {
    static class QTexture {

        internal static readonly string BLIZZY_PATH_HIGH = QVars.relativePath + "/Textures/BlizzyToolBar_high";
        internal static readonly string BLIZZY_PATH_MEDIUM = QVars.relativePath + "/Textures/BlizzyToolBar_medium";
        internal static readonly string BLIZZY_PATH_LOW = QVars.relativePath + "/Textures/BlizzyToolBar_low";
        internal static readonly string BLIZZY_PATH_ZERO = QVars.relativePath + "/Textures/BlizzyToolBar_zero";
        internal static readonly string BLIZZY_PATH_MUTE = QVars.relativePath + "/Textures/BlizzyToolBar_mute";
        internal static readonly string BLIZZY_PATH_CONF = QVars.relativePath + "/Textures/BlizzyConf";
        internal static readonly string BLIZZY_PATH_VOL = QVars.relativePath + "/Textures/BlizzyVol";
        internal static readonly string STOCK_PATH_HIGH = QVars.relativePath + "/Textures/StockToolBar_high";
        internal static readonly string STOCK_PATH_MEDIUM = QVars.relativePath + "/Textures/StockToolBar_medium";
        internal static readonly string STOCK_PATH_LOW = QVars.relativePath + "/Textures/StockToolBar_low";
        internal static readonly string STOCK_PATH_ZERO = QVars.relativePath + "/Textures/StockToolBar_zero";
        internal static readonly string STOCK_PATH_MUTE = QVars.relativePath + "/Textures/StockToolBar_mute";
        internal static readonly string ICON_PATH_HIGH = QVars.relativePath + "/Textures/Icon_high";
        internal static readonly string ICON_PATH_MEDIUM = QVars.relativePath + "/Textures/Icon_medium";
        internal static readonly string ICON_PATH_LOW = QVars.relativePath + "/Textures/Icon_low";
        internal static readonly string ICON_PATH_ZERO = QVars.relativePath + "/Textures/Icon_zero";
        internal static readonly string ICON_PATH_MUTE = QVars.relativePath + "/Textures/Icon_mute";

        internal static string BlizzyTexturePath {
            get {
                return QSettings.Instance.Muted ? BLIZZY_PATH_MUTE :
                        QuickMute.Instance.volume.Master > 0.75 ? BLIZZY_PATH_HIGH :
                        QuickMute.Instance.volume.Master > 0.25 ? BLIZZY_PATH_MEDIUM :
                        QuickMute.Instance.volume.Master > 0.01 ? BLIZZY_PATH_LOW :
                        BLIZZY_PATH_ZERO;
            }
        }

        static Texture2D stockHigh;
        static Texture2D stockMedium;
        static Texture2D stockLow;
        static Texture2D stockZero;
        static Texture2D stockMute;
        internal static Texture2D StockTexture {
            get {
                if (stockHigh == null) {
                    stockHigh = GameDatabase.Instance.GetTexture(STOCK_PATH_HIGH, false);
                }
                if (stockMedium == null) {
                    stockMedium = GameDatabase.Instance.GetTexture(STOCK_PATH_MEDIUM, false);
                }
                if (stockLow == null) {
                    stockLow = GameDatabase.Instance.GetTexture(STOCK_PATH_LOW, false);
                }
                if (stockZero == null) {
                    stockZero = GameDatabase.Instance.GetTexture(STOCK_PATH_ZERO, false);
                }
                if (stockMute == null) {
                    stockMute = GameDatabase.Instance.GetTexture(STOCK_PATH_MUTE, false);
                }
                return QSettings.Instance.Muted ? stockMute :
                        QuickMute.Instance.volume.Master > 0.75 ? stockHigh :
                        QuickMute.Instance.volume.Master > 0.25 ? stockMedium :
                        QuickMute.Instance.volume.Master > 0.01 ? stockLow :
                        stockZero;
            }
        }

        static Texture2D iconHigh;
        static Texture2D iconMedium;
        static Texture2D iconLow;
        static Texture2D iconZero;
        static Texture2D iconMute;
        internal static Texture2D IconTexture {
            get {
                if (iconHigh == null) {
                    iconHigh = GameDatabase.Instance.GetTexture(ICON_PATH_HIGH, false);
                }
                if (iconMedium == null) {
                    iconMedium = GameDatabase.Instance.GetTexture(ICON_PATH_MEDIUM, false);
                }
                if (iconLow == null) {
                    iconLow = GameDatabase.Instance.GetTexture(ICON_PATH_LOW, false);
                }
                if (iconZero == null) {
                    iconZero = GameDatabase.Instance.GetTexture(ICON_PATH_ZERO, false);
                }
                if (iconMute == null) {
                    iconMute = GameDatabase.Instance.GetTexture(ICON_PATH_MUTE, false);
                }
                return QSettings.Instance.Muted ? iconMute :
                        QuickMute.Instance.volume.Master > 0.75 ? iconHigh :
                        QuickMute.Instance.volume.Master > 0.25 ? iconMedium :
                        QuickMute.Instance.volume.Master > 0.01 ? iconLow :
                        iconZero;
            }
        }
    }
}