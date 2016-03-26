(function () {
    $('#homePageMenu .menuArea .lastResultsPage').click(function (event) {
        event.preventDefault();
        var start = 0;
        var resultsLength = 0;
        var updateResultsArea = function (msg) {
            var area = $('#homePageMenu .displayArea .resultsArea');
            area.html('');
            for (var i = 0; i != msg.results.length; i++) {
                var item = msg.results[i];
                area.append(item.FirstName + ' ' + item.LastName + ' ' + item.Faculty + ' ' + item.Speciality + ' ' + item.Course + ' ' + item.Group + '<a href="/Test/GetAnswerFile?answerId=' + item.AnswerId + '">Download</a><br/>');
            }
        };
        var NextResults = function () {
            $.ajax({
                method: 'POST',
                url: '/Test/GetAnswers',
                data: { start: start }
            })
            .done(function (msg) {
                updateResultsArea(msg);
            });
        };

        $("#homePageMenu .displayArea").html('<input type="text" placeholder="search"/><input class="searchButton" type="submit" value="Search"/><div class="resultsArea"></div><input class="prevButton" type="submit" value="Prev" disabled/><input class="nextButton" type="submit" value="Next"/>');
        $("#homePageMenu .displayArea .nextButton").click(function () {
            $("#homePageMenu .displayArea .prevButton").removeAttr('disabled');
            NextResults();
            if (resultsLength < 10) {
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
                url: "/Test/SearchAnswer",
                data: { search: searchButton.prev().val() }
            })
            .done(function (msg) {
                updateResultsArea(msg);
            });
        });

        NextResults();
        if (resultsLength < 10) {
            $("#homePageMenu .displayArea .nextButton").attr('disabled', 'disable');
        }
    })
})();