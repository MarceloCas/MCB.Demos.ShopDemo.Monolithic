using Demos.ShopDemo.Monolithic.Tests.IntegrationTests.Fixtures;
using FluentAssertions;
using FluentValidation;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Enums;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Responses;
using System.Net.Http.Json;
using System.Text;
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
    private ImportCustomerPayload? _importCustomerPayload;
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

    // Private methods
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
    private static ResponseMessageType Parse(Severity severity)
    {
        return severity switch
        {
            Severity.Info => ResponseMessageType.Information,
            Severity.Warning => ResponseMessageType.Warning,
            Severity.Error => ResponseMessageType.Error,
            _ => throw new NotImplementedException(),
        };
    }

    #region [ Given ]

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

        response.IsSuccessStatusCode.Should().BeTrue();

        _importCustomerPayload = importCustomerPayload;
    }

    [Given(@"a customer who does have empty informations")]
    public void GivenACustomerWhoDoesHaveEmptyInformations()
    {
        _importCustomerPayload = null;
    }

    #endregion [ Given ]

    #region [ When ]

    [When(@"the customer import is requested")]
    public async Task WhenTheCustomerImportIsRequested()
    {
        _importCustomerHttpResponseMessage = _importCustomerPayload is null
            ? await _httpClient.PostAsync(
                requestUri: _importCustomerUrl,
                content: new StringContent(content: "{}", encoding: Encoding.UTF8, mediaType: "application/json"),
                cancellationToken: default
            )
            : await _httpClient.PostAsJsonAsync(
                requestUri: _importCustomerUrl,
                value: _importCustomerPayload,
                cancellationToken: default
            );

        var content = await _importCustomerHttpResponseMessage.Content.ReadAsStringAsync();

        if (!string.IsNullOrWhiteSpace(content))
            _importCustomerResponse = JsonSerializer.Deserialize<ImportCustomerResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    #endregion [ When ]

    #region [ Then ]
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
    }

    [Then(@"a customer email already registered message must be returned")]
    public void ThenAnCustomerEmailMessageAlreadyRegisteredMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == CustomerService.CustomerEmailAlreadyRegisteredErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(CustomerService.CustomerEmailAlreadyRegisteredNotificationType));
    }

    [Then(@"a should have tenantId message must be returned")]
    public void ThenAShouldHaveTenantIdMessageMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == IInputBaseValidator.InputBaseShouldHaveTenantIdErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(IInputBaseValidator.InputBaseShouldHaveTenantIdSeverity));
    }

    [Then(@"a should have executionUser message must be returned")]
    public void ThenAShouldHaveExecutionUserMessageMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == IInputBaseValidator.InputBaseShouldHaveExecutionUserErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(IInputBaseValidator.InputBaseShouldHaveExecutionUserSeverity));
    }

    [Then(@"a should have sourcePlatform message must be returned")]
    public void ThenAShouldHaveSourcePlatformMessageMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == IInputBaseValidator.InputBaseShouldHaveSourcePlatformErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(IInputBaseValidator.InputBaseShouldHaveSourcePlatformSeverity));
    }

    [Then(@"a should have firstName message must be returned")]
    public void ThenAShouldHaveFirstNameMessageMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == ICustomerSpecifications.CustomerShouldHaveFirstNameErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(ICustomerSpecifications.CustomerShouldHaveFirstNameSeverity));
    }

    [Then(@"a should have lastName message must be returned")]
    public void ThenAShouldHaveLastNameMessageMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == ICustomerSpecifications.CustomerShouldHaveLastNameErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(ICustomerSpecifications.CustomerShouldHaveLastNameSeverity));
    }

    [Then(@"a should have birthDate message must be returned")]
    public void ThenAShouldHaveBirthDateMessageMustBeReturned()
    {
        var message = _importCustomerResponse?.Messages?.FirstOrDefault(q =>
            q.Code == ICustomerSpecifications.CustomerShouldHaveBirthDateErrorCode
        );

        message.Should().NotBeNull();
        message?.Type.Should().Be(Parse(ICustomerSpecifications.CustomerShouldHaveBirthDateSeverity));
    }

    #endregion [ Then ]
}
