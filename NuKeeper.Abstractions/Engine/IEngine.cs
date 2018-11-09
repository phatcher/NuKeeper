using System.Threading.Tasks;
using NuKeeper.Abstractions.Configuration;

namespace NuKeeper.Abstractions.Engine
{
    public interface IEngine
    {
        Task<int> Run(SettingsContainer settings);
    }
}
