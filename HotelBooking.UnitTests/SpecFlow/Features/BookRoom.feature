Feature: BookRoom
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Create BookingBB
	Given the startDate is tomorrow-3
	And the endDate is tomorrow-2
	When the dates are check
	Then the result should be true

Scenario: Create BookingAA
	Given the startDate is tomorrow+4
	And the endDate is tomorrow+5
	When the dates are check
	Then the result should be true

Scenario: Create BookingBA
	Given the startDate is tomorrow-3
	And the endDate is tomorrow+5
	When the dates are check
	Then the result should be true

Scenario: Create BookingBO
	Given the startDate is tomorrow-3
	And the endDate is tomorrow+2
	When the dates are check
	Then the result should be false


