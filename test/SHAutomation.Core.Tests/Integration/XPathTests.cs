using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Caching;
using SHAutomation.Core.Logging;
using SHAutomation.Core.StaticClasses;
using SHAutomation.Core.Tests.Common;
using StackExchange.Redis;
using System;
using System.IO;

namespace SHAutomation.Core.Tests.Integration
{
    [TestClass]
    public class XPathTests : TestBase
    {
        [TestMethod]
        public void XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));


            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);

            db.KeyExists(TestContext.TestName).Should().BeTrue();
            db.StringGet(TestContext.TestName).Should().Be("[{\"Item1\":\"Test1\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");

        }

        [TestMethod]
        public void DuplicatesRemovedBeforeSave_SaveXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);

            window.GetXPathCache(TestContext.TestName);

            window.XPathList.Count.Should().Be(1);

        }

        [TestMethod]
        public void UnusedXPathsAreDiscarded_SaveXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.XPathList.Add((identifier: "Test2", property: "AutomationId", xpath: "XPATH123"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);

            db.StringGet(TestContext.TestName).Should().Be("[{\"Item1\":\"Test1\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");

        }

        [TestMethod]
        public void XPathIsInsertedIntoAppData_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);
            window.GetXPathCache(TestContext.TestName);

            window.XPathList.Should().Contain((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
        }

        [TestMethod]
        public void XPathIsRetrievedFromAppData_GetXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);
            window.GetXPathCache(TestContext.TestName);

            window.XPathList.Should().Contain((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
        }

        [TestMethod]
        public void XPathIsNotCachedWhenMatchesOriginalValue_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock.Setup(x => x.GetCacheValue(It.IsAny<string>(), It.IsAny<string>()));
            cacheServiceMock.Setup(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()));

            window.CacheService = cacheServiceMock.Object;

            window.GetXPathCache(TestContext.TestName);

            window.SaveXPathCache(TestContext.TestName);

            cacheServiceMock.Verify(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [TestMethod]
        public void XPathIsCachedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock.Setup(x => x.GetCacheValue(It.IsAny<string>(), It.IsAny<string>()));
            cacheServiceMock.Setup(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()));

            window.CacheService = cacheServiceMock.Object;

            IDatabase db = RedisManager.Connection.GetDatabase();

            db.KeyDelete(TestContext.TestName);

            window.GetXPathCache(TestContext.TestName);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));

            window.SaveXPathCache(TestContext.TestName);

            cacheServiceMock.Verify(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        [TestMethod]
        public void XPathNotSavedToRedisWhenNoXpath_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);

            window.SaveXPathCache(TestContext.TestName);

            db.KeyExists(TestContext.TestName).Should().BeFalse();

        }

        [TestMethod]
        public void XPathRedisValueIsOverwrittenAndSavedCorrectly_SaveXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);

            db.StringGet(TestContext.TestName).Should().Be("[{\"Item1\":\"Test1\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");

            window.XPathList.Clear();
            window.XPathList.Add((identifier: "Test2", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);


            db.StringGet(TestContext.TestName).Should().Be("[{\"Item1\":\"Test2\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");


        }

        [TestMethod]
        public void XPathValueRetrievedFromCacheWhenExists_GetXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.UsedXPaths.Add("XPATH");

            window.SaveXPathCache(TestContext.TestName);

            window.GetXPathCache(TestContext.TestName);

            window.XPathList.Should().BeEquivalentTo((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));

        }


        [TestMethod]
        public void KeyExpiryIsSetOnRecordWhenSet_SetXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            RedisManager.KeyExpiry = TimeSpan.FromMinutes(5);

            IDatabase db = RedisManager.Connection.GetDatabase();

            window.XPathList.Add((identifier: "KeyExpTest", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache(TestContext.TestName);

            var record = db.StringGetWithExpiry(TestContext.TestName);
            record.Expiry.Should().NotBeNull();

        }

        [TestMethod]
        public void XPathIsEmptyCollectionWhenNoCacheHit_GetXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            window.GetXPathCache(TestContext.TestName);

            window.XPathList.Should().BeEmpty();

        }
        [TestMethod]
        public void XPathKeyReturnedSavesAsNormalTestNameWhenBranchNameNotNumericFormat_GenerateCacheKey_BeTrue()
        {

            var loggingServiceMock = new Mock<ILoggingService>();
            loggingServiceMock.Setup(x => x.Error(It.IsAny<Exception>()));
            loggingServiceMock.Setup(x => x.Error(It.IsAny<string>()));

            string currentVariable = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var cache = new CacheService(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"), loggingServiceMock.Object);
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "master");
            string output = cache.GenerateCacheKey(TestContext.TestName);
            output.Should().Be(TestContext.TestName);
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "Task124123");
            output = cache.GenerateCacheKey(TestContext.TestName);
            output.Should().Be(TestContext.TestName);
            Environment.SetEnvironmentVariable("Build_SourceBranchName", currentVariable);
        }
        [TestMethod]
        public void XPathKeyReturnedAsNormalTestWhenNoBranchVariable_GenerateCacheKey_BeTrue()
        {
            var loggingServiceMock = new Mock<ILoggingService>();
            loggingServiceMock.Setup(x => x.Error(It.IsAny<Exception>()));
            loggingServiceMock.Setup(x => x.Error(It.IsAny<string>()));

            string currentVariable = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var cache = new CacheService(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"), loggingServiceMock.Object);
            Environment.SetEnvironmentVariable("Build_SourceBranchName", string.Empty);
            string output = cache.GenerateCacheKey(TestContext.TestName);
            output.Should().Be(TestContext.TestName);
            Environment.SetEnvironmentVariable("Build_SourceBranchName", currentVariable);
        }
        [TestMethod]
        public void XPathKeyReturnedWithIterationWhenIterationBranchAvailable_GenerateCacheKey_BeTrue()
        {
            var loggingServiceMock = new Mock<ILoggingService>();
            loggingServiceMock.Setup(x => x.Error(It.IsAny<Exception>()));
            loggingServiceMock.Setup(x => x.Error(It.IsAny<string>()));

            Environment.SetEnvironmentVariable("BranchMatchRegex", @"\d\.\d\d");
            string currentVariable = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var cache = new CacheService(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"), loggingServiceMock.Object);
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "8.24");
            string output = cache.GenerateCacheKey(TestContext.TestName);
            output.Should().Be(TestContext.TestName + "_8.24");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "8.25");
            output = cache.GenerateCacheKey(TestContext.TestName);
            output.Should().Be(TestContext.TestName + "_8.25");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", currentVariable);
        }

        [TestMethod]
        public void SavesXPathWithIterationName_SaveXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            Environment.SetEnvironmentVariable("Build_SourceBranchName", "8.25");

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);
            db.KeyDelete(TestContext.TestName + "_8.25");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache(TestContext.TestName);
            db.KeyExists(TestContext.TestName).Should().BeFalse();
            db.KeyExists(TestContext.TestName + "_8.25").Should().BeTrue();

        }
        [TestMethod]
        public void SavesXPathWithoutIterationName_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            Environment.SetEnvironmentVariable("Build_SourceBranchName", "master");

            IDatabase db = RedisManager.Connection.GetDatabase();
            db.KeyDelete(TestContext.TestName);
            db.KeyDelete(TestContext.TestName + "_8.25");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("SavesXPathWithoutIterationName_SaveXPathCache_BeTrue");
            db.KeyExists(TestContext.TestName).Should().BeTrue();
            db.KeyExists(TestContext.TestName + "_8.25").Should().BeFalse();

        }
    }
}
