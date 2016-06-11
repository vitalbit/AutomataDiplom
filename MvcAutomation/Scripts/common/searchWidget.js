var SearchNavigation = function (selector, nextUrl, searchUrl, updateResults) {
    var start = 0;
    var resultsLength = 0;

    var NextResults = function () {
        $.ajax({
            method: 'POST',
            url: nextUrl,
            data: { start: start }
        })
        .done(function (msg) {
            resultsLength = updateResults(msg, SearchNavigation.prototype.resultsArea);
            if (resultsLength < 10) {
                $(selector + " .nextButton").attr('disabled', 'disabled');
            }
        });
    };

    //initialize
    $(selector).html('<input type="text" placeholder="search" class="search"/><input class="searchButton" type="submit" value="Search"/><div class="resultsArea"></div><input class="prevButton" type="submit" value="Prev" disabled/><input class="nextButton" type="submit" value="Next"/>');
    //Change results area
    SearchNavigation.prototype.resultsArea = selector + ' .resultsArea';

    $(selector + " .nextButton").click(function () {
        $(selector + " .prevButton").removeAttr('disabled');
        NextResults();
        start += 10;
    });

    $(selector + " .prevButton").click(function () {
        $(selector + " .nextButton").removeAttr('disabled');
        start -= 10;
        if (start == 0) {
            $(selector + " .prevButton").attr('disabled', 'disabled');
        }
        NextResults();
    });

    var searchButton = $(selector + ' .searchButton');
    searchButton.click(function () {
        $.ajax({
            method: "POST",
            url: searchUrl,
            data: { search: searchButton.prev().val() }
        })
        .done(function (msg) {
            updateResults(msg, SearchNavigation.prototype.resultsArea);
        });
    });

    NextResults();
}