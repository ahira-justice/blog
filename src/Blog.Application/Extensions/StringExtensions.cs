namespace Blog.Application.Extensions
{
    public static class StringExtensions
    {
        public static string GetControllerName(this string value)
        {
            return value.Replace("Controller", "");
        }
    }
}
