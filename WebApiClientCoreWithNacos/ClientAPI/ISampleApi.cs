[HttpHost("http://sample")]
public interface ISampleApi : IHttpApi
{
    [HttpGet("/api/get")]
    Task<string> GetAsync();
}