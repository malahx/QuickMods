/* 
QuickSearch
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

namespace QuickSearch.QUtils
{
    static class Texture
    {

        internal static string STOCKTOOLBAR_PATH { get { return QuickSearch.relativePath + "/Textures/StockToolBar"; } }
        internal static string BLIZZY_PATH { get { return QuickSearch.relativePath + "/Textures/BlizzyToolBar"; } }
        internal static string SEARCH_PATH { get { return QuickSearch.relativePath + "/Textures/search"; } }
        internal static string DELETE_PATH { get { return QuickSearch.relativePath + "/Textures/delete"; } }

        static Texture2D delete;
        internal static Texture2D Delete
        {
            get
            {
                if (delete == null)
                {
                    delete = GameDatabase.Instance.GetTexture(DELETE_PATH, false);
                }
                return delete;
            }
        }

        static Texture2D search;
        internal static Texture2D Search
        {
            get
            {
                if (search == null)
                {
                    search = GameDatabase.Instance.GetTexture(SEARCH_PATH, false);
                }
                return search;
            }
        }
    }
}