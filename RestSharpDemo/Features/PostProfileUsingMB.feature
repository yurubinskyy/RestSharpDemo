Feature: PostProfileUsingMB
  Test POST operation using REST-assured library


  Background: 
	Given I create service virutalization for profile
        | id | name | profile |
        | 1  | Sams | 2       |   


  @smoke
  Scenario: Verify Post operation for Profile
    Given I Perform POST operation for "/posts/{profileNo}/profile" with body
      | name | profile |
      | Sams | 2       |
	Then I should see the "name" name as "Sams"