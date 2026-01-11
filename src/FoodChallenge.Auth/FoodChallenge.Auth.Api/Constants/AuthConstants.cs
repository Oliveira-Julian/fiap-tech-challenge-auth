namespace FoodChallenge.Auth.Api.Constants;

public static class AuthConstants
{
    public static class Scopes
    {
        // Orders
        public const string OrdersRead = "orders.read";
        public const string OrdersWrite = "orders.write";
        
        // Configurations
        public const string ConfigurationsRead = "configurations.read";
        public const string ConfigurationsWrite = "configurations.write";
        
        // Payments
        public const string PaymentsRead = "payments.read";
        public const string PaymentsWrite = "payments.write";
        
        // Kitchens
        public const string KitchensRead = "kitchens.read";
        public const string KitchensWrite = "kitchens.write";
    }
    
    public static class Audiences
    {
        public const string OrdersApi = "orders-api";
        public const string ConfigurationsApi = "configurations-api";
        public const string PaymentsApi = "payments-api";
        public const string KitchensApi = "kitchens-api";
    }
}
