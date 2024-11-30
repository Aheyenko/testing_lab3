using NUnit.Framework;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[Binding]
public class BookingSteps
{
    private RestClient _client;
    private RestRequest? _request;
    private RestResponse? _response;
    private string _token;

    public BookingSteps()
    {
        _client = new RestClient("https://restful-booker.herokuapp.com");
        _token = GetAuthToken(); // Отримання токена при ініціалізації
    }

    private string GetAuthToken()
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddJsonBody(new { username = "admin", password = "password123" });

        var response = _client.ExecutePost(request);

        if (response.IsSuccessful && response.Content != null)
        {
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            return jsonResponse.token;
        }
        else
        {
            throw new Exception("Failed to retrieve the token");
        }
    }

    [Given(@"I have the booking API endpoint")]
    public void GivenIHaveTheBookingAPIEndpoint()
    {
        // Client is already initialized in the constructor.
    }
    [When(@"I send a GET request for ID list")]
    public void WhenISendAGETRequestForIDlist()
    {
        _request = new RestRequest($"/booking/", Method.Get);
       
        _response = _client.Execute(_request);

        Assert.That(_response?.IsSuccessful, Is.True, "Failed to retrieve booking ID list.");
    }
    [When(@"I send a GET request for booking ID (.*)")]
    public void WhenISendAGETRequestForBookingID(int index)
    {
        JArray bookingList = JArray.Parse(_response?.Content);
        int firstBookingId = (int)bookingList[index]["bookingid"];

        // Використання firstBookingId у новому запиті для отримання детальної інформації
        _request = new RestRequest($"/booking/{firstBookingId}", Method.Get);
        _request.AddHeader("Accept", "application/json");
        _response = _client.Execute(_request);
    }

    [Then(@"the response should be successful")]
    public void ThenTheResponseShouldBeSuccessful()
    {
        Assert.That(_response?.IsSuccessful, Is.True, "The response was not successful");
    }

    [When(@"I send a POST request to create a new booking")]
    public void WhenISendAPOSTRequestToCreateANewBooking()
    {
        _request = new RestRequest("/booking", Method.Post);
        _request.AddHeader("Accept", "application/json");
        _request.AddHeader("Content-Type", "application/json");
        _request.AddJsonBody(new
        {
            firstname = "John",
            lastname = "Doe",
            totalprice = 120,
            depositpaid = true,
            bookingdates = new { checkin = "2023-01-01", checkout = "2023-01-10" }
        });
        _response = _client.Execute(_request);

       
    }
    [Then(@"the booking should be created successfully")]
    public void TheBookingShouldBeCreatedSuccessfully()
    {
        Assert.That(_response?.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The booking creation was not successful");
    }

    [When(@"I send a PUT request to update booking ID (.*)")]
    public void WhenISendAPUTRequestToUpdateBookingID(int index)
    {
        JArray bookingList = JArray.Parse(_response?.Content);
        int firstBookingId = (int)bookingList[index]["bookingid"];

        _request = new RestRequest($"/booking/{firstBookingId}", Method.Put);
        _request.AddHeader("Cookie", $"token={_token}");
        _request.AddHeader("Accept", "application/json");
        _request.AddHeader("Content-Type", "application/json");
        _request.AddJsonBody(new
        {
            firstname = "John",
            lastname = "Doe",
            totalprice = 120,
            depositpaid = true,
            bookingdates = new { checkin = "2023-01-01", checkout = "2023-01-10" }
        });
        _response = _client.Execute(_request);
        
    }

    [Then(@"the booking should be updated successfully")]
    public void TheBookingShouldBeUpdatedSuccessfully()
    {
        Assert.That(_response?.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [When(@"I sent a PATCH request to update a current booking ID (.*)")]
    public void WhenISentAPATCHRequestToUpdateACurrentBooking(int index)
    {
        JArray bookingList = JArray.Parse(_response?.Content);
        int firstBookingId = (int)bookingList[index]["bookingid"];

        _request = new RestRequest($"/booking/{firstBookingId}", Method.Patch);
        _request.AddHeader("Cookie", $"token={_token}");
        _request.AddHeader("Accept", "application/json");
        _request.AddHeader("Content-Type", "application/json");
        _request.AddJsonBody(new
        {
            firstname = "John",
            lastname = "Doe",
        });
        _response = _client.Execute(_request);

    }

    [Then(@"the booking should be update with a partional payload")]
    public void TheBookingShouldBeUpdatedWithAPartionalPayload()
    {
        Assert.That(_response?.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [When(@"I send a DELETE request for booking ID (.*)")]
    public void WhenISendADELETERequestForBookingID(int index)
    {
        JArray bookingList = JArray.Parse(_response?.Content);
        int firstBookingId = (int)bookingList[index]["bookingid"];

        _request = new RestRequest($"/booking/{firstBookingId}", Method.Delete);
        _request.AddHeader("Cookie", $"token={_token}");
        _request.AddHeader("Content-Type", "application/json");
        _response = _client.Execute(_request);
        
    }
    [Then(@"the booking should be deleted successfully")]
    public void TheBookingShouldBeDeletedSuccessfully()
    {
        Assert.That(_response?.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}
