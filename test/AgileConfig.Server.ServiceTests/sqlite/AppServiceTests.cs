﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AgileConfig.Server.Data.Entity;
using System.Threading.Tasks;
using AgileConfig.Server.IService;
using AgileConfig.Server.Service;
using Microsoft.Extensions.DependencyInjection;
using AgileConfig.Server.Data.Abstraction;

namespace AgileConfig.Server.ServiceTests.sqlite
{
    [TestClass()]
    public class AppServiceTests: BasicTestService
    {
        IAppService _appservice = null;
        public override Dictionary<string, string> GetConfigurationData()
        {
            return
                new Dictionary<string, string>
                {
                {"db:provider","sqlite" },
                {"db:conn","Data Source=agile_config.db" }
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _appservice = _serviceProvider.GetService<IAppService>();
            ClearData();
        }


        [TestMethod()]
        public async Task AddAsyncTest()
        {
            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            var app = await _appservice.GetAsync(source.Id);

            Assert.IsTrue(result);
            Assert.IsNotNull(app);

            Assert.AreEqual(source.Id, app.Id);
            Assert.AreEqual(source.Name, app.Name);
            Assert.AreEqual(source.Secret, app.Secret);
            Assert.AreEqual(source.CreateTime.ToString("yyyyMMddhhmmss"), app.CreateTime.ToString("yyyyMMddhhmmss"));
            Assert.AreEqual(source.UpdateTime.Value.ToString("yyyyMMddhhmmss"), app.UpdateTime.Value.ToString("yyyyMMddhhmmss"));
            Assert.AreEqual(source.Enabled, app.Enabled);
        }

        [TestMethod()]
        public async Task DeleteAsyncTest()
        {
            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            Assert.IsTrue(result);

            var delResult = await _appservice.DeleteAsync(source);
            Assert.IsTrue(delResult);
        }

        [TestMethod()]
        public async Task DeleteAsyncTest1()
        {
            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            Assert.IsTrue(result);

            var delResult = await _appservice.DeleteAsync(id);
            Assert.IsTrue(delResult);

            var app = await _appservice.GetAsync(source.Id);

            Assert.IsNull(app);

        }

        [TestMethod()]
        public async Task GetAsyncTest()
        {
            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            Assert.IsTrue(result);

            var app = await _appservice.GetAsync(id);
            Assert.IsNotNull(app);

            Assert.AreEqual(source.Id, app.Id);
            Assert.AreEqual(source.Name, app.Name);
            Assert.AreEqual(source.Secret, app.Secret);
            Assert.AreEqual(source.CreateTime.ToString("yyyyMMddhhmm"), app.CreateTime.ToString("yyyyMMddhhmm"));
            Assert.AreEqual(source.UpdateTime.Value.ToString("yyyyMMddhhmm"), app.UpdateTime.Value.ToString("yyyyMMddhhmm"));
            Assert.AreEqual(source.Enabled, app.Enabled);
        }

        [TestMethod()]
        public async Task GetAllAppsAsyncTest()
        {
            ClearData();

            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            Assert.IsTrue(result);
            var id1 = Guid.NewGuid().ToString();
            var source1 = new App
            {
                Id = id1,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result1 = await _appservice.AddAsync(source1);
            Assert.IsTrue(result1);

            var apps = await _appservice.GetAllAppsAsync();
            Assert.IsNotNull(apps);
            Assert.AreEqual(2, apps.Count);


        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            Assert.IsTrue(result);

            source.Name = "new name";
            source.Secret = "new sec";
            source.CreateTime = DateTime.Now.AddDays(1);
            source.UpdateTime = DateTime.Now.AddDays(1);
            source.Enabled = false;

            var result1 = await _appservice.UpdateAsync(source);
            Assert.IsTrue(result1);

            var app = await _appservice.GetAsync(source.Id);

            Assert.AreEqual(source.Id, app.Id);
            Assert.AreEqual(source.Name, app.Name);
            Assert.AreEqual(source.Secret, app.Secret);
            Assert.AreEqual(source.CreateTime.ToString("yyyyMMddhhmmss"), app.CreateTime.ToString("yyyyMMddhhmmss"));
            Assert.AreEqual(source.UpdateTime.Value.ToString("yyyyMMddhhmmss"), app.UpdateTime.Value.ToString("yyyyMMddhhmmss"));
            Assert.AreEqual(source.Enabled, app.Enabled);
        }

        [TestMethod()]
        public async Task CountEnabledAppsAsyncTest()
        {
            this.ClearData();

            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true
            };
            var result = await _appservice.AddAsync(source);
            Assert.IsTrue(result);
            var id1 = Guid.NewGuid().ToString();
            var source1 = new App
            {
                Id = id1,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = false
            };
            var result1 = await _appservice.AddAsync(source1);
            Assert.IsTrue(result1);

            var count = await _appservice.CountEnabledAppsAsync();
            Assert.AreEqual(1, count);
        }

        [TestMethod()]
        public async Task GetAllInheritancedAppsAsyncTest()
        {
            this.ClearData();

            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true,
                Type = AppType.PRIVATE
            };
            var source1 = new App
            {
                Id = Guid.NewGuid().ToString(),
                Name = "xxx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true,
                Type = AppType.PRIVATE
            };
            var source2 = new App
            {
                Id = Guid.NewGuid().ToString(),
                Name = "xxxx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true,
                Type = AppType.Inheritance
            };
            var source3 = new App
            {
                Id = Guid.NewGuid().ToString(),
                Name = "xxxx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = false,
                Type = AppType.Inheritance
            };
            var result = await _appservice.AddAsync(source);
            await _appservice.AddAsync(source1);
            await _appservice.AddAsync(source2);
            await _appservice.AddAsync(source3);

            Assert.IsTrue(result);

            var apps = await _appservice.GetAllInheritancedAppsAsync();

            Assert.AreEqual(2, apps.Count);
        }
        [TestMethod()]
        public async Task GetInheritancedAppsAsyncTest()
        {
            this.ClearData();

            var id = Guid.NewGuid().ToString();
            var source = new App
            {
                Id = id,
                Name = "xx",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true,
                Type = AppType.PRIVATE
            };
            var source1 = new App
            {
                Id = Guid.NewGuid().ToString(),
                Name = "xx1",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true,
                Type = AppType.Inheritance
            };
            var source2 = new App
            {
                Id = Guid.NewGuid().ToString(),
                Name = "xx2",
                Secret = "sec",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Enabled = true,
                Type = AppType.Inheritance
            };
            //
            var appInher = new AppInheritanced();
            appInher.Id = Guid.NewGuid().ToString();
            appInher.AppId = source.Id;
            appInher.InheritancedAppId = source1.Id;
            appInher.Sort = 1;
            var appInher1 = new AppInheritanced();
            appInher1.Id = Guid.NewGuid().ToString();
            appInher1.AppId = source.Id;
            appInher1.InheritancedAppId = source2.Id;
            appInher1.Sort = 2;

            var result = await _appservice.AddAsync(source);
            await _appservice.AddAsync(source1);
            await _appservice.AddAsync(source2);

            await _serviceProvider.GetService<IAppInheritancedRepository>().InsertAsync(appInher);
            await _serviceProvider.GetService<IAppInheritancedRepository>().InsertAsync(appInher1);

            Assert.IsTrue(result);

            var apps = await _appservice.GetInheritancedAppsAsync(source.Id);

            Assert.AreEqual(2, apps.Count);
        }

        [TestCleanup]
        public void Clean()
        {
        }
    }
}