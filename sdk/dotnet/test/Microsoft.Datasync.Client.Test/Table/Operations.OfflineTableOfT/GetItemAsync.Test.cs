﻿// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Datasync.Common.Test.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Datasync.Client.Test.Table.Operations.OfflineTableOfT
{
    [ExcludeFromCodeCoverage]
    public class GetItemAsync_Tests : BaseOperationTest
    {
        [Fact]
        [Trait("Method", "GetItemAsync")]
        public async Task GetItemAsync_ThrowsOnNull()
        {
            await table.ServiceClient.InitializeOfflineStoreAsync();
            await Assert.ThrowsAsync<ArgumentNullException>(() => table.GetItemAsync(null)).ConfigureAwait(false);
        }

        [Theory]
        [MemberData(nameof(BaseOperationTest.GetInvalidIds), MemberType = typeof(BaseOperationTest))]
        [Trait("Method", "GetItemAsync")]
        public async Task GetItemAsync_ThrowsOnInvalidId(string id)
        {
            await table.ServiceClient.InitializeOfflineStoreAsync();
            await Assert.ThrowsAsync<ArgumentException>(() => table.GetItemAsync(id)).ConfigureAwait(false);
        }

        [Fact]
        [Trait("Method", "GetItemAsync")]
        public async Task GetItemAsync_MissingItem()
        {
            // Arrange
            await table.ServiceClient.InitializeOfflineStoreAsync();
            var sId = Guid.NewGuid().ToString();

            // Act
            var response = await table.GetItemAsync(sId);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        [Trait("Method", "GetItemAsync")]
        public async Task GetItemAsync_WithItem()
        {
            // Arrange
            await table.ServiceClient.InitializeOfflineStoreAsync();
            var item = GetSampleMovie<ClientMovie>();
            item.Id = Guid.NewGuid().ToString();
            item.UpdatedAt = DateTimeOffset.UtcNow;
            var json = (JObject)table.ServiceClient.Serializer.Serialize(item);
            store.Upsert("movies", new[] { json });

            // Act
            var response = await table.GetItemAsync(item.Id).ConfigureAwait(false);

            // Assert
            Assert.Equal(item.Title, response.Title);
        }
    }
}