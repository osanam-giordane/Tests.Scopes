/// <reference types="cypress" />

context('Customers', () => {
    beforeEach(() => {
        cy.visit('https://web:5000/customer')
    })

    describe('Verify customers registered', () => {
        it('Contains 5 customers in list', () => verifyList(5))
    })

    describe('Register customers', () => {
        beforeEach(() => cy.get('#create_customer').click())

        it('With a valid fields', () => {
            cy.get('#login').type('joaodasilva')
            cy.get('#password').type('12345678')
            cy.get('#name').type('João da Silva')
            cy.get('#birth_date').type('1990-02-01')
            cy.get('#email').type('joao@gmail.com')
            cy.get('#phone').type('(19) 9880-0102')

            cy.get('#create_customer_modal').click()

            verifyList(6)
        })

        it('With invalid name', () => {
            cy.get('#login').type('joaodasilva')
            cy.get('#password').type('12345678')
            cy.get('#name').type('JP')
            cy.get('#birth_date').type('1990-02-01')
            cy.get('#email').type('joao@gmail.com')
            cy.get('#phone').type('(19) 9880-0102')

            cy.get('#create_customer_modal').click()

            verifyList(6)
        })
    })
})

function verifyList(numberOfItens) {
    cy.get('.table > tbody')
        .find('tr')
        .should('have.length', numberOfItens)
}