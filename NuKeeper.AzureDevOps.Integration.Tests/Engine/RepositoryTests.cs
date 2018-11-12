using System;
using System.Threading.Tasks;
using NSubstitute;
using NuKeeper.Abstractions.Configuration;
using NuKeeper.Abstractions.Logging;
using NUnit.Framework;

namespace NuKeeper.AzureDevOps.Integration.Tests.Engine
{
    [TestFixture]
    public class RepositoryTests 
    {
        [Test]
        public async Task GetOrganisation()
        {
            var client = CreateClient();

            var orgs = await client.GetOrganizations().ConfigureAwait(false);

            Assert.That(orgs.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task GetRepositories()
        {
            var client = CreateClient();

            var repos = await client.GetRepositoriesForOrganisation("Samples").ConfigureAwait(false);

            Assert.That(repos.Count, Is.Not.EqualTo(0));
        }

        private static AzureDevOpsClient CreateClient()
        {
            const string testKeyWithOnlyPublicAccess = "66zm7yzw2xk6ehbbh7y5n6vruazz6mwognbgfqcaqwgyyf6hoh3a";
            var logger = Substitute.For<INuKeeperLogger>();

            var client = new AzureDevOpsClient(logger);
            client.Initialise(new AuthSettings(new Uri("https://dev.azure.com/nukeeper"), testKeyWithOnlyPublicAccess));

            return client;
        }
    }
}
