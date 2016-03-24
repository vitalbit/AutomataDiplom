﻿$(document).ready(function () {
    $.ajax({
        method: "POST",
        url: "/TestPassing/CurrentFile",
        data: { testId: $('#testArea input[type="hidden"]').val() }
    })
    .done(function (res) {
        var msg = JSON.parse(res);
        $('#targetArea').append('Lambda: ' + msg.Lambda);
        $('#targetArea').append('<br/>Polish Expression:<br/><input type="text" placeholder="Your polish"/><br/><input type="submit" value="OK"/><br/><img/>');
        $('#targetArea input[type="submit"]').click(function (event) {
            event.preventDefault();
            $('#targetArea img').attr('src', '/TestPassing/ImageForPolish?polish=' + $('#targetArea input[type="text"]').val());
        });
    });
});