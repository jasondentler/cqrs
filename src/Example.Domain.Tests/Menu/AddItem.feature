Feature: Add menu items
	In order to make my millions
	As a Restbucks franchise owner
	I want to add menu items

@domain
Scenario: Add a menu item
	When I add coffee to the menu
	Then coffee is added to the menu
	And nothing else happens

