using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using ExchangeRatePreviewer.Core.Interfaces.Services;
using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;
using Microsoft.Extensions.Logging;

namespace ExchangeRatePreviewer.Core.Services.SOAP;

public class BankOfLithuaniaSoapClient : IBankOfLithuaniaSoapClient
{
    /// <summary>
    /// Gets the settings for the Bank of Lithuania HTTP client. 
    /// </summary>
    private IBankOfLithuaniaSoapClientSettings ClientSettings { get; }
    private readonly ILogger<BankOfLithuaniaSoapClient> _logger;
    private HttpClient _httpClient;
    private readonly Uri _relativeUri;

    public BankOfLithuaniaSoapClient(IBankOfLithuaniaSoapClientSettings clientSettings,
        ILogger<BankOfLithuaniaSoapClient> logger)
    {
        ClientSettings = clientSettings;
        _logger = logger;
        _relativeUri = new Uri(clientSettings.RequestUrl, UriKind.Relative);
        _httpClient = GetClient();
    }

    /// <summary>
    /// Instantiates an HTTP client, sets its BaseAddress, DefaultRequestHeader, SecurityProtocol, and Timeout values up.
    /// </summary>
    /// <param name="clientSettings">BankOfLithuaniaSoapClientSettings model.</param>
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
        
        _httpClient.BaseAddress = new Uri(ClientSettings.BaseUrl, UriKind.Absolute);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        _httpClient.DefaultRequestHeaders.Host = ClientSettings.Host;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13; // newest or use tls12 if their api does not support

        return _httpClient;
    }

    public async Task<BankOfLithuaniaExchangeRateServiceResponse?> GetExchangeRatesByDate(
        DateTime date)
    {
        var dateString = date.ToShortDateString(); // Format: YYYY-MM-DDT:hh:mm:ss
        
        var payload = 
$@"<soap12:Body>
        <getExchangeRatesByDate xmlns={_httpClient.BaseAddress}>
            <Date>{dateString}</Date>
        </getExchangeRatesByDate>
    </soap12:Body>";

        var requestContent = new StringContent(GenerateEnvelope(payload), Encoding.UTF8, "application/soap+xml");
        //requestContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/soap+xml");
        //requestContent.Headers.Add("Host", "webservices.lb.lt");
        var responseMessage = await MakeRequest(requestContent);
        var responseString = await responseMessage.Content.ReadAsStringAsync();

        return ParseResponseXmlElement(responseString, ClientSettings);
    }

    private async Task<HttpResponseMessage> MakeRequest(StringContent requestContent)
    {
        var responseMessage = await _httpClient.PostAsync(_relativeUri, requestContent);
        var statusCode = (int)responseMessage.StatusCode;
        if (statusCode is >= 300 and <= 399)
        {
            var redirectUri = responseMessage.Headers.Location;
            if (!redirectUri.IsAbsoluteUri) redirectUri = new Uri(_httpClient.BaseAddress!, redirectUri);
            requestContent.Headers.ContentLocation = redirectUri;
            
            return await MakeRequest(requestContent);
        }

        if(!responseMessage.IsSuccessStatusCode)
            throw new Exception();

        return responseMessage;
    }

    /// <summary>
    /// Builds the SOAP envelope, combining Headers with Body 
    /// </summary>
    /// <param name="payload">Request body</param>
    /// <returns>XML string SOAP envelope</returns>
    private string GenerateEnvelope(string payload)
    {
        /*POST /webservices/exchangerates/exchangerates.asmx HTTP/1.1
            Host:webservices.lb.lt
            Content-Type: application/soap+xml; charset=utf-8
            Content-Length: length*/
        return
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
    xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
    {payload}
</soap12:Envelope>";
    }

    
    private static BankOfLithuaniaExchangeRateServiceResponse ParseResponseXmlElement(string responseString,
        IBankOfLithuaniaSoapClientSettings clientSettings)
    {
        BankOfLithuaniaExchangeRateServiceResponse response = new()
        {
            ExchangeRateDtos = new List<ExchangeRateDto>()
        };
        

        var xmlDocument = new XmlDocument();
        var resultXmlNamespace = clientSettings.ResultXmlNamespace;
        xmlDocument.LoadXml(responseString);

        var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
        xmlNamespaceManager.AddNamespace("namespace", $"{resultXmlNamespace}");

        var resultNodeList = xmlDocument.SelectNodes("//namespace:result", xmlNamespaceManager);
        var resultChildren = resultNodeList.Item(0).ChildNodes;

        try
        {
            // Populate BankOfLithuaniaExchangeRateServiceResponse with parsed values
            foreach (XmlElement resultChild in resultChildren)
            {
                var exchangeRateDto = new ExchangeRateDto();
                switch (resultChild.LocalName)
                {
                    case "date":
                        exchangeRateDto.Date = DateTime.Parse(resultChild.InnerText);
                        break;
                    case "currency":
                        exchangeRateDto.Currency = resultChild.InnerText;
                        break;
                    case "quantity":
                        exchangeRateDto.Quantity =
                            int.Parse(resultChild.InnerText);
                        break;
                    case "rate":
                        var isParseToDecimalSuccess = decimal.TryParse(resultChild.InnerText, out var rate);
                        exchangeRateDto.Rate = isParseToDecimalSuccess ? rate : decimal.Zero;
                        break;
                    case "unit":
                        exchangeRateDto.UnitDescription = resultChild.InnerText;
                        break;
                }
                response.ExchangeRateDtos.Add(exchangeRateDto);
            }
        }
        catch (Exception ex)
        {
            //_logger.Debug($"{ex.InnerException} caused {ex.Message}.");
            throw;
        }

        return response;
    }
}

public class BankOfLithuaniaExchangeRateServiceResponse
{
    internal bool IsSuccess { get; set; }
    public long? TransactionId { get; set; }
    public long? ErrorCode { get; set; }
    public string? ErrorDescription { get; set; }
    public List<ExchangeRateDto> ExchangeRateDtos { get; set; }
}