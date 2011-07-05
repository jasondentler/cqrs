Feature: Cancel an order
	In order to keep my job
	As a cashier
	I want to cancel customers' orders

@domain
Scenario: Cancel an order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	When I cancel the order
	Then the order is cancelled 
	And nothing else happens

@domain
Scenario: Cancel a paid order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	And I have paid for the order
	When I cancel the order
	Then the aggregate state is invalid
	And the error is "You can't cancel this order. It has already been paid."

@domain
Scenario: Cancel a cancelled order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	And I have cancelled the order
	When I cancel the order
	Then nothing happens
