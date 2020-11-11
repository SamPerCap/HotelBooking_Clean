Feature: BookRoom
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario Outline:
	Given the start date which is tomorrow plus <startDate>
	And the end date which is tomorrow plus <endDate>
	When the dates are check with the occupied range
	Then the <Availability> should be returned

Examples: 
| startDate | endDate | Availability |
|	  0		|	 1    |    True      |

