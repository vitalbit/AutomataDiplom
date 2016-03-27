(function () {
    $('#homePageMenu .menuArea .resultsPage').click(function (event) {
        event.preventDefault();

        var updateResultsArea = function (msg, areaSelector) {
            var area = $(areaSelector);
            area.html('');
            for (var i = 0; i != msg.results.length; i++) {
                var item = msg.results[i];
                area.append(item.TestName + ' ' + item.FirstName + ' ' + item.LastName + ' Course: ' + item.Course + ' Group: ' + item.Group + ' Mark: ' + item.Mark + '<br/><a href="/Test/GetAnswerFile?answerId=' + item.AnswerId + '">Download</a><br/>');
            }
        };
        var search = new SearchNavigation("#homePageMenu .displayArea", '/Test/GetUserAnswers', "/Test/SearchUserAnswer", updateResultsArea);
    });
})();