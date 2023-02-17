

$(document).ready(function () {

    $('#OSPayPal_cmdSave').unbind("click");
    $('#OSPayPal_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        // lower case cmd must match ajax provider ref.
        nbxget('ospaypal_savesettings', '.OSPayPaldata', '.OSPayPalreturnmsg');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        // lower case cmd must match ajax provider ref.
        nbxget('ospaypal_selectlang', '.OSPayPaldata', '.OSPayPaldata');
    });


    $(document).on("nbxgetcompleted", OSPayPal_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function OSPayPal_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();

        if (e.cmd == 'ospaypal_selectlang') {
                        
        }

    };

});

