Feature: BookRoom
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario Outline:
	Given the startDate which is tomorrow plus 0
	And the endDate which is tomorrow plus 1
	When the dates are check with the occupied range
	Then the result should be returned

Examples: 
| startDate | endDate | Availability |
| 0			|	+1    | Yes          |

