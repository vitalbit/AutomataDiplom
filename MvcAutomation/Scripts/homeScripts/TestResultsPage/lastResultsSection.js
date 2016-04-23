(function () {
    $('#homePageMenu .menuArea .lastResultsPage').click(function (event) {
        event.preventDefault();

        var textTemplate = Automata.HtmlLoader.Load('/Scripts/homeScripts/TestResultsPage/TestResultsDisplay.html');

        var updateResultsArea = function (msg, areaSelector) {
            var area = $(areaSelector);
            area.html('');
            for (var i = 0; i != msg.results.length; i++) {
                var template = $(textTemplate);
                var item = msg.results[i];
                template.find('span').text(item.TestName);

                var mainDivs = template.children('div');

                var divs = mainDivs[0].children;
                divs[0].textContent = item.FirstName + ' ' + item.LastName;
                divs[1].textContent = divs[1].textContent + item.Course;
                divs[2].textContent = divs[2].textContent + item.Group;

                if (item.Mark < 4) {
                    $(mainDivs[1]).css('color', 'red');
                }
                else {
                    $(mainDivs[1]).css('color', 'green');
                }
                mainDivs[1].textContent = item.Mark;
                $(mainDivs[2]).children('a').attr('href', '/Test/GetAnswerFile?answerId=' + item.AnswerId);

                area.append(template);
            }
        };

        var search = new SearchNavigation("#homePageMenu .displayArea", '/Test/GetAnswers', "/Test/SearchAnswer", updateResultsArea);
    });
})();