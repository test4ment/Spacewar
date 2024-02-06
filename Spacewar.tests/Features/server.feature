Feature: Server

Scenario: Soft-остановка сервера
    Given Инициализирован IoC
    And 
    When 
    Then 

Scenario: Hard-остановка сервера
    Given Инициализирован IoC
    And Создана очередь
    And Запущен сервер
    And Добавлена пустая команда
    And 
    When 
    Then 
