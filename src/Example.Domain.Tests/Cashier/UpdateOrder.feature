Feature: Addition
	In order to keep my job
	As a cashier
	I want to allow customers to update their orders

@domain
Scenario: Add an item to the order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	When I add a large latte, skim milk, double shot
	Then the updated order has two items
	And the updated order includes a small latte, whole milk
	And the updated order includes a large latte, skim milk, double shot
	And the updated order total is $15.20
	And nothing else happens

@domain
Scenario: Update a cancelled order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	And I have cancelled the order
	When I add a large latte, skim milk, double shot
	Then the aggregate state is invalid 
	And the error is "You can't add an item. This order is already cancelled. Place a new order."

@domain
Scenario: Update a paid order
	Given the manager has set up the menu
	And I have placed an order for a small latte, whole milk
	And I have paid for the order
	When I add a large latte, skim milk, double shot
	Then the aggregate state is invalid
	And the error is "You can't change this order. It's already paid. Place a new order."

