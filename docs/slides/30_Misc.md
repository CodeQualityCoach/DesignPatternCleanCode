---
title: Design Pattern & Clean Code
description: Design Pattern & Clean Code – ein pragmatischer Ansatz
footer: Design Pattern & Clean Code | Thomas Ley | @CleanCodeCoach
paginate: true
marp: true
size: 4K
---

<!-- _footer: "" -->
<!-- _paginate: "" -->
# Design Pattern & Clean Code

## Testable Code

###### Thomas Ley | @CleanCodeCoach

---
<!-- _footer: "" -->
<!-- _paginate: "" -->
# Goals

* Design Pattern
* Clean Code
* Best Practices

---
# Kaizen vs. Kaikaku

* `Make the world a better place, day by day`

* Kaizen is the Sino-Japanese word for "improvement"
* Kaikaku is the Japanese term for "radical change"

---
# [Boyscout Rule]

`Always leave the code better than you found it.`

* Remove technical debts
* Renaming, documentation, tests, or
* Clean(er) Code
* Small or smaller, depends on you

---
# [Broken Window Principle}

* If you start breaking it,
* Everyone will continue. 

---
# [30 second rule]

`If it takes only five minutes, do it now`

* If it can be fast, do it now
* If it is a bigger task, create a ticket
* If there are too many "five mins", create a `TODO`

---
# SOLID

* Single-responsibility principle
    * Every class should have only one responsibility
* Open–closed principle
    * Software entities ... should be open for extension, but closed for modification.
* Liskov substitution principle
    * A derived class should not break behaviour of the base class
* Interface segregation principle
    * Many client-specific interfaces are better than one general-purpose interface
* Dependency inversion principle
    * Depend upon abstractions, not concretions

---
# Extension Methods

* No state
* No business logic
* Just "facade" methods

---
# Aggregation Pattern

* IGetInformation
* IAggregateGetInformation
* IAggregateGetInformation implements IGetInformation
* IAggregateGetInformation registers only with aggregate interface

---
# Tagging Interface

* Empty interface to register a class for a specific purpose

---
# 3rd party libraries

* Create separate project
* Interfaces to "contracts" project
* Empty implementation to "contracts" project
* Concrete implementation in 3rd party library

---
# [Strategy Pattern]

- Inject behaviour
- Strategy changes depending on needs
- DI implementa strategy with registrations
- Some frameworks support "named registrations"

:bulb: Demo: StrategyResolver<T>

---
# [Command Pattern]

- Is a strategy pattern
- Has the following methods
    - `CanExecute(context)`
    - `Execute(context)`

:bulb: Demo: PdfTools
