using Demos.ShopDemo.Monolithic.Tests.IntegrationTests.Fixtures;
using FluentAssertions;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;
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
    private HttpResponseMessage _importCustomerResponse = null!;

    // Constructors
    public ImportCustomerStepDefinitions(
        ITestOutputHelper output,
        DefaultFixture fixture
    )
    {
        _output = output;
        _fixture = fixture;
        _httpClient = new HttpClient();

        _importCustomerUrl = "https://localhost:5001/api/v1/Customers/import";
    }

    [Given(@"a customer who does not have the registered email")]
    public void GivenACustomerWhoDoesNotHaveTheRegisteredEmail()
    {
        _output.WriteLine("Generating NewImportCustomerPayload");

        _importCustomerPayload = _fixture.GenerateNewImportCustomerPayload();

        _output.WriteLine($"NewImportCustomerPayload generated|Json:{JsonSerializer.Serialize(_importCustomerPayload)}");
    }

    [When(@"the customer import is requested")]
    public async Task WhenTheCustomerImportIsRequested()
    {
        _importCustomerResponse = await _httpClient.PostAsJsonAsync(
            requestUri: _importCustomerUrl,
            value: _importCustomerPayload,
            cancellationToken: default
        );
    }

    [Then(@"the client should be successfully imported")]
    public async void ThenTheClientShouldBeSuccessfullyImported()
    {
        _importCustomerResponse.Should().NotBeNull();
        (await _importCustomerResponse.Content.ReadAsStringAsync()).Should().BeEmpty();
        _importCustomerResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }
}
