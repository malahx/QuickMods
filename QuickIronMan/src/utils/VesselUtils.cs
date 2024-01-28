using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using KSP.UI;
using KSP.UI.Screens;
using QuickIronMan.construction;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuickIronMan.utils
{
    public static class VesselUtils
    {
        public static string GenerateId(ShipTemplate shipTemplate)
        {
            return ToMd5($"{shipTemplate.filename}|{shipTemplate.shipName}|{shipTemplate.shipSize}|{shipTemplate.totalMass}|{shipTemplate.totalCost}");
        }

        private static string ToMd5(string data)
        {
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                foreach (var t in md5.ComputeHash(Encoding.ASCII.GetBytes(data)))
                    sb.Append(t.ToString("X2"));
                return sb.ToString();
            }
        }

        public static Dictionary<ShipTemplate, VesselListItem> InitializeVessels()
        {
            var vesselListItems = new Dictionary<ShipTemplate, VesselListItem>();

            var list = (IList)typeof(VesselSpawnDialog)
                .GetField("vesselDataItemList", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(VesselSpawnDialog.Instance);

            if (list == null || list.Count == 0)
            {
                Debug.Log("[QuickIronMan](VesselUtils): No Vessel data item list :'(");
                return null;   
            }

            foreach (var o in list)
            {
                var shipTemplate = RetrieveShipTemplate(o);
                var component = RetrieveVesselListItem(o);

                // Disable click on list if the vessel is not construct
                if (!ConstructionService.Instance.CanLaunch(shipTemplate))
                {
                    component.radioButton.onClick =
                        new UIRadioButton.ClickEvent<PointerEventData, UIRadioButton.State,
                            UIRadioButton.CallType>();
                }

                if (component.vesselWarnings.text.Equals(""))
                {
                    // Update message
                    component.vesselWarnings.text = MessageUtils.PrepareMessage(shipTemplate);
                }

                vesselListItems.Add(shipTemplate, component);
            }
            Debug.Log($"[QuickIronMan](VesselUtils): Vessel found: {vesselListItems.Count}");
            return vesselListItems;
        }

        public static ShipTemplate RetrieveShipTemplate(object o)
        {
            return (ShipTemplate)o.GetType()
                .GetProperty("template", BindingFlags.Instance | BindingFlags.Public)?.GetValue(o);
        }

        public static VesselListItem RetrieveVesselListItem(object o)
        {
            // Retrieve private listItem field
            var listItem = (UIListItem)o.GetType()
                .GetField("listItem", BindingFlags.Instance | BindingFlags.Public)?.GetValue(o);

            return listItem == null ? null : listItem.GetComponent<VesselListItem>();
        }
        
        public static double ConstructionTime(ShipTemplate shipTemplate)
        {
            // Minimal construction time 30 Kerbin days
            // Maximal construction time 1 Earth year
            return Math.Min(shipTemplate.partCount * shipTemplate.totalMass * 100 + 3600 * 6 * 30, 3600 * 6 * 30 * 365);
        }
    }
}