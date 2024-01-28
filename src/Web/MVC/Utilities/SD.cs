namespace MVC.Models;



public static class SD
{
    public static string ProductAPIBase { get; set; }
    public static string AuthAPIBase { get; set; }
    public  enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
