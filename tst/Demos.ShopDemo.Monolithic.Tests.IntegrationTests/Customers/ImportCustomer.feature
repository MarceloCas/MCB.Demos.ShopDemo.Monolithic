Feature: ImportCustomer

As a e-commerce system administrator user
I want import customers
So that customers can be used in e-commerce

@customers
Scenario: New client
	Given a customer who does not have the registered email
	When the customer import is requested
	Then the client should be successfully imported
