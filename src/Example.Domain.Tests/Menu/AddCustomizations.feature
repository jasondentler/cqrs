Feature: Add customizations
	In order to appease the spoiled brats who frequent coffee shops
	As a Restbucks franchise owner
	I want to setup customizations on menu items

@domain
Scenario: Add a customization
	Given I have add coffee to the menu
	When I add drink size customizations to coffee
	Then drink size customizations are added to coffee
	And nothing else happens

