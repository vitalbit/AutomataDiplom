(function () {
    var setStageOneHide = function (afterFunc) {
        $('#stageOneLog').hide(0, afterFunc);
    }
    var setLoginHide = function (afterFunc) {
        $('#loginFields').hide(0, afterFunc);
    }
    var setSignUpHide = function (afterFunc) {
        $('#signupFields').hide(0, afterFunc);
    }
    var setStageOne = function () {
        $('#stagOneLog').show(1000);
    }
    var setLogin = function () {
        $('#loginFields').show(1000);
    }
    var setSignUp = function () {
        $('#signupFields').show(1000);
    }
    $('#stageOneSubmit').click(function () {
        var error = $('#errorMsg');
        error.hide(0);
        $.ajax({
            method: "POST",
            url: "/Home/CheckEmail",
            data: { email: $('#emailText').val() }
        })
        .done(function (msg) {
            if (msg.isRegistered == 'true' && msg.isInDb == 'true') {
                setStageOneHide(setLogin);
            }
            else if (msg.isInDb == 'true') {
                setStageOneHide(setSignUp);
            }
            else {
                error.html("No such user!");
                error.show();
            }
        });
    });
})();