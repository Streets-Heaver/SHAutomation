using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Caching;
using SHAutomation.Core.StaticClasses;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SHAutomation.Tests
{
    [TestClass]
    public class XPathTests
    {
        [TestMethod]
        public void XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));


            IDatabase db = Redis.Connection.GetDatabase();
            db.KeyDelete("XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue");

            db.KeyExists("XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue").Should().BeTrue();
            db.StringGet("XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue").Should().Be("[{\"Item1\":\"Test1\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");

        }

        [TestMethod]
        public void XPathIsInsertedIntoAppData_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("XPathIsInsertedIntoAppData_SaveXPathCache_BeTrue");
            window.GetXPathCache("XPathIsInsertedIntoAppData_SaveXPathCache_BeTrue");

            window.XPathList.Should().Contain((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
        }

        [TestMethod]
        public void XPathIsRetrievedFromAppData_GetXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object);

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("XPathIsRetrievedFromAppData_GetXPathCache_BeTrue");
            window.GetXPathCache("XPathIsRetrievedFromAppData_GetXPathCache_BeTrue");

            window.XPathList.Should().Contain((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
        }

        [TestMethod]
        public void XPathIsNotCachedWhenMatchesOriginalValue_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock.Setup(x => x.GetCacheValue(It.IsAny<string>(), It.IsAny<string>()));
            cacheServiceMock.Setup(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()));

            window.CacheService = cacheServiceMock.Object;

            window.GetXPathCache("XPathIsOnlySavedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue");

            window.SaveXPathCache("XPathIsOnlySavedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue");

            cacheServiceMock.Verify(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [TestMethod]
        public void XPathIsCachedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock.Setup(x => x.GetCacheValue(It.IsAny<string>(), It.IsAny<string>()));
            cacheServiceMock.Setup(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()));

            window.CacheService = cacheServiceMock.Object;

            IDatabase db = Redis.Connection.GetDatabase();

            db.KeyDelete("XPathIsOnlySavedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue");

            window.GetXPathCache("XPathIsOnlySavedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));

            window.SaveXPathCache("XPathIsOnlySavedWhenDifferentFromOriginalValue_SaveXPathCache_BeTrue");

            cacheServiceMock.Verify(x => x.SetCacheValue(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        [TestMethod]
        public void XPathNotSavedToRedisWhenNoXpath_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();

            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = Redis.Connection.GetDatabase();
            db.KeyDelete("XPathNotSavedToRedisWhenNoXpath_SaveXPathCache_BeTrue");

            window.SaveXPathCache("XPathNotSavedToRedisWhenNoXpath_SaveXPathCache_BeTrue");

            db.KeyExists("XPathNotSavedToRedisWhenNoXpath_SaveXPathCache_BeTrue").Should().BeFalse();

        }

        [TestMethod]
        public void XPathRedisValueIsOverwrittenAndSavedCorrectly_SaveXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = Redis.Connection.GetDatabase();
            db.KeyDelete("XPathRedisValueIsOverwrittenAndSavedCorrectly_SaveXPathCache_BeTrue");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("XPathRedisValueIsOverwrittenAndSavedCorrectly_SaveXPathCache_BeTrue");

            db.StringGet("XPathIsInsertedIntoRedis_SaveXPathCache_BeTrue").Should().Be("[{\"Item1\":\"Test1\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");

            window.XPathList.Clear();
            window.XPathList.Add((identifier: "Test2", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("XPathRedisValueIsOverwrittenAndSavedCorrectly_SaveXPathCache_BeTrue");


            db.StringGet("XPathRedisValueIsOverwrittenAndSavedCorrectly_SaveXPathCache_BeTrue").Should().Be("[{\"Item1\":\"Test2\",\"Item2\":\"AutomationId\",\"Item3\":\"XPATH\"}]");


        }

        [TestMethod]
        public void XPathValueRetrievedFromCacheWhenExists_GetXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            IDatabase db = Redis.Connection.GetDatabase();
            db.KeyDelete("XPathValueRetrievedFromCacheWhenExists_GetXPathCache_BeTrue");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("XPathValueRetrievedFromCacheWhenExists_GetXPathCache_BeTrue");

            window.GetXPathCache("XPathValueRetrievedFromCacheWhenExists_GetXPathCache_BeTrue");

            window.XPathList.Should().BeEquivalentTo((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));

        }

        [TestMethod]
        public void XPathIsEmptyCollectionWhenNoCacheHit_GetXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            window.GetXPathCache("XPathIsEmptyCollectionWhenNoCacheHit_GetXPathCache_BeTrue");

            window.XPathList.Should().BeEmpty();

        }
        [TestMethod]
        public void XPathKeyReturnedSavesAsNormalTestNameWhenBranchNameNotNumericFormat_GenerateCacheKey_BeTrue()
        {

            string currentVariable = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var cache = new CacheService(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "master");
            string output = cache.GenerateCacheKey("XPathSavesAsNormalTestNameWhenBranchNameNotNumericFormat_GetXPathCache_BeTrue");
            output.Should().Be("XPathSavesAsNormalTestNameWhenBranchNameNotNumericFormat_GetXPathCache_BeTrue");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "Task124123");
            output = cache.GenerateCacheKey( "XPathSavesAsNormalTestNameWhenBranchNameNotNumericFormat_GetXPathCache_BeTrue");
            output.Should().Be("XPathSavesAsNormalTestNameWhenBranchNameNotNumericFormat_GetXPathCache_BeTrue");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", currentVariable);
        }
        [TestMethod]
        public void XPathKeyReturnedAsNormalTestWhenNoBranchVariable_GenerateCacheKey_BeTrue()
        {

            string currentVariable = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var cache = new CacheService(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));
            Environment.SetEnvironmentVariable("Build_SourceBranchName", string.Empty);
            string output = cache.GenerateCacheKey( "XPathKeyReturnedAsNormalTestWhenNoBranchVariable_GenerateCacheKey_BeTrue");
            output.Should().Be("XPathKeyReturnedAsNormalTestWhenNoBranchVariable_GenerateCacheKey_BeTrue");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", currentVariable);
        }
        [TestMethod]
        public void XPathKeyReturnedWithIterationWhenIterationBranchAvailable_GenerateCacheKey_BeTrue()
        {
            Environment.SetEnvironmentVariable("BranchMatchRegex", @"\d\.\d\d");
            string currentVariable = Environment.GetEnvironmentVariable("Build_SourceBranchName");
            var cache = new CacheService(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "8.24");
            string output = cache.GenerateCacheKey("XPathKeyReturnedWithIterationWhenIterationBranchAvailable_GenerateCacheKey_BeTrue");
            output.Should().Be("XPathKeyReturnedWithIterationWhenIterationBranchAvailable_GenerateCacheKey_BeTrue_8.24");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", "8.25");
            output = cache.GenerateCacheKey("XPathKeyReturnedWithIterationWhenIterationBranchAvailable_GenerateCacheKey_BeTrue");
            output.Should().Be("XPathKeyReturnedWithIterationWhenIterationBranchAvailable_GenerateCacheKey_BeTrue_8.25");
            Environment.SetEnvironmentVariable("Build_SourceBranchName", currentVariable);
        }

        [TestMethod]
        public void SavesXPathWithIterationName_SaveXPathCache_BeTrue()
        {

            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            Environment.SetEnvironmentVariable("Build_SourceBranchName", "8.25");

            IDatabase db = Redis.Connection.GetDatabase();
            db.KeyDelete("SavesXPathWithIterationName_SaveXPathCache_BeTrue");
            db.KeyDelete("SavesXPathWithIterationName_SaveXPathCache_BeTrue_8.25");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("SavesXPathWithIterationName_SaveXPathCache_BeTrue");
            db.KeyExists("SavesXPathWithIterationName_SaveXPathCache_BeTrue").Should().BeFalse();
            db.KeyExists("SavesXPathWithIterationName_SaveXPathCache_BeTrue_8.25").Should().BeTrue();

        }
        [TestMethod]
        public void SavesXPathWithoutIterationName_SaveXPathCache_BeTrue()
        {
            var frameworkAutomationElementMock = new Mock<FrameworkAutomationElementBase>();
            var window = new Window(frameworkAutomationElementMock.Object, Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "shautomation.json"));

            Environment.SetEnvironmentVariable("Build_SourceBranchName", "master");

            IDatabase db = Redis.Connection.GetDatabase();
            db.KeyDelete("SavesXPathWithoutIterationName_SaveXPathCache_BeTrue");
            db.KeyDelete("SavesXPathWithoutIterationName_SaveXPathCache_BeTrue_8.25");

            window.XPathList.Add((identifier: "Test1", property: "AutomationId", xpath: "XPATH"));
            window.SaveXPathCache("SavesXPathWithoutIterationName_SaveXPathCache_BeTrue");
            db.KeyExists("SavesXPathWithoutIterationName_SaveXPathCache_BeTrue").Should().BeTrue();
            db.KeyExists("SavesXPathWithoutIterationName_SaveXPathCache_BeTrue_8.25").Should().BeFalse();

        }
    }
}
