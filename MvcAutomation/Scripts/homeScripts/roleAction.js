(function () {
    $("#homePageMenu .menuArea .roleControlePage").click(function (event) {
        event.preventDefault();
        
        var updateUserRoleArea = function (msg, areaSelector) {
            var area = $(areaSelector);
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
        var search = new SearchNavigation("#homePageMenu .displayArea", "/Home/TenUsersRoles", "/Home/SearchUser", updateUserRoleArea);
    });
})();