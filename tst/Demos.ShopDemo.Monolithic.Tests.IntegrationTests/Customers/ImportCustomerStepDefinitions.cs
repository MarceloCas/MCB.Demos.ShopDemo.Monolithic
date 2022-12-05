using Demos.ShopDemo.Monolithic.Tests.IntegrationTests.Fixtures;
using FluentAssertions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Enums;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace Demos.ShopDemo.Monolithic.Tests.IntegrationTests.Customers;

[Binding]
public class ImportCustomerStepDefinitions
{
    // Fields
    private readonly ITestOutputHelper _output;
    private readonly DefaultFixture _fixture;
    private readonly HttpClient _httpClient;
    private readonly string _importCustomerUrl;

    // Test fields
    private ImportCustomerPayload _importCustomerPayload = null!;
    private HttpResponseMessage _importCustomerHttpResponseMessage = null!;
    private ImportCustomerResponse? _importCustomerResponse;

    // Constructors
    public ImportCustomerStepDefinitions(
        ITestOutputHelper output,
        DefaultFixture fixture
    )
    {
        _output = output;
        _fixture = fixture;
        _httpClient = new HttpClient();

        _importCustomerUrl = "http://localhost:5000/api/v1/Customers/import";
    }

    [Given(@"a customer who does not have the registered email")]
    public void GivenACustomerWhoDoesNotHaveTheRegisteredEmail()
    {
        _importCustomerPayload = _fixture.GenerateNewImportCustomerPayload();
    }

    [Given(@"a customer who does have the registered email")]
    public async Task GivenACustomerWhoDoesHaveTheRegisteredEmail()
    {
        var importCustomerPayload = _fixture.GenerateNewImportCustomerPayload();
        var response = await _httpClient.PostAsJsonAsync(_importCustomerUrl, importCustomerPayload, cancellationToken: default);

        _importCustomerPayload = importCustomerPayload;
    }

    [When(@"the customer import is requested")]
    public async Task WhenTheCustomerImportIsRequested()
    {
        _importCustomerHttpResponseMessage = await _httpClient.PostAsJsonAsync(
            requestUri: _importCustomerUrl,
            value: _importCustomerPayload,
            cancellationToken: default
        );

        var content = await _importCustomerHttpResponseMessage.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(content))
            _importCustomerResponse = JsonSerializer.Deserialize<ImportCustomerResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }

    [Then(@"the client should be successfully imported")]
    public async void ThenTheClientShouldBeSuccessfullyImported()
    {
        _importCustomerHttpResponseMessage.Should().NotBeNull();
        (await _importCustomerHttpResponseMessage.Content.ReadAsStringAsync()).Should().BeEmpty();
        _importCustomerHttpResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Then(@"the client should not be successfully imported")]
    public void ThenTheClientShouldNotBeSuccessfullyImported()
    {
        _importCustomerHttpResponseMessage.Should().NotBeNull();
        _importCustomerHttpResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);

        _importCustomerResponse.Should().NotBeNull();
        _importCustomerResponse!.Messages.Should().HaveCount(1);
    }

    [Then(@"an customer email message already registered must be returned")]
    public void ThenAnCustomerEmailMessageAlreadyRegisteredMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault();

        message.Should().NotBeNull();
        message?.Code.Should().Be(CustomerService.CustomerEmailAlreadyRegisteredErrorCode.ToString());
        message?.Type.Should().Be(Parse(CustomerService.CustomerEmailAlreadyRegisteredNotificationType));
    }

    private static ResponseMessageType Parse(NotificationType notificationType)
    {
        return notificationType switch
        {
            NotificationType.Information => ResponseMessageType.Information,
            NotificationType.Warning => ResponseMessageType.Warning,
            NotificationType.Error => ResponseMessageType.Error,
            _ => throw new NotImplementedException(),
        };
    }
}
