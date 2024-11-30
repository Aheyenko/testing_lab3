Feature: Booking API Operations


  Scenario: Retrieve booking by ID
    Given I have the booking API endpoint
    When I send a GET request for ID list
    And I send a GET request for booking ID 1
    Then the response should be successful

  Scenario: Create a new booking successfully
    Given I have the booking API endpoint
    When I send a POST request to create a new booking
    Then the booking should be created successfully


  Scenario: Update an existing booking successfully
    Given I have the booking API endpoint
    When I send a GET request for ID list
    And I send a PUT request to update booking ID 1
    Then the booking should be updated successfully

  Scenario: Update a current booking with a partial payload
    Given I have the booking API endpoint
    When I send a GET request for ID list
    And I sent a PATCH request to update a current booking ID 1
    Then the booking should be update with a partional payload 

  Scenario: Delete an existing booking successfully
    Given I have the booking API endpoint
    When I send a GET request for ID list
    And I send a DELETE request for booking ID 1
    Then the booking should be deleted successfully
