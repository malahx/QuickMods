/* 
QuickContracts
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

using KSP.UI.Screens;
using Contracts;
using System;
using System.Collections.Generic;

namespace QuickContracts {

	public partial class QGUI {

		float declineCost = 0;
		float declineContracts = 0;

		void OnDeclined(Contracts.Contract contract) {
			if (MissionControl.Instance == null) {
				return;
			}
			declineCost += HighLogic.CurrentGame.Parameters.Career.RepLossDeclined;
			declineContracts++;
			Log ("A contract has been declined!", "QGUI");
		}

		void OnGUIMissionControlDespawn() {
			QSettings.Instance.Save ();
			if (declineCost > 0 && declineContracts > 0 & MessageSystem.Ready) {
				if (QSettings.Instance.EnableMessage) {
					string _string = string.Format ("You have declined <b><color=#FF0000>{0}</color></b> contract(s).\nIt has cost you <color=#E0D503>¡<b>{1}</b></color>", declineContracts, declineCost);
					MessageSystem.Instance.AddMessage (new MessageSystem.Message (MOD, _string, MessageSystemButton.MessageButtonColor.ORANGE, MessageSystemButton.ButtonIcons.ALERT));
					Log ("Message send.", "QGUI");
				}
				declineContracts = 0;
				declineCost = 0;
			}
		}

		void Accept() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts", "QGUI");
				return;
			}
			if (MissionControl.Instance.selectedMission == null) {
				Log ("There's no selected contract", "QGUI");
				return;
			}
			if (!MissionControl.Instance.btnAccept.IsInteractable()) {
				Log ("Can't accept this contract", "QGUI");
				return;
			}
			int _active = ContractSystem.Instance.GetActiveContractCount ();
			int _accept = GameVariables.Instance.GetActiveContractsLimit (ScenarioUpgradeableFacilities.GetFacilityLevel (SpaceCenterFacility.MissionControl));
			if (_active >= _accept) {
				Log ("You can't accept a new contract, you have " + _active + " active contracts and you can accept " + _accept + " contracts.", "QGUI");
				return;
			}
			MissionControl.Instance.btnAccept.onClick.Invoke ();
			Log ("Accepted a contract", "QGUI");
		}

		void Decline() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts", "QGUI");
				return;
			}
			if (MissionControl.Instance.selectedMission == null) {
				Log ("There's no selected contract", "QGUI");
				return;
			}
			if (!MissionControl.Instance.btnDecline.IsInteractable()) {
				Log ("Can't decline this contract", "QGUI");
				return;
			}
			MissionControl.Instance.btnDecline.onClick.Invoke ();
			Log ("Declined a contract", "QGUI");
		}

		void DeclineAll(Type ContractType) {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts", "QGUI");
				return;
			}
			if (ContractSystem.Instance == null) {
				return;
			}
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			for (int i = 0; i < _contracts.Count; i++) {
				Contract _contract = _contracts [i];
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined () && _contract.GetType() == ContractType) {
					_contract.Decline ();
				}
			}
			MissionControl.Instance.RebuildContractList ();
			Log ("Decline all: " + ContractType.Name, "QGUI");
		}

		void DeclineAll() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts", "QGUI");
				return;
			}
			if (ContractSystem.Instance == null) {
				return;
			}
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			for (int i = 0; i < _contracts.Count; i++) {
				Contract _contract = _contracts [i];
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined ()) {
					_contract.Decline ();
				}
			}
			MissionControl.Instance.RebuildContractList ();
			Log ("Decline all contracts", "QGUI");
		}
	}
}