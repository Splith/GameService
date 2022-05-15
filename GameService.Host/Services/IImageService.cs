using GameService.Host.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameService.Host.Services
{
    public interface IImageService
    {
        Task<ImageReservation> StartImageAsync(InstanceData instance);

        Task StopImageAsync(ImageReservation reservation);

        IEnumerable<ImageReservation> GetAllImages();
    }
}