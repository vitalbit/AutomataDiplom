(function () {
    var sendFile = function (elementId, path, runAfter) {
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
                runAfter();
                alert(xhr.responseText);
            }
        }
    }

    var updateTestTypes = function () {
        var typeSelect = $("#type-select");

        $.ajax({
            method: "GET",
            url: "/TestType/GetTypes"
        })
        .done(function (msg) {
            typeSelect.html('');
            for (var i = 0; i != msg.testTypes.length; i++) {
                typeSelect.append('<option value="' + msg.testTypes[i].Id + '">' + msg.testTypes[i].ModuleName + '</option>');
            }
        });
    }

    var updateFileList = function () {
        var fileSelect = $("#file-list");

        $.ajax({
            method: "GET",
            url: "/TestType/GetFiles"
        })
        .done(function (msg) {
            fileSelect.html('');
            for (var i = 0; i != msg.testFiles.length; i++) {
                fileSelect.append('<option value="' + msg.testFiles[i].Id + '">' + msg.testFiles[i].FileName + '</option>');
            }
        });
    }

    $("#js-input .add-js-file").click(function (event) {
        event.preventDefault();

        $("#js-input .js-file").click();
    });

    $("#css-input .add-css-file").click(function (event) {
        event.preventDefault();

        $("#css-input .css-file").click();
    });

    $("#json-input .add-json-file").click(function (event) {
        event.preventDefault();

        $("#json-input .json-file").click();
    });

    $("#js-input").submit(function (event) {
        event.preventDefault();

        sendFile('js-file', '/TestType/AddJsFile');
    });

    $("#css-input").submit(function (event) {
        event.preventDefault();

        sendFile('css-file', '/TestType/AddCssFile');
    });

    $("#json-input").submit(function (event) {
        event.preventDefault();

        sendFile('json-file', '/TestType/AddFile', updateFileList);
    });

    $("#js-input .js-file").change(function () {
        var fileName = $(this).val().replace("C:\\fakepath\\", "");
        $(".edit-test-page .js-filename").val(fileName);
        $("#js-input").submit();
    });

    $("#css-input .css-file").change(function () {
        var fileName = $(this).val().replace("C:\\fakepath\\", "");
        $(".edit-test-page .css-filename").val(fileName);
        $("#css-input").submit();
    });

    $("#json-input .json-file").change(function () {
        var fileName = $(this).val().replace("C:\\fakepath\\", "");
        $(".json-filename").val(fileName);
        $("#json-input").submit();
    })

    $("#add-type").click(function (event) {
        event.preventDefault();

        $.ajax({
            method: "POST",
            url: "/TestType/CreateType",
            data: { testType: $('#testType').val(), jsFile: $(".js-filename").val(), cssFile: $(".css-filename").val() }
        })
        .done(function (msg) {
            updateTestTypes();
            alert(msg.message);
        });
    });

    $("#type-select").change(function () {
        $.ajax({
            method: "GET",
            url: "/TestType/GetType",
            data: { id: this.value }
        })
        .done(function (msg) {
            $(".js-filename").val(msg.testType.JsFileName);
            $(".css-filename").val(msg.testType.CssFileName);
            $("#testType").val(msg.testType.ModuleName);
        });
    });

    $("#test-create").click(function (event) {
        event.preventDefault();
        $.ajax({
            method: "POST",
            url: "/Test/Create",
            data: { typeId: $('#type-select').val(), fileId: $('#file-list').val(), testName: $('#test-name').val() }
        })
        .done(function (msg) {
            alert(msg.message);
        });
    })

    updateTestTypes();
    updateFileList();
})();