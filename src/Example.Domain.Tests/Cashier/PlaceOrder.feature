Feature: Place an order
	In order to keep my job
	As a cashier
	I want to take orders from customers and give them their total

@domain
Scenario: Place an order
	Given the manager has set up the menu
	When a customer places a take-away order for one small latte, whole milk
	Then a take-away order is placed for one small latte, whole milk
	And the price is $7.60
	And nothing else happens
