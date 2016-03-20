(function () {
    $("#homePageMenu .menuArea .roleControlePage").click(function (event) {
        event.preventDefault();
        var start = 0;
        var updateUserRoleArea = function (msg) {
            var area = $('#homePageMenu .displayArea .userRolesArea');
            area.html('');
            for (var i = 0; i != msg.users.length; i++) {
                var item = msg.users[i];
                var workArea = $('<span>');
                workArea.appendTo(area);
                workArea.append('<input type="hidden" value="' + item.Email + '"/>');
                workArea.append('<span>' + item.FirstName + ' ' + item.LastName + ' ' + item.Speciality + ' ' + item.Course + ' ' + item.Group + '</span>');
                var sel = $('<select>');
                sel.appendTo(workArea);
                for (var j = 0; j != msg.roles.length; j++) {
                    var role = msg.roles[j];
                    if (role.Name == item.Role) {
                        sel.append('<option selected value="' + role.Name + '">' + role.Name + '</option>');
                    }
                    else {
                        sel.append('<option value="' + role.Name + '">' + role.Name + '</option>');
                    }
                }
                workArea.append('<input type="submit" value="Change"/><br/>');
            }
            area.find('input[value="Change"]').click(function (event) {
                $.ajax({
                    method: "POST",
                    url: "/Home/ChangeRole",
                    data: { email: $(event).prevAll('input[type="hidden"]').val(), newRole: $(event).prev().find('option[selected]').val() }
                })
                .done(function (msg) {
                    alert(msg.message);
                });
            });
        };
        var NextResults = function () {
            $.ajax({
                method: "POST",
                url: "/Home/TenUsersRoles",
                data: { start: start }
            })
            .done(function (msg) {
                updateUserRoleArea(msg);
            });
        };

        $("#homePageMenu .displayArea").html('<input type="text" placeholder="search"/><input class="searchButton" type="submit" value="Search"/><div class="userRolesArea"></div><input class="nextButton" type="submit" value="Next"/>');

        $("#homePageMenu .displayArea .nextButton").click(function () {
            NextResults();
            start += 10;
        });

        var searchButton = $('#homePageMenu .displayArea .searchButton');
        searchButton.click(function () {
            $.ajax({
                method: "POST",
                url: "/Home/SearchUser",
                data: { search: searchButton.prev().val() }
            })
            .done(function (msg) {
                updateUserRoleArea(msg);
            });
        });

        NextResults();
    });
})();