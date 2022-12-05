Feature: ImportCustomer

As a e-commerce system administrator user
I want import customers
So that customers can be used in e-commerce

@customers
Scenario: New client
	Given a customer who does not have the registered email
	When the customer import is requested
	Then the client should be successfully imported

@customers
Scenario: Existing client
	Given  a customer who does have the registered email
	When the customer import is requested
	Then the client should not be successfully imported
	And an customer email message already registered must be returned

#Cenário 2: Cliente existente
#Dado um cliente que já possua o e-mail cadastrado na base
#Quando a importação for solicitada
#Então a importação deve ser recusada
#E um aviso de e-mail já cadastrado deve ser retornado