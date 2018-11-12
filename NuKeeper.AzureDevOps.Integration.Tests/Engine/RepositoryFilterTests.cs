using System;
using System.Threading.Tasks;
using NSubstitute;
using NuKeeper.Abstractions.Configuration;
using NuKeeper.Abstractions.Logging;
using NuKeeper.AzureDevOps;
using NuKeeper.Engine;
using NUnit.Framework;

namespace NuKeeper.Integration.Tests.Engine
{
    [TestFixture]
    public class RepositoryFilterTests
    {
        [Test]
        public async Task ShouldFilterOutNonDotnetRepository()
        {
            IRepositoryFilter subject = MakeRepositoryFilter();

            var result = await subject.ContainsDotNetProjects(new RepositorySettings
            {
                RepositoryName = "NonDotNet",
                RepositoryOwner = "nukeeper"
            });
            Assert.False(result);
        }

        [Test]
        public async Task ShouldNotFilterOutADotnetRepository()
        {
            IRepositoryFilter subject = MakeRepositoryFilter();

            var result = await subject.ContainsDotNetProjects(new RepositorySettings { RepositoryName = "AspNetCoreSample", RepositoryOwner = "nukeeper" });
            Assert.True(result);
        }

        private static RepositoryFilter MakeRepositoryFilter()
        {
            const string testKeyWithOnlyPublicAccess = "66zm7yzw2xk6ehbbh7y5n6vruazz6mwognbgfqcaqwgyyf6hoh3a";
            var logger = Substitute.For<INuKeeperLogger>();

            var client = new AzureDevOpsClient(logger);
            client.Initialise(new AuthSettings(new Uri("https://dev.azure.com/nukeeper"), testKeyWithOnlyPublicAccess));

            return new RepositoryFilter(client, logger);
        }
    }
}
