﻿using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Products.Payloads;

public class DeleteProductPayload
    : PayloadBase
{
    public string? Code { get; set; }
}
