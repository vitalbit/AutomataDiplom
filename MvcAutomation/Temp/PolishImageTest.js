﻿$(document).ready(function () {
    var testId = $('#testArea > input[type="hidden"]')[0].val();
    var dllFilePath = $('#testArea > input[type="hidden"]')[1].val();
    var resolveDllType = $('#testArea > input[type="hidden"]')[2].val();

    $.ajax({
        method: "POST",
        url: "/TestPassing/CurrentFile",
        data: { testId: testId }
    })
    .done(function (msg) {
        $('#targetArea').append('Regex: ' + msg.Regex);
        $('#targetArea').append('<br/>Polish Expression:<br/><input type="text" placeholder="Your polish"/><br/><input type="submit" value="OK"/><br/><img/>');
        $('#targetArea input[type="submit"]').click(function (event) {
            event.preventDefault();
            $('#targetArea img').attr('src', '/TestPassing/Image?input=' + $('#targetArea input[type="text"]').val() + '&resolveDll=' + dllFilePath + '&resolveType=' + resolveDllType);
        });
    });
});