(function () {
    //var diagram = new Diagram('.col2', [[50, 100], [200, 50], [200, 150], [150, 100], [250, 100]], [[0, 1], [0, 2], [1, 3], [1, 4], [2, 3], [2, 4], [3, 0]]);
    var validateEmail = function (email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    var error = $('#errorMsg');
    var textTemplate = Automata.HtmlLoader.Load('/Scripts/loginScripts/CheckEmailPage.html');
    
    $('#loginArea').html(textTemplate);
    $('#emailText').keyup(function () {
        if (validateEmail($('#emailText').val())) {
            $('#enterButton').prop('disabled', false);
            $('#signUpButton').prop('disabled', false);
        }
        else {
            $('#enterButton').prop('disabled', true);
            $('#signUpButton').prop('disabled', true);
        }
    });

    $('#enterButton').click(function (event) {
        event.preventDefault();

        var spinner = new Spinner('#enterButton');
        spinner.show();
        error.hide();
        $.ajax({
            method: "POST",
            url: "/Home/CheckEmail",
            data: { email: $('#emailText').val() }
        })
        .done(function (msg) {
            spinner.hide();
            if (msg.isInDb == 'true') {
                setStageOneHide(setLogin);
                //diagram.lineEquation(0, 1);
                //diagram.goingToCircle();
            } else {
                error.html("Не найден пользователь с таким почтовым адресом");
                error.show();
                //diagram.lineEquation(0, 3);
                //diagram.goingToCircle();
            }
        });
    });

    var ShowSignUpScreen = function () {
        var signupTemplate = Automata.HtmlLoader.Load('/Scripts/loginScripts/SignUpPage.html');
        var email = $('#emailText').val();
        $('#loginArea').html(signupTemplate);
        $('#signupEmail').val(email);

        $.ajax({
            method: "GET",
            url: "/StructureOrganize/GetUniversityInfo"
        })
        .done(function (msg) {
            var sel = $('select');
            for (var i = 0; i != msg.universityInfo.length; i++) {
                var item = msg.universityInfo[i];
                sel.append('<option value="' + item.Id + '">' + item.Info + '</option>');
            }
        });

        $('#signUpSubmit').click(function (event) {
            event.preventDefault();
            var spinner2 = new Spinner('#signUpSubmit');
            spinner2.show();
            $.ajax({
                method: "POST",
                url: "/Home/SignUp",
                data: { email: $('#signupEmail').val(), firstName: $('#userName').val(), lastName: $('#userLastName').val(), universityInfo: $('option[selected]').val() }
            })
            .done(function (msg) {
                spinner2.hide();
                alert(msg.message);
            });
        });
    };

    $('#signUpButton').click(function (event) {
        event.preventDefault();
        error.hide();
        $.ajax({
            method: "POST",
            url: "/Home/CheckEmail",
            data: { email: $('#emailText').val() }
        })
        .done(function (msg) {
            if (msg.isInDb == 'true') {
                error.html("Пользователь с таким почтовым адресом уже зарегистрирован");
                error.show();
            }
            else {
                ShowSignUpScreen();
            }
        });
    });

    var setStageOneHide = function (afterFunc) {
        $('#loginArea').hide({ queue: true, complete: afterFunc });
    }
    var setLoginHide = function (afterFunc) {
        $('#loginFields').hide({ queue: true, complete: afterFunc });
    }
    var setSignUpHide = function (afterFunc) {
        $('#signupFields').hide({ queue: true, complete: afterFunc });
    }
    var setStageOne = function () {
        $('#stagOneLog').show(800);
    }
    var setLogin = function () {
        var email = $('#emailText').val();
        $('#loginFields #Email').val(email);
        $('#loginFields').show(800);
    }
    var setSignUp = function () {
        var email = $('#emailText').val();
        $('#signupFields #Email').val(email);
        $('#signupFields').show(800);
    }
})();