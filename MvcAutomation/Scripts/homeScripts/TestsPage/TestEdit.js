﻿(function () {
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
            if ($('#testIdForEdit').length != 0) {
                var id = $('#testIdForEdit').val();
                $('#type-select').val(id).change();
            }
        });
    }

    var updateFileList = function () {
        var fileSelect = $("#file-list");

        $.ajax({
            method: "GET",
            url: "/TestType/GetFiles",
            data: { testType: $("#testType").val() }
        })
        .done(function (msg) {
            fileSelect.html('');
            for (var i = 0; i != msg.testFiles.length; i++) {
                fileSelect.append('<option value="' + msg.testFiles[i] + '">' + msg.testFiles[i] + '</option>');
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

    $("#dll-input .add-dll-file").click(function (event) {
        event.preventDefault();

        $("#dll-input .dll-file").click();
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

    $("#dll-input").submit(function (event) {
        event.preventDefault();

        sendFile('dll-file', '/TestType/AddDllFile');
    });

    $("#json-input").submit(function (event) {
        event.preventDefault();

        sendFile('json-file', '/TestType/AddFiles?testType=' + $("#testType").val(), updateFileList);
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

    $("#dll-input .dll-file").change(function () {
        var fileName = $(this).val().replace("C:\\fakepath\\", "");
        $(".edit-test-page .dll-filename").val(fileName);
        $("#dll-input").submit();
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
            data: { testType: $('#testType').val(), jsFile: $(".js-filename").val(), cssFile: $(".css-filename").val(), dllFile: $(".dll-filename").val(), resolveType: $(".dll-type-resolve").val() }
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
            $(".dll-filename").val(msg.testType.DllFileName);
            $(".dll-type-resolve").val(msg.testType.ResolveDllType);
            $("#testType").val(msg.testType.ModuleName);
            updateFileList();
        });
    });

    $("#test-create").click(function (event) {
        event.preventDefault();
        $.ajax({
            method: "POST",
            url: "/Test/Create",
            data: { typeId: $('#type-select').val(), testName: $('#test-name').val() }
        })
        .done(function (msg) {
            alert(msg.message);
        });
    })

    updateTestTypes();
})();