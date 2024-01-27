using System.Net.Mime;

namespace MVC.Models;

public class RequestDto
{
    
    public SD.ApiType Sd { get; set; } = SD.ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
    
    
    
    
}