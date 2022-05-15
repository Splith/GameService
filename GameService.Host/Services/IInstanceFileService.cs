using GameService.Host.Models;
using System.Threading.Tasks;

namespace GameService.Host.Services
{
    public interface IInstanceFileService
    {
        Task LoadInstanceFilesAsync(InstanceData instanceData, PortData portData);

        Task BackupInstanceFilesAsync(InstanceData instanceData, PortData portData);

        Task ShutdownInstanceFilesAsync(InstanceData instanceData, PortData portData);
    }
}