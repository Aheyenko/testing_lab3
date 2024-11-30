Feature: Cat Facts API Operations

  Scenario: Retrieve random cat facts
    Given I have the Cat Facts API endpoint
    When I send a GET request for random facts with animal type "cat" and amount 2
    Then the Cat Facts response should be successful
    Then the response should contain exactly 2 facts
    Then each fact should be about a "cat"
    

  Scenario: Retrieve a specific fact by ID
    Given I have the Cat Facts API endpoint
    When I send a GET request for a fact with ID "591f98803b90f7150a19c229"
    Then the Cat Facts response should be successful
    Then the response should contain the correct fact data for the given ID

  Scenario: Retrieve queued facts for the authenticated user
    Given I am authenticated with a valid API
    When I send a GET request to retrieve my queued facts with animal type "cat"    
    Then the Cat Facts response should be successful
    Then the response should contain an array of facts
    Then each fact should have a type "cat"
