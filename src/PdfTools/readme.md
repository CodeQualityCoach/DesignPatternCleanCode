# Readme

https://www.mclibre.org/descargar/docs/revistas/magpi-books/the-magpi-essentials-scratch-01-en-201606.pdf

https://www.securipi.co.uk/pibrella.pdf

# Actions

    .\PdfTools.exe archive https://www.mclibre.org/descargar/docs/revistas/magpi-books/the-magpi-essentials-scratch-01-en-201606.pdf doc2.pdf

    .\PdfTools.exe archive http://fortoffee.org.uk/wp-content/uploads/2014/07/Worksheet-PiBrella.pdf doc1.pdf

    .\PdfTools.exe combine doc.pdf .\doc1.pdf .\doc2.pdf


### Command: **addcode**
usage: `pdftools addcode <input> <text> [output]`

Adds a QR Code with a <text> to the <input> pdf. If [output] is given, the pdf is stored as [output], otherwise it overrides <input>.

### Command: **archive**
usage: `pdftools archive <url> <output>`

Downloads a pdf file from an <url> and adds the url as a barcode on the first page. The output pdf is stored as <output>

### Command: **mergescan**
usage: `pdftools mergescan <output> <input 1> <input 2>`

Merges a two paged single side flipped scan. Order of pdf1 is '1, 3, 5, 7' and pdf2 is '8, 6, 4, 2'

### Command: **reverse**
usage: `pdftools reverse <output> <input 1>

Reverses one input files <input 1> into a pdf <output>

### Command: **shuffle**
usage: `pdftools shuffle <output> <input 1> <input 2> [input 3..n]`

Shuffles two or more input files <input n> into a single pdf <output>

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

