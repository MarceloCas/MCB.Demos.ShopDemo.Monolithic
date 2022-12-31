Feature: ImportCustomer

As a e-commerce system administrator user
I want import customers
So that customers can be used in e-commerce

@customers
Scenario: New client
	Given a customer who does not have the registered email
	When the customer import is requested
	Then the client should be successfully imported

#@customers
#Scenario: Existing client
#	Given  a customer who does have the registered email
#	When the customer import is requested
#	Then the client should not be successfully imported
#	And a customer email already registered message must be returned
#
#@customers
#Scenario: Client with empty informations
#	Given  a customer who does have empty informations
#	When the customer import is requested
#	Then the client should not be successfully imported
#	And a should have tenantId message must be returned
#	And a should have executionUser message must be returned
#	And a should have sourcePlatform message must be returned
#	And a should have firstName message must be returned
#	And a should have lastName message must be returned
#	And a should have birthDate message must be returned
