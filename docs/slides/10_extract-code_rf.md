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

## Zero Impact Injection

###### Thomas Ley | @CleanCodeCoach

---
<!-- _footer: "" -->
<!-- _paginate: "" -->
# Goals

* Refactor Brownfield Project
* Single Responsibility Principle
* Interface Segregation Principle
* Dependency Injection
* Zero Impact Injection
* Testable Code.


---
<!-- _footer: "" -->
<!-- _paginate: "" -->
# Steps

* Extract Code (Method :arrow_right: Class :arrow_right: Project)
* Create Single Dependency (`new()`)
* Inject Dependency 
* Null-Object Pattern.

---
# Extract code [SRP]

- Extract method
- Move to class
- Introduce field

:bulb: Namespace indicates dependencies
:bulb: Create a documentation of the class without "and"
:vs: __Demo: Barcode Code__

---
# [Zero Impact Injection]

- Add interface to class
- Inject as interface into class
- Use `??` for default implementation

:vs: __Demo: Barcode class__

---
# [Null-Object Pattern]

- Reduces null-checks
- "Identity Element" for an interface
- Empty implementation
- Returns `default`
- IFooBar --> EmptyFooBar
- Implemented along with Interface

:vs: __Demo: Empty logger class__
