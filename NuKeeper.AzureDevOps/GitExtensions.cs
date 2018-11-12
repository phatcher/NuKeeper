using System;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;

using NuKeeper.Abstractions.DTOs;

namespace NuKeeper.AzureDevOps
{
    public static class GitExtensions
    {
        public static string UserRepositoryName(this string repositoryName, string userName)
        {
            // TODO: Configure fork name pattern somewhere
            return string.IsNullOrEmpty(userName) ? repositoryName : $"{repositoryName}.{userName}";
        }

        public static Organization ToOrganization(this TeamProjectReference project)
        {
            return new Organization(project.Name, null);
        }

        public static Repository ToNuKeeperRepsitory(this GitRepository value)
        {
            // TODO: Provide User object
            // TODO: Extract/parse parent repository if it exists - do we need it?
            return new Repository(value.Name, false, new UserPermissions(false, true, true), new Uri(value.WebUrl), new Uri(value.RemoteUrl), null, value.IsFork, null);
        }
    }
}
