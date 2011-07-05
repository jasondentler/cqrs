Feature: Prepare an order
	In order to keep my job
	As a barista
	I want to prepare drinks for the customers

@domain
Scenario: Begin preparing an order
	Given the manager has set up the menu
	And the cashier has queued an order for a small latte, whole milk
	When I begin preparing the order
	Then the order is being prepared
	And nothing else happens

@domain
Scenario: Prepare an order
	Given the manager has set up the menu
	And the cashier has queued an order for a small latte, whole milk
	And I have begun preparing the order
	When I prepare the order
	Then the order is prepared
	And nothing else happens

@domain
Scenario: Prepare an order without beginning preparation
	Given the manager has set up the menu
	And the cashier has queued an order for a small latte, whole milk
	When I prepare the order
	Then the order is being prepared
	And the order is prepared
	And nothing else happens
