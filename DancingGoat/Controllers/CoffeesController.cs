﻿using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class CoffeesController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync<Coffee>(
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "processing", "url_pattern"),
                new DepthParameter(0)
            );

            return View(response.Items);
        }

        public async Task<ActionResult> Filter(CoffeesFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "processing", "url_pattern"),
                new DepthParameter(0),
            };

            var filter = model.GetFilteredValues().ToArray();
            if (filter.Any())
            {
                parameters.Add(new AnyFilter("elements.processing", filter));
            }

            var response = await client.GetItemsAsync<Coffee>(parameters);

            return PartialView("ProductListing", response.Items);
        }
    }
}