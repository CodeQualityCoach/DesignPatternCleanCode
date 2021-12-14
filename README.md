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

# Combin'en der Folien

## Erstellen eins PDF Dokuments

Sind erst einmal alle PDFs erzeugt, können diese mit den PDF Tools zu einem großen PDF zusammengefügt werden.

__Wichtig:__ Es müssen die `PDFTools.exe` aus dem `Cleaner-Code` branch verwendet werden, da dieser Wildcards unterstützt.

    src\PdfTools\bin\Debug\PdfTools.exe combine aa.pdf .\docs\build\*.pdf

## Hilfe

Eine Hilfe findet sich hier:

### Command: **addcode**
usage: `pdftools addcode <input> <text> [output]`

Adds a QR Code with a <text> to the <input> pdf. If [output] is given, the pdf is stored as [output], otherwise it overrides <input>.

### Command: **archive**
usage: `pdftools archive <url> <output>`

Downloads a pdf file from an <url> and adds the url as a barcode on the first page. The output pdf is stored as <output>

### Command: **combine**
usage: `pdftools combine <output> <input 1> <input 2> [input 3..n]`

Combines two or more input files <input n> into a single pdf <output>

### Command: **download**
usage: `pdftools download <url> <output>`

Downloads a pdf file <url> from the web and stores it locally as <output>.

### Command: **help**
usage: `pdftools help`

Shows a list of all commands and their usage.

### Command: **create**
usage: `pdftools create <markdown> <output>`

Converts an input file <markdown> in markdown format into a pdf and stores it as  <output>.

