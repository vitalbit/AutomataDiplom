(function () {
    $('#homePageMenu .menuArea .control-test-page').click(function (event) {
        event.preventDefault();

        var textTemplate = Automata.HtmlLoader.Load('/Scripts/homeScripts/TestsPage/TestViewEditDisplay.html');

        var updateTestArea = function (msg, areaSelector) {
            var area = $(areaSelector);
            area.html('');
            for (var i = 0; i != msg.tests.length; i++) {
                var template = $(textTemplate);
                var item = msg.tests[i];
                template.find('label').text(item.TestName);
                template.find('input[type="hidden"]').val(item.TestId);
                area.append(template);
            }
        }

        var search = new SearchNavigation('#homePageMenu .displayArea', '/Test/GetTests', 'Test/SearchTests', updateTestArea);
    });
})();