(function () {
    $('#homePageMenu .menuArea .addTestPage').click(function (event) {
        event.preventDefault();
        var updateTestTypes = function () {
            var selectArea = $('#homePageMenu .displayArea select').first();
            $.ajax({
                method: "GET",
                url: "/TestType/GetTypes"
            })
            .done(function (msg) {
                selectArea.html('');
                for (var i = 0; i != msg.testTypes.length; i++) {
                    selectArea.append('<option value="' + msg.testTypes[i].Id + '">' + msg.testTypes[i].ModuleName + '</option>');
                }
            });
        };
        var updateTestFiles = function () {
            var selectArea = $('#homePageMenu .displayArea select').last();
            $.ajax({
                method: "GET",
                url: "/TestType/GetFiles"
            })
            .done(function (msg) {
                selectArea.html('');
                for (var i = 0; i != msg.testFiles.length; i++) {
                    selectArea.append('<option value="' + msg.testFiles[i].Id + '">' + msg.testFiles[i].FileName + '</option>');
                }
            });
        };

        $("#homePageMenu .displayArea").html('Тип теста<br/><select></select><input type="text" placeholder="Test type"/><input type="submit" value="Add"/><br/><form method="post" id="fileinfo"><select></select><input id="fileInput" type="file"/><input type="submit" value="Upload test file"/></form><br/><input type="text" placeholder="TestName"/><input type="submit" value="Add"/>');
        $('#homePageMenu .displayArea input[type="submit"]').first().click(function (event) {
            event.preventDefault();
            $.ajax({
                method: "POST",
                url: "/TestType/CreateType",
                data: { testType: $('#homePageMenu .displayArea input[type="submit"]').first().prev().val() }
            })
            .done(function (msg) {
                updateTestTypes();
                alert(msg.message);
            });
        });
        $('#fileinfo').submit(function (event) {
            event.preventDefault();
            var formData = new FormData();
            var fileInput = document.getElementById('fileInput');
            for (var i = 0; i != fileInput.files.length; i++) {
                formData.append(fileInput.files[i].name, fileInput.files[i]);
            }
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/TestType/AddFile');
            xhr.send(formData);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    updateTestFiles();
                }
            }
        });
        $('#homePageMenu .displayArea input[type="submit"]').last().click(function (event) {
            event.preventDefault();
            $.ajax({
                method: "POST",
                url: "/Test/Create",
                data: { typeId: $('#homePageMenu .displayArea select').first().val(), fileId: $('#homePageMenu .displayArea select').last().val(), testName: $('#homePageMenu .displayArea input[type="text"]').last().val() }
            })
            .done(function (msg) {
                alert(msg.message);
            });
        });

        updateTestTypes();
        updateTestFiles();
    })
})();