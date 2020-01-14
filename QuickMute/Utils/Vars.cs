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
using System.IO;
using System.Reflection;

namespace QuickMute.Object {
    static class QVars {
        public readonly static string VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
        public readonly static string MOD = Assembly.GetExecutingAssembly().GetName().Name;
        public readonly static string relativePath =  MOD;
        public static string PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../";
    }
}