using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuickConstruct.utils
{
    public static class VesselUtils
    {
        public static string ShipName(ShipTemplate shipTemplate)
        {
            return $"{shipTemplate.shipName}|{shipTemplate.shipSize}|{shipTemplate.totalMass}|{shipTemplate.totalCost}";
        }

        public static Dictionary<ShipTemplate, VesselListItem> InitializeVessels()
        {
            var vesselListItems = new Dictionary<ShipTemplate, VesselListItem>();

            var list = (IList)typeof(VesselSpawnDialog)
                .GetField("vesselDataItemList", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(VesselSpawnDialog.Instance);

            if (list == null || list.Count == 0)
            {
                Debug.Log("[QuickConstruct](VesselUtils): No Vessel data item list :'(");
                return null;   
            }

            foreach (var o in list)
            {
                var shipTemplate = RetrieveShipTemplate(o);
                var component = RetrieveVesselListItem(o);

                // Disable click on list if the vessel is not construct
                if (!ConstructScenario.Instance.CanLaunch(shipTemplate))
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
            Debug.Log($"[QuickConstruct](VesselUtils): Vessel found: {vesselListItems.Count}");
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
    }
}