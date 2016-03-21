(function () {
    $('#homePageMenu .menuArea .addStudentsPage').click(function (event) {
        event.preventDefault();
        var selectorUpdate = function (arr, className) {
            var selector = $('#homePageMenu .displayArea .' + className);
            selector.html('');
            for (var i = 0; i != arr.length; i++) {
                selector.append('<option value="' + arr[i].Id + '">' + arr[i].Name + '</option>');
            }
        };
        var updateFaculty = function (arr) {
            selectorUpdate(arr, 'addFacultySelect');
        };
        var updateSpeciality = function (arr) {
            selectorUpdate(arr, 'addSpecialitySelect');
        };
        var updateCourse = function (arr) {
            selectorUpdate(arr, 'addCourseSelect');
        };
        var updateGroup = function (arr) {
            selectorUpdate(arr, 'addGroupSelect');
        };
        $("#homePageMenu .displayArea").html('<span><input type="text" placeholder="Add faculty"/><select class="addFacultySelect"></select><input type="submit" value="Add"/></span><br/>' +
            '<span><input type="text" placeholder="Add speciality"/><select class="addSpecialitySelect"></select><input type="submit" value="Add"/></span><br/>' +
            '<span><input type="text" placeholder="Add course"/><select class="addCourseSelect"></select><input type="submit" value="Add"/></span><br/>' +
            '<span><input type="text" placeholder="Add group"/><select class="addGroupSelect"></select><input type="submit" value="Add"/></span><br/>' +
            '<input type="text" placeholder="Email"/><br/><input type="text" placeholder="FirstName"/><br/><input type="text" placeholder="LastName"/><br/><input type="submit" value="Add"/>');
        $.ajax({
            method: "GET",
            url: "/StructureOrganize/GetData"
        })
        .done(function (msg) {
            updateFaculty(msg.faculties);
            updateSpeciality(msg.specialities);
            updateCourse(msg.courses);
            updateGroup(msg.groups);
        });

        $("#homePageMenu .displayArea .addFacultySelect").next().click(function () {
            $.ajax({
                method: "POST",
                url: "/StructureOrganize/AddFaculty",
                data: { faculty: $("#homePageMenu .displayArea .addFacultySelect").prev().val() }
            })
            .done(function (msg) {
                updateFaculty(msg.faculties);
            });
        });
        $("#homePageMenu .displayArea .addSpecialitySelect").next().click(function () {
            $.ajax({
                method: "POST",
                url: "/StructureOrganize/AddSpeciality",
                data: { speciality: $("#homePageMenu .displayArea .addSpecialitySelect").prev().val() }
            })
            .done(function (msg) {
                updateSpeciality(msg.specialities);
            });
        });
        $("#homePageMenu .displayArea .addCourseSelect").next().click(function () {
            $.ajax({
                method: "POST",
                url: "/StructureOrganize/AddCourse",
                data: { course: $("#homePageMenu .displayArea .addCourseSelect").prev().val() }
            })
            .done(function (msg) {
                updateCourse(msg.courses);
            });
        });
        $("#homePageMenu .displayArea .addGroupSelect").next().click(function () {
            $.ajax({
                method: "POST",
                url: "/StructureOrganize/AddGroup",
                data: { group: $("#homePageMenu .displayArea .addGroupSelect").prev().val() }
            })
            .done(function (msg) {
                updateGroup(msg.groups);
            });
        });

        $('input[value="Add"]').last().click(function () {
            var inputs = $("#homePageMenu .displayArea").children('input[type="text"]');
            $.ajax({
                method: "POST",
                url: "/StructureOrganize/AddStudent",
                data: {
                    facultyId: $('#homePageMenu .displayArea .addFacultySelect').val(),
                    specialityId: $("#homePageMenu .displayArea .addSpecialitySelect").val(),
                    courseId: $("#homePageMenu .displayArea .addCourseSelect").val(),
                    groupId: $("#homePageMenu .displayArea .addGroupSelect").val(),
                    email: inputs[0].value,
                    firstname: inputs[1].value,
                    lastname: inputs[2].value
                }
            })
            .done(function (msg) {
                alert(msg.message);
            });
        })
    });
})();