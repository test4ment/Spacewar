Feature: Macrocommands

Scenario: Длительной операцией может быть любой объект реализующий ICommand
    Given Инициализирован IoC
    And Сгенерированы разные команды
    When Команды кладутся и исполняются в очереди
    Then В объекте оказываются все сгенерированные команды