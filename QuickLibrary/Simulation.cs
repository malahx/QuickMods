using UnityEngine;

namespace QuickLibrary
{
    public abstract class Simulation
    {
        public static Simulation Instance;

        public static EventVoid OnEnterSimulation = new EventVoid(nameof (OnEnterSimulation));
        public static EventVoid OnExitSimulation = new EventVoid(nameof (OnExitSimulation));
        
        public abstract bool IsInSimulation();
        public abstract void SetSimulation(bool simulation);
    }
}