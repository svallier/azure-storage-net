// -----------------------------------------------------------------------------------------
// <copyright file="HttpFactoryTests.cs" company="Microsoft">
//    Copyright 2018 Microsoft Corporation
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// -----------------------------------------------------------------------------------------

namespace Microsoft.Azure.Storage.Core
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth.Protocol;
    using Microsoft.WindowsAzure.Storage.Core;
    using Microsoft.WindowsAzure.Storage.Shared.Protocol;
    using System;
    using System.Net.Http;

#if WINDOWS_DESKTOP || NETCOREAPP2_0
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

    [TestClass]
    public class HttpFactoryTests : TestBase
    {
        [TestMethod]
        [Description("HttpClientFromDelegatingHandler when delegatingHandler is null")]
        [TestCategory(ComponentCategory.Core)]
        [TestCategory(TestTypeCategory.UnitTest)]
        [TestCategory(SmokeTestCategory.Smoke)]
        [TestCategory(TenantTypeCategory.Cloud)]
        public void HttpClientFromDelegatingHandlerNullDelegatingHandlerTest()
        {
            HttpClient result = HttpClientFactory.HttpClientFromDelegatingHandler(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        [Description("HttpClientFromDelegatingHandler when delegatingHandler inner is not a DelegatingHandler")]
        [TestCategory(ComponentCategory.Core)]
        [TestCategory(TestTypeCategory.UnitTest)]
        [TestCategory(SmokeTestCategory.Smoke)]
        [TestCategory(TenantTypeCategory.Cloud)]
        public void HttpClientFromDelegatingHandlerExceptionTest()
        {
            TestHelper.ExpectedException<ArgumentException>(
                () => HttpClientFactory.HttpClientFromDelegatingHandler(new DelegatingHandlerImpl(new HttpClientHandler())),
                "HttpClientFromDelegatingHandler with DelegatingHandler with invalid inner should throw an ArgumentException",
                SR.DelegatingHandlerNonNullInnerHandler);
        }

        [TestMethod]
        [Description("HttpClientFromDelegatingHandler when delegatingHandler is null")]
        [TestCategory(ComponentCategory.Core)]
        [TestCategory(TestTypeCategory.UnitTest)]
        [TestCategory(SmokeTestCategory.Smoke)]
        [TestCategory(TenantTypeCategory.Cloud)]
        public void HttpClientFromDelegatingHandlerNullInnerTest()
        {
            DelegatingHandler delegatingHandler = new DelegatingHandlerImpl();
            StorageAuthenticationHttpHandler storageAuthenticationHttpHandler = StorageAuthenticationHttpHandler.Instance;
            HttpClientFactory.HttpClientFromDelegatingHandler(delegatingHandler);
            Assert.AreEqual(storageAuthenticationHttpHandler, delegatingHandler.InnerHandler);
        }

        [TestMethod]
        [Description("HttpClientFromDelegatingHandler with a chain of 2 valid DelegatingHandlers")]
        [TestCategory(ComponentCategory.Core)]
        [TestCategory(TestTypeCategory.UnitTest)]
        [TestCategory(SmokeTestCategory.Smoke)]
        [TestCategory(TenantTypeCategory.Cloud)]
        public void HttpClientFromDelegatingHandlerChainTest()
        {
            DelegatingHandler delegatingHandler = new DelegatingHandlerImpl(new DelegatingHandlerImpl());
            StorageAuthenticationHttpHandler storageAuthenticationHttpHandler = StorageAuthenticationHttpHandler.Instance;
            HttpClientFactory.HttpClientFromDelegatingHandler(delegatingHandler);
            Assert.AreEqual(storageAuthenticationHttpHandler, ((DelegatingHandler)delegatingHandler.InnerHandler).InnerHandler);
        }
    }
}