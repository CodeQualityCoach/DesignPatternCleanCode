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

## Refactor `PdfTools`

###### Thomas Ley | @CleanCodeCoach

---
# Refactoring `PdfTools`

* Written without any design
* Some pdf and qrcode features
* Download of files
* Concatenate files.

---
# Issues in `PdfTools`

* Duplicate code
* Un-testable
* No [SRP]
* Hard to extend
* Hard to reuse components.

---
# Demo "PdfTools"

---
# Challenge

* How do I clean up brownfield projects?
* Without rewriting them?
* Without breaking them?
* Where do I start?
