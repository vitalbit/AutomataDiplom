(function () {
    $('#homePageMenu .menuArea .testsPage').click(function (event) {
        event.preventDefault();

        var textTemplate = Automata.HtmlLoader.Load('/Scripts/homeScripts/TestsPage/TestInfoDisplay.html');

        var updateTestArea = function (msg, areaSelector) {
            var area = $(areaSelector);
            area.html('');
            for (var i = 0; i != msg.tests.length; i++) {
                var template = $(textTemplate);
                var item = msg.tests[i];
                template.find('input[type="hidden"]').val(item.TestId);
                template.find('input[type="submit"]').val(item.TestName);
                area.append(template);
            }
            return msg.tests.length;
        };

        var search = new SearchNavigation('#homePageMenu .displayArea', '/Test/GetTests', "/Test/SearchTests", updateTestArea);
    });
})();