

$(document).ready(function () {

    $('#OSStripe_cmdSave').unbind("click");
    $('#OSStripe_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        // lower case cmd must match ajax provider ref.
        nbxget('osstripe_savesettings', '.OSStripedata', '.OSStripereturnmsg');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        // lower case cmd must match ajax provider ref.
        nbxget('osstripe_selectlang', '.OSStripedata', '.OSStripedata');
    });


    $(document).on("nbxgetcompleted", OSStripe_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function OSStripe_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();

        if (e.cmd === 'osstripe_selectlang') {
            window.location.reload(true);               
        }

    };

});

