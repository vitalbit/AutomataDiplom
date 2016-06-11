(function () {
    $('#homePageMenu .menuArea .materialsPage').click(function (event) {
        event.preventDefault();

        var updateMaterialArea = function (msg, areaSelector) {
            var area = $(areaSelector);
            area.html('');
            for (var i = 0; i != msg.materials.length; i++) {
                var item = msg.materials[i];
                area.append('<a href="/Material/GetMaterial?materialId=' + item.Id + '">' + item.FileName + '</a><br/>');
            }
            return msg.materials.length;
        };
        var search = new SearchNavigation("#homePageMenu .displayArea", '/Material/GetMaterials', "/Material/SearchMaterial", updateMaterialArea);
    });
})();