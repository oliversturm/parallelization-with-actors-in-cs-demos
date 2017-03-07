var markdownpdf = require("markdown-pdf");
var fs = require("fs");
var split = require("split");
var through = require("through");
var duplexer = require("duplexer");

function preprocessMd() {
    var splitter = split();
    var replacer = through(function(d) {
	this.queue(
	    d.replace(/^\^$/, "<pagebreak>").
		replace(/^---$/, "<pagebreak>") + "\n");
    });
    splitter.pipe(replacer);
    return duplexer(splitter, replacer);
}

fs.createReadStream("content.md").
    pipe(markdownpdf({
	paperOrientation: "landscape",
	cssPath: "pdf.css",
	remarkable: {
	    html: true,
	    breaks: true,
	    typographer: true
	},
	preProcessMd: preprocessMd
    })).
    pipe(fs.createWriteStream("slidecontent.pdf"));

