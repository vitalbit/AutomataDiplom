(function () {
    $('#homePageMenu .menuArea .materialsPage').click(function (event) {
        event.preventDefault();
        var start = 0;
        var materialLength = 0;
        var updateMaterialArea = function (msg) {
            var area = $('#homePageMenu .displayArea .materialArea');
            area.html('');
            for (var i = 0; i != msg.materials.length; i++) {
                var item = msg.materials[i];
                area.append('<a href="/Material/GetMaterial?materialId=' + item.Id + '">' + item.FileName + '</a><br/>');
            }
        };
        var NextResults = function () {
            $.ajax({
                method: 'POST',
                url: '/Material/GetMaterials',
                data: { start: start }
            })
            .done(function (msg) {
                updateMaterialArea(msg);
            });
        };

        $("#homePageMenu .displayArea").html('<input type="text" placeholder="search"/><input class="searchButton" type="submit" value="Search"/><div class="materialArea"></div><input class="prevButton" type="submit" value="Prev" disabled/><input class="nextButton" type="submit" value="Next"/>');
        $("#homePageMenu .displayArea .nextButton").click(function () {
            $("#homePageMenu .displayArea .prevButton").removeAttr('disabled');
            NextResults();
            if (materialLength < 10) {
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
                url: "/Material/SearchMaterial",
                data: { search: searchButton.prev().val() }
            })
            .done(function (msg) {
                updateMaterialArea(msg);
            });
        });

        NextResults();
        if (materialLength < 10) {
            $("#homePageMenu .displayArea .nextButton").attr('disabled', 'disable');
        }
    })
})();