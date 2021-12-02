---
title: Design Pattern & Clean Code
tags: designpattern, cleancode, cleancodecoach
---

# Das haben wir besprochen

## pt.1

* Wie alt bist du: C64 Basic, QBasic, Turbo Pascal, VB2, Delphi, VB6, .NET 1.1, .NET 2.0, .NET Core
* Beschäftigt euch 20% mit einem Thema und ihr lernt 80%
* Dependencies vs. Abstractions
* ISP - Interface segregation principle
* SRP - Single Responsibility Principle
* Zero Impact Injection (Refactor without breaking code)

## pt.2

* Dependency Injection vs. Service Locator
* Dependency Injection Framework
    * Lifetime Scopes (Transient, Singleton)
    * Container Setup and register interface-class mapping
* Singleton
    * Global galaktisch
    * Per "context" e.g. HttpRequest
    * Singleton per "Tree resolve" vs. "AlwaysNew"
* Strategy Pattern

## pt.3

* Strategy Resolver
* Factory Pattern (Method, Class)
* Unit Testing
    * Cover happy path and all error path
    * One path per test
* "Static Class Wrapper" Pattern
    * `HttpContext`
    * `Random`
    * `Console.WriteLine`/`Console.ReadLine`

## pt.4

* Tests bring clean code and design pattern together
* Architectural "Unit Tests"
* Dependencies can be mocked
* `ContextFor<>` to mock all dependencies
    * Detail See [Blog](https://codequalitycoach.de/posts/ContextFor-for-testing/)
    * Gist is [here](https://gist.github.com/ThomasLey/b8246fc93e0b747476f7b18ce303ae0f#file-contextfor-cs)

## Anhang

* Bibliothek stellt ein Interface bereit und Adapter in Programm
* Action<T> als gemeinsame Abstraktion