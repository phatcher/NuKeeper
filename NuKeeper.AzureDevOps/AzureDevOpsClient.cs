using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using NuGet.Protocol;
using NuKeeper.Abstractions;
using NuKeeper.Abstractions.CollaborationPlatform;
using NuKeeper.Abstractions.Configuration;
using NuKeeper.Abstractions.DTOs;
using NuKeeper.Abstractions.Logging;

namespace NuKeeper.AzureDevOps
{
    public sealed class AzureDevOpsClient : ICollaborationPlatform, IDisposable
    {
        private readonly INuKeeperLogger _logger;
        private bool _initialised = false;

        private VssConnection connection;
        private Uri _apiBase;

        public AzureDevOpsClient(INuKeeperLogger logger)
        {
            _logger = logger;
        }

        public void Initialise(AuthSettings settings)
        {
            _apiBase = settings.ApiBase;

            connection = new VssConnection(_apiBase, new VssBasicCredential(string.Empty, settings.Token));

            _initialised = true;
        }

        public Task<User> GetCurrentUser()
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public Task OpenPullRequest(ForkData target, PullRequestRequest request, IEnumerable<string> labels)
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Organization>> GetOrganizations()
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Repository>> GetRepositoriesForOrganisation(string organisationName)
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public Task<Repository> GetUserRepository(string userName, string repositoryName)
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public Task<Repository> MakeUserFork(string owner, string repositoryName)
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public Task<bool> RepositoryBranchExists(string userName, string repositoryName, string branchName)
        {
            CheckInitialised();

            throw new NotImplementedException();
        }

        public async Task<SearchCodeResult> Search(SearchCodeRequest search)
        {
            CheckInitialised();

            var client = await connection.GetClientAsync<GitHttpClient>().ConfigureAwait(false);

            //var repos = new RepositoryCollection();
            foreach (var r in search.Repos)
            {
                var repo = await client.GetRepositoryAsync(r.owner, r.name).ConfigureAwait(false);

                repo.
            }

            //var result = await _client.Search.SearchCode(
            //    new Octokit.SearchCodeRequest(search.Term)
            //    {
            //        Repos = repos,
            //        In = new[] { CodeInQualifier.Path },
            //        PerPage = search.PerPage
            //    });
            return new SearchCodeResult(0);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }

        private void CheckInitialised()
        {
            if (!_initialised)
            {
                throw new NuKeeperException("AzureDevOps has not been initialised");
            }
        }
    }
}
