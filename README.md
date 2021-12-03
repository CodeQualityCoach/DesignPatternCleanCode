# Design Pattern & Clean Code

## Design Pattern & Clean Code – ein pragmatischer Ansatz

__GOF, DI, IOC, ISP; Singleton, Factory, Lazy; *Unit, Substitute, Testing Helper__ ... so oder so ähnlich könnte ein Bullshit Bingo für Clean Code aussehen. Aber was bedeuten die Abkürzungen, womit fange ich an und wie kann ich Clean Code im Projekt zielgerichtet umsetzen?

In diesem Workshop werden wir pragmatisch und nach dem Pareto Prinzip vorgehen. Wir werden uns Entwurfsmuster und Implementierungsmuster ansehen, und uns anschauen, wie wir diese im Projekt umsetzen können, ohne alles neu zu bauen oder bei neuen Projekten das Ziel aus den Augen zu verlieren. Als einen wichtigen Bestandteil werden wir uns IoC Container näher anschauen, aber auch kleine Lösungen ohne IoC betrachten.

Die folgenden Fragen werden uns in dem Workshop beschäftigen:

* Wie fange ich an und was bedeutet „Kaizen“ eigentlich?
* Wie mache ich „brown-field“ Projekte ein bisschen besser?
* Wie verbessere ich die Testbarkeit von Projekten?
* Was sind die Tücken von Dependency Injection?

Am Ende des Tages haben wir einen allround Werkzeugkoffer, mit dem wir für die wichtigsten Aufgaben gewappnet sind.


# Marp'en der Folien

Startet auf dem PC einnen Service auf Port [8080](http://localhost:8080) und erstellt die Folien on-demand bzw. bei Änderung.

    marp -p -s --input-dir .\slides\

Hiermit können alle Slides erzeugt werden. Output ist entsprechend der `build` Ordner.

    marp --input-dir .\slides\
    marp --input-dir .\slides\ --pdf
    
`.marprc.yml` enthält die Konfiguration für Theme, Output etc.

Die gerenderten Slides befinden sich `Slides`

# Die Folien

* `RF` => Refactoring Pattern
* `CC` => Clean Code Principle
* `DP` => Design Pattern
* `BP` => Best Practice

