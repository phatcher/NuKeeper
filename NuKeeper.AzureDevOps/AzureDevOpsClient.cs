using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

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
        private ProjectHttpClient projectClient;
        private GitHttpClient client;
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

        public async Task<User> GetCurrentUser()
        {
            await CheckInitialised().ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task OpenPullRequest(ForkData target, PullRequestRequest request, IEnumerable<string> labels)
        {
            await CheckInitialised().ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Organization>> GetOrganizations()
        {
            await CheckInitialised().ConfigureAwait(false);

            // Use projects as proxy for organisations
            var projects = await projectClient.GetProjects().ConfigureAwait(false);

            var orgs = projects.Select(x => x.ToOrganization()).ToList();
            return orgs;
        }

        public async Task<IReadOnlyList<Repository>> GetRepositoriesForOrganisation(string organisationName)
        {
            await CheckInitialised().ConfigureAwait(false);

            // Organisation is project as the PAT token we authenticated with already restricts us to DevOps Org
            var project = await projectClient.GetProject(organisationName).ConfigureAwait(false);

            var devopRepos = await client.GetRepositoriesAsync(project.Id).ConfigureAwait(false);

            var repos = devopRepos.Select(x => x.ToNuKeeperRepsitory()).ToList();
            return repos;
        }

        public async Task<Repository> GetUserRepository(string userName, string repositoryName)
        {
            await CheckInitialised().ConfigureAwait(false);

            _logger.Detailed($"Looking for user fork for {userName}/{repositoryName}");

            // TODO: How do we know which project we are in here?
            var result = await client.GetRepositoryAsync(userName, repositoryName.UserRepositoryName(userName)).ConfigureAwait(false);
            if (result == null)
            {
                _logger.Detailed("User fork not found");
                return null;
            }
            _logger.Normal($"User fork found at {result.Url}");
            return result.ToNuKeeperRepsitory();
        }

        public async Task<Repository> MakeUserFork(string owner, string repositoryName)
        {
            await CheckInitialised().ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> RepositoryBranchExists(string userName, string repositoryName, string branchName)
        {
            await CheckInitialised().ConfigureAwait(false);

            var repo = await client.GetRepositoryAsync(repositoryName.UserRepositoryName(userName)).ConfigureAwait(false);
            if (repo == null)
            {
                _logger.Detailed($"Repository not found for {userName} / {repositoryName} / {branchName}");
                return false;
            }

            var branches = await client.GetRefsAsync(repo.Id, filter: "heads/" + branchName).ConfigureAwait(false);
            if (branches.Count < 1)
            {
                _logger.Detailed($"No branch found for {userName} / {repositoryName} / {branchName}");
                return false;
            }

            _logger.Detailed($"Branch found for {userName} / {repositoryName} / {branchName}");
            return true;
        }

        public async Task<SearchCodeResult> Search(SearchCodeRequest search)
        {
            await CheckInitialised().ConfigureAwait(false);

            // TODO: How do we do a search on the repository.
            //var client = await connection.GetClientAsync<GitHttpClient>().ConfigureAwait(false);

            //var repos = new RepositoryCollection();
            //var result = await _client.Search.SearchCode(
            //    new Octokit.SearchCodeRequest(search.Term)
            //    {
            //        Repos = repos,
            //        In = new[] { CodeInQualifier.Path },
            //        PerPage = search.PerPage
            //    });

            // HACK: Force result to true for now
            return new SearchCodeResult(1);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }

        private async Task CheckInitialised()
        {
            if (!_initialised)
            {
                throw new NuKeeperException("AzureDevOps has not been initialised");
            }

            if (projectClient == null)
            {
                projectClient = await connection.GetClientAsync<ProjectHttpClient>().ConfigureAwait(false);
                client = await connection.GetClientAsync<GitHttpClient>().ConfigureAwait(false);
            }
        }
    }
}
