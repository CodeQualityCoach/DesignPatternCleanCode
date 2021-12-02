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

## What are Dependencies?

###### Thomas Ley | @CleanCodeCoach
---

# Indirection

    "All problems in computer science can be solved by another level of indirection" 

##### __Butler Lampson__
##### in _"fundamental theorem of software engineering"_

---
# Dependencies

* Class (internal, hard)
* Interface (internal, soft)
* Libraries (external, hard/soft).

---
# Class

:point_up: __New is Glue__

* "Hard dependency"
* Knows implementation
* Cannot be replaced 
* `new()` as indicator.

---
# Interface

* "Soft dependency"
* Hides implementation
* Can be replaced
* Subset of functionality [ISP].

---
# Libraries

* "Hard dependency" or "Soft dependency"
* Knows or hides implementation
* Can partially be replaced
* Depending from 3rd party.

---
# Independence is Freedom

* Reduce (or remove) hard dependencies
* Use abstractions (e.g. Interfaces, Lambda)
* Watch your dependencies
* Define your dependencies.

---
# Dependencies can be found

* nuget/references
* namespace usings
* constructor
* `new()`
* properties.

---
# Favour Composition

* Favour composition over inheritance
* Legacy Clean Code principle
* Multi-Inheritance (C++)
* A class using inheriance to "get functions".

---
# Have a break...

# ... have a workout

[7 Minute Workout](https://www.youtube.com/watch?v=mmq5zZfmIws) or [Bring Sally Up](https://www.youtube.com/watch?v=41N6bKO-NVI)
