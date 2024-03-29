﻿namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataContexts.Models;

public class RedisOptions
{
    // Properties
    public string ConnectionString { get; }

    // Constructors
    public RedisOptions(string connectionString)
    {
        ConnectionString = connectionString;
    }
}