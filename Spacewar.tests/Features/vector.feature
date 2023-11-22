Feature: Векторы

Scenario: Векторы можно складывать и сравнивать
    Given Вектор (1, 2) и вектор (3, 4)
    When складывать
    Then получится вектор (4, 6)

Scenario: Векторы могут быть не равны между собой
    Given Вектор (0, 1) и вектор (1, 0)
    When сравнивать
    Then результат ложь

Scenario: Нельзя сравнивать векторы разной размерности
    Given Вектор (0) и вектор (1, 2)
    When складывать
    Then появляется ошибка
    