(function () {
    $('#homePageMenu .menuArea .addStudentsPage').click(function (event) {
        event.preventDefault();

        var textTemplate = Automata.HtmlLoader.Load('/Scripts/homeScripts/StructureOrganizePage/StructureOrganizeTemplate.html');

        $("#homePageMenu .displayArea").html(textTemplate);

        var updateGroups = function () {
            $.ajax({
                method: "GET",
                url: "/StructureOrganize/GetUniversityInfo"
            })
            .done(function (msg) {
                var sel = $('select');
                sel.html('');
                for (var i = 0; i != msg.universityInfo.length; i++) {
                    var item = msg.universityInfo[i];
                    sel.append('<option value="' + item.Id + '">' + item.Info + '</option>');
                }
            });
        }

        $('#add_Info').click(function () {
            var spinner = new Spinner('#add_Info');
            spinner.show();
            var inputs = $("#homePageMenu .displayArea").find('input[type="text"]');
            $.ajax({
                method: "POST",
                url: "/StructureOrganize/AddUniversityInfo",
                data: {
                    University: inputs[0].value,
                    Faculty: inputs[1].value,
                    Course: inputs[2].value,
                    Group: inputs[3].value,
                    Speciality: inputs[4].value,
                    AdditionalInfo: $('textarea').val()
                }
            })
            .done(function (msg) {
                updateGroups();
                spinner.hide();
            });
        });

        updateGroups();
    });
})();