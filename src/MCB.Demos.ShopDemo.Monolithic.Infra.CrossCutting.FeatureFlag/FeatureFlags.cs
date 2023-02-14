namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag;
public static class FeatureFlags
{
    public const string IMPORT_CUSTOMER_FEATURE_FLAG_KEY = "import-customer";
    public const string IMPORT_CUSTOMER_BATCH_FEATURE_FLAG_KEY = "import-customer-batch";
    public const string DELETE_CUSTOMER_FEATURE_FLAG_KEY = "delete-customer";
    public const string GET_CUSTOMER_FEATURE_FLAG_KEY = "get-customer";

    public const string IMPORT_PRODUCT_FEATURE_FLAG_KEY = "import-product";
    public const string IMPORT_PRODUCT_BATCH_FEATURE_FLAG_KEY = "import-product-batch";
    public const string DELETE_PRODUCT_FEATURE_FLAG_KEY = "delete-product";
    public const string GET_PRODUCT_FEATURE_FLAG_KEY = "get-product";
}
