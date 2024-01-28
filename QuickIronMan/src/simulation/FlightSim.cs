using QuickIronMan.simulation.model;
using UnityEngine;

namespace QuickIronMan.simulation
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ToolbarSim : MonoBehaviour
    {

        private bool flightReady = false;

        private readonly Simulation sim = Simulation.INSTANCE;

        private void Awake() {

            GameEvents.onLaunch.Add(OnLaunch);
            GameEvents.onFlightReady.Add(OnFlightReady);

            Debug.Log($"[QuickIronMan]({name}) Awake");
        }

        private void Start() {

            sim.InitSimulation();

            Debug.Log($"[QuickIronMan]({name}) Start, simulation: {sim.IsInSimulation()}");
        }

        private void OnFlightReady() {

            flightReady = true;

            Debug.Log($"[QuickIronMan]({name}) Flight ready");
        }

        private void OnLaunch(EventReport data) {

            sim.VesselLaunched();

            if (!sim.IsInSimulation()) {
                sim.LostRevert();
            }

            Debug.Log($"[QuickIronMan]({name}) Launch, Simulation: {sim.IsInSimulation()}");
        }

        private void FixedUpdate() {

            if (sim.VesselStatus != VesselStatus.Waiting || !flightReady) return;

            PauseMenu.Display();
            sim.VesselLaunched();
        }

        private void OnDestroy() {

            GameEvents.onLaunch.Remove(OnLaunch);
            GameEvents.onFlightReady.Remove(OnFlightReady);
            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}
