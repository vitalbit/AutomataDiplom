if (typeof (Automata) == 'undefined') {
    Automata = {};
}

Automata.HtmlLoader = {
    Load: function (htmlPath) {
        var template = null;
        var xhr = new XMLHttpRequest();
        xhr.open('GET', htmlPath, false);
        xhr.onreadystatechange = function () {
            if (this.readyState !== 4) {
                return;
            }
            template = this.responseText;
        }
        xhr.send();
        return template;
    }
}