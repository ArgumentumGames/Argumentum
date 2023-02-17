
$(document).ready(function () {

    // get list of records via ajax:  NBrightRazorTemplate_nbxget({command}, {div of data passed to server}, {return html to this div} )
    nbxget('os_reports_getdata', '#selectparams', '#editdata');

    $('.actionbuttonwrapper #cmdsave').unbind('click');
    $('.actionbuttonwrapper #cmdsave').click(function () {
        nbxget('os_reports_savedata', '#editdata');
    });

    $('.actionbuttonwrapper #cmdreturn').unbind('click');
    $('.actionbuttonwrapper #cmdreturn').click(function () {
        $('#selecteditemid').val(''); // clear sleecteditemid.        
        nbxget('os_reports_getdata', '#selectparams', '#editdata');
    });

    $('.actionbuttonwrapper #cmddelete').unbind('click');
    $('.actionbuttonwrapper #cmddelete').click(function () {
        if (confirm($('#deletemsg').val())) {
            nbxget('os_reports_deleterecord', '#editdata');
        }
    });

    $('#addnew').unbind('click');
    $('#addnew').click(function () {
        $('.processing').show();
        $('#newitem').val('new');
        $('#selecteditemid').val('');
        nbxget('os_reports_addnew', '#selectparams', '#editdata');
    });

    $('.selecteditlanguage').unbind('click');
    $('.selecteditlanguage').click(function () {
        $('#nextlang').val($(this).attr('lang')); // alter lang after, so we get correct data record
        nbxget('os_reports_selectlang', '#editdata'); // do ajax call to save current edit form
    });

});

$(document).on("nbxgetcompleted", nbxgetCompleted); // assign a completed event for the ajax calls


function nbxgetCompleted(e) {

    $('#rundisplay').val("");


    if (e.cmd === 'os_reports_rundisplay') {
        $('#runreport').unbind();
        $('#runreport').click
            (function () {
                $('.processing').show();
                nbxget('os_reports_runreport', '#selectparams', '#editdata');
            });
    }

    if (e.cmd === 'os_reports_addnew') {
        $('#newitem').val(''); // clear item so if new was just created we don;t create another record
        $('#selecteditemid').val($('#itemid').val()); // move the itemid into the selecteditemid, so page knows what itemid is being edited
        OS_Reports_DetailButtons();
        $('.processing').hide(); 
    }

    if (e.cmd === 'os_reports_deleterecord') {
        $('#selecteditemid').val(''); // clear selecteditemid, it now doesn;t exists.
        nbxget('os_reports_getdata', '#selectparams', '#editdata');// relist after delete
    }

    if (e.cmd === 'os_reports_savedata') {
        $('#selecteditemid').val(''); // clear sleecteditemid.        
        nbxget('os_reports_getdata', '#selectparams', '#editdata');// relist after save
    }

    if (e.cmd === 'os_reports_selectlang') {
        $('#editlang').val($('#nextlang').val()); // alter lang after, so we get correct data record
        nbxget('os_reports_getdata', '#selectparams', '#editdata'); // do ajax call to get edit form
    }

    if (e.cmd === 'os_reports_getdata') {
        $('.processing').hide();

        $('#OS_Reports_cmdSearch').unbind("click");
        $('#OS_Reports_cmdSearch').click(function () {
            $('.processing').show();
            $('#pagenumber').val('1');
            $('#searchtext').val($('#OrderAdmin_searchtext').val());
            nbxget('os_reports_getdata', '#selectparams', '#editdata');
        });

        $('#OS_Reports_cmdReset').unbind("click");
        $('#OS_Reports_cmdReset').click(function () {
            $('.processing').show();
            $('#pagenumber').val('1');
            $('#searchtext').val('');
            nbxget('os_reports_getdata', '#selectparams', '#editdata');
        });

        $('.cmdPg').unbind("click");
        $('.cmdPg').click(function () {
            $('.processing').show();
            $('#pagenumber').val($(this).attr('pagenumber'));
            nbxget('os_reports_getdata', '#selectparams', '#editdata');
        });

        $('.rundisplay').unbind();
        $('.rundisplay').click(function () {
            OS_Reports_RunButtons();
            $('.processing').show();
            $('#selecteditemid').val($(this).attr("itemid")); // assign the selected itemid, so the server knows what item is being edited
            $('#rundisplay').val('true');
            nbxget('os_reports_rundisplay', '#selectparams', '#editdata'); // do ajax call to get edit form
        });



    }

    // check if we are displaying a list or the detail and do processing.
    if (($('#selecteditemid').val() !== '') || (e.cmd === 'os_reports_addnew')) {
        // PROCESS DETAIL
        $('.processing').hide();
    } else {
        //PROCESS LIST
        OS_Reports_ListButtons();
        $('.edititem').unbind('click');
        $('.edititem').click(function () {
            OS_Reports_DetailButtons();
            $('.processing').show();
            $('#selecteditemid').val($(this).attr("itemid")); // assign the sleected itemid, so the server knows what item is being edited
            nbxget('os_reports_getdata', '#selectparams', '#editdata'); // do ajax call to get edit form
        });
        $('.processing').hide(); 
    }



}

function OS_Reports_DetailButtons() {
    $('#cmdsave').show();
    $('#cmddelete').show();
    $('#cmdreturn').show();
    $('#addnew').hide();
    $('input[datatype="date"]').datepicker(); // assign datepicker to any ajax loaded fields
}

function OS_Reports_ListButtons() {
    $('#cmdsave').hide();
    $('#cmddelete').hide();
    $('#cmdreturn').hide();
    $('#addnew').show();
}

function OS_Reports_RunButtons() {
    $('#cmdsave').hide();
    $('#cmddelete').hide();
    $('#cmdreturn').show();
    $('#addnew').hide();
}

