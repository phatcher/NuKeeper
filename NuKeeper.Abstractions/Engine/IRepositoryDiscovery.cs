using System.Collections.Generic;
using System.Threading.Tasks;
using NuKeeper.Abstractions.CollaborationPlatform;
using NuKeeper.Abstractions.Configuration;

namespace NuKeeper.Abstractions.Engine
{
    public interface IRepositoryDiscovery
    {
        Task<IEnumerable<RepositorySettings>> GetRepositories(ICollaborationPlatform github, SourceControlServerSettings settings);
    }
}
