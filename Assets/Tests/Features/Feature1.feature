Feature: Feature1

A short summary of the feature

@tag1
Scenario: Successful login
    Given I am on the login page
    When I enter valid credentials
    And I click the login button
    Then I should be redirected to the home page
