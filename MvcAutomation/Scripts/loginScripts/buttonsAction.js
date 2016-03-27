(function () {
    var diagram = new Diagram('.col2', [[50, 100], [200, 50], [200, 150], [150, 100], [250, 100]], [[0, 1], [0, 2], [1, 3], [1, 4], [2, 3], [2, 4], [3, 0]]);
    var setStageOneHide = function (afterFunc) {
        $('#stageOneLog').hide({ queue: true, complete: afterFunc });
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
    $('#stageOneSubmit').click(function (event) {
        event.preventDefault();
        var spinner = new Spinner('#stageOneSubmit');
        spinner.show();
        var error = $('#errorMsg');
        error.hide();
        $.ajax({
            method: "POST",
            url: "/Home/CheckEmail",
            data: { email: $('#emailText').val() }
        })
        .done(function (msg) {
            spinner.hide();
            if (msg.isRegistered == 'true' && msg.isInDb == 'true') {
                setStageOneHide(setLogin);
                diagram.lineEquation(0, 1);
                diagram.goingToCircle();
            }
            else if (msg.isInDb == 'true') {
                setStageOneHide(setSignUp);
                diagram.lineEquation(0, 2);
                diagram.goingToCircle();
            }
            else {
                error.html("Такого пользователя не существует.");
                error.show();
                diagram.lineEquation(0, 3);
                diagram.goingToCircle();
            }
        });
    });
})();