using Docker.DotNet;
using Docker.DotNet.Models;
using GameService.Host.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameService.Host.Repos
{
    public class DockerImageRepo : GameService.Host.Services.IImageService
    {
        private Task setupTask;
        private IDictionary<Guid, ImageReservation> reservations;
        private DockerClient client;

        public DockerImageRepo()
        {
            client = new DockerClientConfiguration()
                .CreateClient();
            reservations = new SortedDictionary<Guid, ImageReservation>();
            setupTask = StartSetupTask();
        }

        private Task StartSetupTask()
        {
            return Task.Run(() => {
                var ftb = new ImagesCreateParameters
                {
                    FromImage = "mc/ftb",
                    Tag = "latest"
                };
                var vanilla = new ImagesCreateParameters
                {
                    FromImage = "mc/vanilla",
                    Tag = "latest"
                };
                var images = new ImagesCreateParameters[] { ftb, vanilla };

                foreach (var image in images)
                {
                    client.Images.CreateImageAsync
                    (
                        parameters: image, 
                        authConfig: null,
                        progress: new Progress<JSONMessage>() 
                    ).Wait();
                }
            });
        }

        public async Task<ImageReservation> StartImageAsync(InstanceData instanceData)
        {
            await client.Images.CreateImageAsync
            (
                parameters: MineCraftFTB(),
                authConfig: null,
                progress: new Progress<JSONMessage>()
            );

            var reservation = new ImageReservation(new PortData(), instanceData);
            reservations.Add(instanceData.InstanceGuid, reservation);

            return reservation;
        }

        public async Task StopImageAsync(ImageReservation imageReservation)
        {
            await client.Containers.StopContainerAsync
            (
                id: imageReservation.
            );
        }

        public IEnumerable<ImageReservation> GetAllImages()
        {
            throw new NotImplementedException();
        }

        #region Data

        private ImagesCreateParameters MineCraftFTB() => new ImagesCreateParameters
        {
            FromImage = "mc/ftb",
            Tag = "latest"
        };

        private ImagesCreateParameters MineCraftVanilla() => new ImagesCreateParameters
        {
            FromImage = "mc/vanilla",
            Tag = "latest"
        };

        #endregion
    }
}