(function () {
    $('#homePageMenu .menuArea .testsPage').click(function (event) {
        event.preventDefault();
        var start = 0;
        var testLength = 0;
        var updateTestArea = function (msg) {
            var area = $('#homePageMenu .displayArea .testArea');
            area.html('');
            for (var i = 0; i != msg.tests.length; i++) {
                var item = msg.tests[i];
                area.append('<form method="GET" action="/Test/Index"><input type="hidden" name="testId" value="' + item.TestId +'"/><input type="submit" name="testName" value="' + item.TestName + '"/><br/><br/></form>');
            }
        };
        var NextResults = function () {
            $.ajax({
                method: 'POST',
                url: '/Test/GetTests',
                data: { start: start }
            })
            .done(function (msg) {
                updateTestArea(msg);
            });
        };

        $('#homePageMenu .displayArea').html('<input type="text" placeholder="search"/><input class="searchButton" type="submit" value="Search"/><div class="testArea"></div><input class="prevButton" type="submit" value="Prev" disabled/><input class="nextButton" type="submit" value="Next"/>');
        $("#homePageMenu .displayArea .nextButton").click(function () {
            $("#homePageMenu .displayArea .prevButton").removeAttr('disabled');
            NextResults();
            if (testLength < 10) {
                $("#homePageMenu .displayArea .nextButton").attr('disabled', 'disable');
            }
            start += 10;
        });

        $("#homePageMenu .displayArea .prevButton").click(function () {
            $("#homePageMenu .displayArea .nextButton").removeAttr('disabled');
            start -= 10;
            if (start == 0) {
                $("#homePageMenu .displayArea .prevButton").attr('disabled', 'disabled');
            }
            NextResults();
        });

        var searchButton = $('#homePageMenu .displayArea .searchButton');
        searchButton.click(function () {
            $.ajax({
                method: "POST",
                url: "/Material/SearchTests",
                data: { search: searchButton.prev().val() }
            })
            .done(function (msg) {
                updateTestArea(msg);
            });
        });

        NextResults();
        if (testLength < 10) {
            $("#homePageMenu .displayArea .nextButton").attr('disabled', 'disable');
        }
    })
})();