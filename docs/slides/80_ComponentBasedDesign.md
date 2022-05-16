---
title: Design Pattern & Clean Code
description: Component Based Design
footer: Design Pattern & Clean Code | Thomas Ley | @CleanCodeCoach
paginate: true
marp: true
size: 4K
---

<!-- _footer: "" -->
<!-- _paginate: "" -->
# Design Pattern & Clean Code

## component-based development

###### Thomas Ley | @CleanCodeCoach
---


## Sweets Randomizer

![width:400px bg right](assets/80_SweetsRandomizer.jpg)

- Wheel of fortune
- Video projection/screen
- One-Button user interface
- Shelves for prices

---

## Adjustable Furniture 

![width:400px bg right](assets/80_SweetsRandomizer.jpg)

- Box with hardware (main component)
- Projection board (optional)
- Shelves (optional)

---

## Alternative Setups

![width:400px bg right](assets/80_SrAlternative.jpg)

- Main box with beamer
- Main box+shelves with screen
- Main box with shelves (delocated)
- Shelves with another "game"

---

## Independent Component: Shelves

![width:400px bg right](assets/80_SrLeds.jpg)

- USB powered
- Wifi communication
- (open) REST API
- Shelves "as a service" 

---

## Component-based development

- Independent components
- Dependencies are not mandatory
- (default) Interfacecs

---

## the whole thing...

- ... is more than the sum of its parts (Aristoteles)
- Microservices approach
- Small functionality which "just works"
- Business logic just connects functionalities

---

## Design Pattern/Principles

- SRP
- Strategy Pattern
- Null-Object Pattern
- Dependency Injection

---

