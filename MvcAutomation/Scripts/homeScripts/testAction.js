(function () {
    $('#homePageMenu .menuArea .testsPage').click(function (event) {
        event.preventDefault();

        var updateTestArea = function (msg, areaSelector) {
            var area = $(areaSelector);
            area.html('');
            for (var i = 0; i != msg.tests.length; i++) {
                var item = msg.tests[i];
                area.append('<form method="GET" action="/Test/Index"><input type="hidden" name="testId" value="' + item.TestId + '"/><input type="submit" name="testName" value="' + item.TestName + '"/><br/><br/></form>');
            }
        };
        var search = new SearchNavigation('#homePageMenu .displayArea', '/Test/GetTests', "/Material/SearchTests", updateTestArea);
    });
})();