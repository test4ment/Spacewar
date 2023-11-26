Feature: Continious operation

Scenario: Объект может длительно выполнять некоторую операцию
    Given Инициализирован IoC 
    And Объект и приказ для него
    When Executed
    Then В очередь приходят ICommand