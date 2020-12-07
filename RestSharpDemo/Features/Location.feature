Feature: Location
	Test the location functionality

Background: 
	Given I get JWT authentication of User with following details
		| Email             | Password |
		| karthik@email.com | haha123  |
@smoke
Scenario: Get the first Location and verify its city
	Given I perform GET operation for "location?id={id}"
	And I perform operation for location as "1"
	Then I should see the "city" name as "chennai" in response


@smoke
Scenario: Create new Location and verify its Address details
	Given I perform POST operation to create new location with following details
		| city     | country | street    | flat no | pincode | type    |
		| Auckland | NZ      | 11th grey | 121A    | 0629    | primary |
	And I perform PUT operation to update the address details
		| city     | country | street        | flat no | pincode | type    |
		| Auckland | NZ      | 12th New Lynn | 121A     | 0629    | primary |
	Then I should see the "address" name as "12th New Lynn" for address

@smoke
Scenario: Create new Location and verify its Address details and delete
	Given I perform POST operation to create new location with following details
		| city     | country | street    | flat no | pincode | type    |
		| Auckland | NZ      | 11th grey | 121A     | 0629    | primary |
	And I perform PUT operation to update the address details
		| city     | country | street        | flat no | pincode | type    |
		| Auckland | NZ      | 12th New Lynn | 121A     | 0629    | primary |
	Then I should see the "address" name as "12th New Lynn" for address
	And I perform DELETE operation of the newly created address
