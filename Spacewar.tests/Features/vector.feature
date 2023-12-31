Feature: Векторы

Scenario: Векторы можно складывать и сравнивать
    Given Вектор (1, 2) и вектор (3, 4)
    When складывать
    Then получится вектор (4, 6)

Scenario: Нельзя складывать векторы разной размерности
    Given Вектор (0) и вектор (1, 2)
    When складывать
    Then появляется ошибка
    
Scenario: Вектор складывается с null
    Given Вектор (0) и null
    When складывать
    Then появляется ошибка

Scenario: null складывается с вектор 
    Given Вектор (0) и null
    When складывать в другом порядке
    Then появляется ошибка

Scenario: Вектор сравнивается с null
    Given Вектор (0) и null
    When сравнивать
    Then результат ложь
