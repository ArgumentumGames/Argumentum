

$(document).ready(function () {

    $('#OS_PayBox_cmdSave').unbind("click");
    $('#OS_PayBox_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        // lower case cmd must match ajax provider ref.
        nbxget('os_paybox_savesettings', '.OS_PayBoxdata', '.OS_PayBoxreturnmsg');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        // lower case cmd must match ajax provider ref.
        nbxget('os_paybox_selectlang', '.OS_PayBoxdata', '.OS_PayBoxdata');
    });


    $(document).on("nbxgetcompleted", OS_PayBox_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function OS_PayBox_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();

        if (e.cmd == 'os_paybox_selectlang') {
                        
        }

    };

});

