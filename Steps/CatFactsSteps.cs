using NUnit.Framework;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[Binding]
public class CatFactsSteps
{
    private RestClient _client;
    private RestRequest? _request;
    private RestResponse? _response;
    

    public CatFactsSteps()
    {
        _client = new RestClient("https://cat-fact.herokuapp.com");
        
    }
   

    [Given(@"I have the Cat Facts API endpoint")]
    public void GivenIHaveTheCatFactsAPIEndpoint()
    {
        // Client is initialized in the constructor.
    }

    [When(@"I send a GET request for random facts with animal type ""(.*)"" and amount (.*)")]
    public void WhenISendAGETRequestForRandomFacts(string animalType, int amount)
    {
        _request = new RestRequest($"/facts/random?animal_type={animalType}&amount={amount}", Method.Get);
        _response = _client.Execute(_request);
    }

    [Then(@"the Cat Facts response should be successful")]
    public void ThenTheCatFactsResponseShouldBeSuccessful()
    {
        Assert.That(_response?.IsSuccessful, Is.True, "The response was not successful.");
    }

    [Then(@"the response should contain exactly (.*) facts")]
    public void ThenTheResponseShouldContainExactlyFacts(int expectedAmount)
    {
        var jsonResponse = JsonConvert.DeserializeObject<JArray>(_response?.Content);
        Assert.That(jsonResponse?.Count, Is.EqualTo(expectedAmount), "The number of facts is not correct.");
    }

    [Then(@"each fact should be about a ""(.*)""")]
    public void ThenEachFactShouldBeAboutTheSpecifiedAnimal(string expectedAnimalType)
    {
        var jsonResponse = JsonConvert.DeserializeObject<JArray>(_response?.Content);
        foreach (var fact in jsonResponse)
        {
            Assert.That(fact["type"]?.ToString(), Is.EqualTo(expectedAnimalType), "Fact is not about the specified animal.");
        }
    }

    [When(@"I send a GET request for a fact with ID ""(.*)""")]
    public void WhenISendAGETRequestForAFactWithID(string factId)
    {
        _request = new RestRequest($"/facts/{factId}", Method.Get);
        _response = _client.Execute(_request);

        Assert.That(_response?.IsSuccessful, Is.True, "Failed to retrieve the specific fact.");
    }

    [Then(@"the response should contain the correct fact data for the given ID")]
    public void ThenTheResponseShouldContainTheCorrectFactDataForTheGivenID()
    {
        if (_response?.IsSuccessful != true || string.IsNullOrEmpty(_response?.Content))
        {
            throw new InvalidOperationException("The response was not successful or the content is empty.");
        }
   
        var jsonResponse = JsonConvert.DeserializeObject<JObject>(_response.Content);

        var factId = jsonResponse?["_id"]?.ToString(); 
        Assert.That(factId, Is.EqualTo("591f98803b90f7150a19c229"), "Fact ID does not match.");
    }

    [Given(@"I am authenticated with a valid API")]
    public void GivenIAmAuthenticatedWithAValidAPI()
    {
        //User is authenticated
    }

   
    [When(@"I send a GET request to retrieve my queued facts with animal type ""(.*)""")]
    public void WhenISendAGETRequestToRetrieveMyQueuedFacts(string animalType)
    {
        _request = new RestRequest($"/facts?animal_type={animalType}", Method.Get);
        _response = _client.Execute(_request);
    }

    [Then(@"the response should contain an array of facts")]
    public void ThenTheResponseShouldContainAnArrayOfFacts()
    {
        if (_response?.Content == null)
        {
            throw new Exception("Response content is null. Could not retrieve facts.");
        }
        var jsonResponse = JsonConvert.DeserializeObject<JArray>(_response.Content);
        Assert.That(jsonResponse, Is.Not.Null, "Response is not a valid array of facts.");
    }


    [Then(@"each fact should have a type ""(.*)""")]
    public void ThenEachFactShouldHaveAType(string expectedAnimalType)
    {
        var jsonResponse = JsonConvert.DeserializeObject<JArray>(_response?.Content);
        foreach (var fact in jsonResponse)
        {
            Assert.That(fact["type"].ToString(), Is.EqualTo(expectedAnimalType), "Fact does not have the correct type.");
        }
    }
}
