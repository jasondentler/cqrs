Feature: Pay for order
	In order to get my morning caffiene fix
	As a customer
	I want to pay for my coffee order

@domain
Scenario: Pay for an order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	When I pay for the order
	Then the order is paid for 
	And the cashier queues the order to the barista
	And the order is queued to the barista
	And nothing else happens

@domain
Scenario: Pay for a cancelled order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	And I have cancelled the order
	When I pay for the order
	Then the aggregate state is invalid
	And the error is "You can't pay for this order. It is cancelled. Place a new order."

@domain
Scenario: Pay for a paid order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	And I have paid for the order
	When I pay for the order
	Then the aggregate state is invalid
	And the error is "You can't pay for this order. It's already paid."
