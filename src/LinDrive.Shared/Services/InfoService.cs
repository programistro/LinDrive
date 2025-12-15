using IPinfo;
using IPinfo.Models;
using LinDrive.Shared.Interfaces;

namespace LinDrive.Shared.Services;

public class InfoService : IInfoService
{
    public async Task<string> GetLocation(string ip, CancellationToken cancellationToken)
    {
        string token = "6d81ceaf0cfdf2";
        IPinfoClient client = new IPinfoClient.Builder()
            .AccessToken(token)
            .Build();

        IPResponse ipResponse = await client.IPApi.GetDetailsAsync(ip);

        Console.WriteLine($"IP address: {ipResponse.IP}");
        Console.WriteLine($"City: {ipResponse.City}");
        Console.WriteLine($"Region / State: {ipResponse.Region}");
        Console.WriteLine($"Country : {ipResponse.Postal}");
        Console.WriteLine($"Country: {ipResponse.Country}");
        Console.WriteLine($"Country Name: {ipResponse.CountryName}");
        Console.WriteLine($"Geographic Coordinate: {ipResponse.Loc}");
        Console.WriteLine($"Contitnent: {ipResponse.Continent.Name}");
        return ipResponse.City;
    }
}