namespace TwoCoinApi
{
    public static class TwoCoinExtensions
    {
        // Mở rộng cho IApplicationBuilder phương thức UseCheckAccess
        public static IApplicationBuilder UseCheckAccessTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckAccessTokenMiddleware>();
        }
    }
}
