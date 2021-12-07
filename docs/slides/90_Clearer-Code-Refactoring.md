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

## Refactoring PDFTools to Patterns

###### Thomas Ley | @CleanCodeCoach

---
<!-- _footer: "" -->
<!-- _paginate: "" -->
# Goals

* Refactoring of PdfTools project
* Branch [Cleaner-Code](https://github.com/CodeQualityCoach/DesignPatternCleanCode/tree/Cleaner-Code) with refactorings
* Each refactoring has a dedicated commit
* Each commit has a descriptive commit message
* Each commit has one or more descriptive slides
* The focus lies on the pattern, and keeps irrelevant parts crappy

---
# Implement [Command Pattern]

The goal of this refactoring is moving the old "if-else-statements" from `Program.cs` into dedicated classes.
Each class has only one responsibility [SRP] to provide the functionality for a dedicated action (command).

---
# Implement [Command Pattern]

* [GitHub Commit](https://github.com/CodeQualityCoach/DesignPatternCleanCode/commit/17870e7c6d83bb038af50411bf2acd9b70110253)
* Add `ICommand` interface as abstraction
* Move "if-else" code to command implementation per action
* Register available commands with their action name and make code more generic

---
# Enhanced command pattern

The goal of this enhancement is using the power of reflection and C# option to make the code more flexible and extensible in a generic way.

Attributes are used to create the available commands dictionary based on all implemented commands.

---
# Enhanced command pattern

* [GitHub Commit](https://github.com/CodeQualityCoach/DesignPatternCleanCode/commit/6be46fce70617798ac8e7b9eded88ff2591d13c9)
* Add `CommandNameAttribute` to each command implementation
* Implement `CommandHelper` to get a list of all commands and their action/command name
* Enhance `ICommand`  to provide a usage help text
* Create `HelpCommand` to print user help
* Replace exceptions with help messages in `Progam.cs`

---
# [Zero Impact Injection] QR code
The goal of this refactoring is extracting the QR code creation code to dedicated class and use this class instead of copy-paste code in each class.

In addition, the extracted class gets an interface to be more flexible according the implementation and testability.

---
# [Zero Impact Injection] QR code

* [GitHub Commit](https://github.com/CodeQualityCoach/DesignPatternCleanCode/commit/9c062c5bec4fa5a8c952e2a59aa92db0e1e4001e)
* Identify similar code
* Move code to method (for QR code already done)
* Move method to class
* Extract interface
* Inject class through interface into using class
* Use default implementation to avoid breakting changes (no command changed!)

---
# Unit Tests

The goal of this refactoring is implementing basic testing. It will create a test project and create a sample test.

The test project uses NUnit and FluentAssertions (library to make test assertions more readable). 

Basically there are two strategies with advantages and disad

---
# Unit Tests

* [GitHub Commit](https://github.com/CodeQualityCoach/DesignPatternCleanCode/commit/1c9c2442b74102660ffe21de65f05e583b820d41)
* Create dedicated test project
* Add sample test for previously created QR code generator: `QrCoderServiceTest`
* I like `Be_Creatable()` test to check constrcutor and constrains like interfaces
* Test happy path for `CreateOverlayImage(string)`
* Test error path for `CreateOverlayImage(null)`
* A null text returns an empty QR code

---
# Separate PDF Handler

The goal of this refactoring is separating concerns more clearly and handle download in a single class instead of copy-paste code in both classes.

The separation leads to a single class with separated methods.

---
# Separate PDF Handler

* [GitHub Commit](https://github.com/CodeQualityCoach/DesignPatternCleanCode/commit/1aa332cb44160f6a7f95028a429c824444f55198)
* Create a `PdfHandler` class
* Separate the concerns of `PdfArchiver` and `PdfCodeEnhancer` methods
* Make smaller but generic methods to handle the request
* Refactor the commands to use the new class.
* Review Code and check for additional places to change

---
# Cleanup Principles

The following clean code principles help to make the code better, if every team member acts this was.

* __Broken Windows Principle__: When you start adding crappy code, time by thime, everyone will follow and create crappy code, too.
* __Boyscout Rule__: Leave a place in a better shape than you found it. Rename variables to match the purpose better, refactor small code parts etc.
* __30 Second Rule__: If it only takes 30 seconds, do it now and do not put it onto the bench.

---
# Cleanup Principles

* [GitHub Commit](https://github.com/CodeQualityCoach/DesignPatternCleanCode/commit/e0d39d45f54991e628f4dffa6cd3674c481d066e)
* Refactor variable names to get rid of the legacy names
* Identify crappy code and simply use a previously refactored class
* Clean namespaces regularily and remove unused variables

---
# [Static Class Wrapper]

The goal of this refactoring is the testability of the pdf handler regarding regarding the http download.

An interface and wrapper is created for HttpClient so it can be injected through an interface. In this example, all `Http*` return values are abstracted and wrapped with an interface and only the used methds are added.

After that, "Zero Impact Injection" is used to have non breaking changes and default behaviour. `Commands` have not changed.

---
# [Static Class Wrapper]

* [GitHub Commit]()

* Add interface `IHttpClient` and inject interface to class
* Use interface in class (so we can get the used methods easily)
* Create a wrapper `HttpClientWrapper` and implement interface
* Create the client instance in constructor
* Add a "static class wrapper" for `HttpResponseMessage` and `HttpContent`
* In opposite to `HttpClientWrapper`, provide the wrapped instance as constructor parameter
* [YAGNI]. Only implement used methods and properties

---
# Future Refactorings

* Where Do I put a factory? Pdf Downloader & Pdf QrCode Adder & Pdf Merger

* Extract the "download" to a IDocumentDownloader
* Extract HttpClient
* System.Io.Abstractoins
* Refactor commands to inject dependencies and use a service locator to create the object graph
* Decorate the downloader with a QR code overlay adder

* Facade in HttpClient wrapper so we can get rid of two interfaces.
