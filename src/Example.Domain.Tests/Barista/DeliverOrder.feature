Feature: Deliver an order
	In order to keep my job
	As a barista
	I want to give the drinks to the customers

@domain
Scenario: Deliver a prepared order
	Given the manager has set up the menu
	And the cashier has queued an order for a small latte, whole milk
	And I have begun preparing the order
	And I have prepared the order
	When I deliver the order
	Then the order is delivered
	And nothing else happens

@domain
Scenario: Deliver an in-progress order
	Given the manager has set up the menu
	And the cashier has queued an order for a small latte, whole milk
	And I have begun preparing the order
	When I deliver the order
	Then the order is prepared
	And the order is delivered
	And nothing else happens

@domain
Scenario: Deliver a queued order
	Given the manager has set up the menu
	And the cashier has queued an order for a small latte, whole milk
	When I deliver the order
	Then the order is being prepared
	And the order is prepared
	And the order is delivered
	And nothing else happens

