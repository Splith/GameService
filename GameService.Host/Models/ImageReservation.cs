namespace GameService.Host.Models
{
    public class InstanceReservation
    {
        public InstanceReservation(PortData portData, InstanceData instanceData)
        {
            PortData = portData;
            InstanceData = instanceData;
        }

        public PortData PortData { get; init; }

        public InstanceData InstanceData { get; init; }
    }
}