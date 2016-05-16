(function () {
    $('#homePageMenu .menuArea .addMaterialPage').click(function (event) {
        event.preventDefault();

        var textTemplate = Automata.HtmlLoader.Load('/Scripts/homeScripts/MaterialPage/AddMaterialDisplay.html');

        $("#homePageMenu .displayArea").html(textTemplate);
        $('#fileinfo').submit(function (event) {
            event.preventDefault();
            var formData = new FormData();
            var fileInput = document.getElementById('fileInput');
            for (var i = 0; i != fileInput.files.length; i++) {
                formData.append(fileInput.files[i].name, fileInput.files[i]);
            }
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/Material/Add');
            xhr.send(formData);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    alert(xhr.responseText);
                }
            }
        });
    });
})();