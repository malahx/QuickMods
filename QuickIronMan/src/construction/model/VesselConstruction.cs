namespace QuickIronMan.construction.model
{
    public class VesselConstruction
    {
        public string Name;
        public string Id;
        public string Path;
        public uint AlarmId;
        public double StartedAt;
        public double Time;
        public VesselStatus Status = VesselStatus.Construct;
    }
}