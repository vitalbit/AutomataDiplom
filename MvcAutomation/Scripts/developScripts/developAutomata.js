(function () {
    var sendFile = function (elementId, path) {
        var formData = new FormData();
        var fileInput = document.getElementsByClassName(elementId)[0];
        for (var i = 0; i != fileInput.files.length; i++) {
            formData.append(fileInput.files[i].name, fileInput.files[i]);
        }
        var xhr = new XMLHttpRequest();
        xhr.open('POST', path);
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                $('#dev-area').text(JSON.parse(xhr.responseText).content);
            }
        }
    }

    $('.dev-file').change(function () {
        $('#dev-input').submit();
    });

    $('#dev-input').submit(function (event) {
        event.preventDefault();

        sendFile('dev-file', '/Developer/GetFile');
    });

    //$('input[type="submit"]').click(function (event) {
    //    event.preventDefault();

    //    $.ajax({
    //        method: "POST",
    //        url: "/Developer/AutomataDevelop",
    //        data: { file: $('#dev-area').text() }
    //    });
    //});
})();