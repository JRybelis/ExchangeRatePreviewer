using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;
using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;


namespace ExchangeRatePreviewer.Core.Services.SOAP;

public class BankOfLithuaniaSoapClient : IBankOfLithuaniaSoapClient
{
    /// <summary>
    /// Gets the settings for the Bank of Lithuania HTTP client. 
    /// </summary>
    private IBankOfLithuaniaSoapClientSettings ClientSettings { get; }
    private readonly ILogger<BankOfLithuaniaSoapClient> _logger;
    private readonly IResultParser _resultParser;
    private HttpClient _httpClient;
    private Uri? _relativeUri;

    public BankOfLithuaniaSoapClient(IBankOfLithuaniaSoapClientSettings clientSettings,
        ILogger<BankOfLithuaniaSoapClient> logger, IResultParser resultParser)
    {
        ClientSettings = clientSettings;
        _logger = logger;
        _resultParser = resultParser;
        _httpClient = GetClient();
    }

    /// <summary>
    /// Instantiates an HTTP client, sets its BaseAddress, DefaultRequestHeader, SecurityProtocol, and Timeout values up.
    /// </summary>
    /// <returns>HttpClient</returns>
    /// <exception cref="InvalidOperationException">If no URL is provided for the BaseAddress.</exception>
    private HttpClient GetClient()
    {
        _httpClient = new HttpClient(
            new HttpClientHandler 
            { 
                AllowAutoRedirect = true, 
                MaxAutomaticRedirections = 5 
            });
        
        _httpClient.BaseAddress = new Uri(ClientSettings.BaseUrl!, UriKind.Absolute);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        _httpClient.DefaultRequestHeaders.Host = ClientSettings.Host;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

        return _httpClient;
    }

    public async Task<List<ExchangeRateDto>?> GetExchangeRatesByDate(DateTime date)
    {
        var dateString = date.ToShortDateString();
        var requestUrl = $"{ClientSettings.RequestUrl}getExchangeRatesByDate?Date={dateString}"; 
        
        _relativeUri = new Uri(requestUrl, UriKind.Relative);
        var requestUri = new Uri(_httpClient.BaseAddress!, _relativeUri);
        
        var responseMessage = await MakeRestRequest(requestUri);
        var responseString = await responseMessage.Content.ReadAsStringAsync();

        return _resultParser.ParseResponseXmlElement(responseString);
    }

    private async Task<HttpResponseMessage> MakeRestRequest(Uri requestUri)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = requestUri,
            Method = HttpMethod.Get,
            Headers = { Host = ClientSettings.Host}
        };
        
        var response = await _httpClient.SendAsync(request);
        var statusCode = (int)response.StatusCode;
        
        if (statusCode is >= 300 and <= 399)
        {
            var redirectUri = response.Headers.Location;
            if (!redirectUri.IsAbsoluteUri) redirectUri = new Uri(_httpClient.BaseAddress!, redirectUri);

            return await MakeRestRequest(redirectUri);
        }
        if(!response.IsSuccessStatusCode)
            throw new Exception();

        return response;
    }
}