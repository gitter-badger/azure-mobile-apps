﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Zumo.E2EServer.DataObjects;
using Microsoft.Zumo.E2EServer.Models;
using Microsoft.Zumo.Server;
using Microsoft.Zumo.Server.Entity;

namespace Microsoft.Zumo.E2EServer.Controllers
{
    [Route("tables/[controller]")]
    [ApiController]
    public class MoviesController : TableController<Movie>
    {
        public MoviesController(AppDbContext context)
        {
            TableRepository = new EntityTableRepository<Movie>(context);
        }

        /// <summary>
        /// The LIST operation (GET .../route), which implements OData Semantics.
        /// </summary>
        /// <returns>The result of the LIST operation</returns>
        [HttpGet, ZumoQuery(MaxTop = 1000)]
        public override IActionResult GetItems() 
            => Ok(QueryItems());

        /// <summary>
        /// Validate the operation - this table is read-only.  The GetItems() LIST method is
        /// handled directly, but the other methods don't need to be over-ridden.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public override int ValidateOperation(TableOperation operation, Movie item)
            => operation == TableOperation.Read ? StatusCodes.Status200OK : StatusCodes.Status405MethodNotAllowed;
    }
}
