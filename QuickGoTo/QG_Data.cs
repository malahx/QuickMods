/* 
QuickGoTo
Copyright 2015 Malah

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

namespace QuickGoTo {
	public class QData {
		public QData (ProtoVessel pvessel) {
			protoVessel = pvessel;
			idx = HighLogic.CurrentGame.flightState.protoVessels.IndexOf (pvessel);
		}
		public int idx {
			get;
			private set;
		}
		public ProtoVessel protoVessel {
			get;
			private set;
		}
	}
}