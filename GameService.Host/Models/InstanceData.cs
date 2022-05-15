using System;

namespace GameService.Host.Models
{
    public class InstanceData
    {
        public Guid InstanceGuid { get; set; }

        public InstanceSizeEnum InstanceSize { get; set; }

        public Guid OwnerGuid { get; set; }

        public string Image { get; set;}
    }
}
