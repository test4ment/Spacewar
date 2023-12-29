Feature: Углы

Scenario: Углы можно складывать и сравнивать
    Given Угол (15) и угол (30)
    When складывать
    Then получится угол (45)

Scenario: Угол сравнивается с null
    Given Угол (0) и null
    When сравнивать
    Then результат ложь

Scenario: Вектор складывается с null
    Given Угол (0) и null
    When складывать с null
    Then появляется ошибка