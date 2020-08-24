// NBright JQuery plugin to generate ajax post of input fields - v1.0.0
(function ($) {

    $.fn.genxmlajax = function (selectordiv) {

        return getgenxml(selectordiv);

    };

    $.fn.popupformlist = function (selformdiv, sellistdiv, selpopupbutton, ajaxbutton, cmdupdate, width) {

        $(selformdiv).dialog({
            autoOpen: false,
            width: width,
            buttons: {
                "Ok": function () {

                    $(selformdiv).trigger('beforeupdate');

                    // get all the inputs into an array.
                    var values = getgenxml(selformdiv);
                    var request = $.ajax({ type: "POST",
                        url: cmdupdate,
                        data: { inputxml: escape(values) }
                    });

                    $(this).dialog("close");

                    request.done(function (data) {
                        $(selformdiv).trigger('afterupdate');
                        displayList(data, ajaxbutton, sellistdiv);
                    });

                    request.fail(function (jqXHR, textStatus) {
                        alert("Request failed: " + textStatus);
                    });

                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });

        // Dialog Link	
        $(selpopupbutton).click(function () {
            $(selformdiv).dialog('open');
            return false;
        });

    };

    $.fn.initlist = function (sellistdiv, ajaxbutton, cmdget) {
        $.ajaxSetup({ cache: false });
        $.get(cmdget, function (data) {
            displayList(data, ajaxbutton, sellistdiv);
        });

    };


    function displayList(data, ajaxbutton, sellistdiv) {
        $(sellistdiv).html(data);
        $(ajaxbutton).click(function () {
            var cmd = $(this).attr("cmd");
            $.get(cmd, function (data) {
                displayList(data, ajaxbutton, sellistdiv);
            });
        });
    };

    function getgenxml(selectordiv) {

        // get all the inputs into an array.
        var values = "<root>";

        var $inputs = $(selectordiv + ' :input');
        $inputs.each(function () {
            var strID = $(this).attr("id");
            if ($(this).attr("type") == 'radio') {
                values += '<f t="rb"  id="' + strID + '" val="' + $(this).attr("value") + '"><![CDATA[' + $(this).attr("checked") + ']]></f>';
            }
            else if ($(this).attr("type") == 'checkbox') {
                var datavalue = $(this).parent().attr("datavalue");
                values += '<f t="cb"  id="' + strID + '" for="' + $('label[for=' + strID + ']').text() + '" val="' + datavalue + '">' + $(this).attr("checked") + '</f>';
            }
            else if ($(this).attr("type") == 'text') {
                values += '<f t="txt"  id="' + strID + '" dt="' + $(this).attr("datatype") + '"><![CDATA[' + $(this).val() + ']]></f>';
            }
            else {
                values += '<f t="' + $(this).attr("type") + '"  id="' + strID + '"><![CDATA[' + $(this).val() + ']]></f>';
            }
        });
        values += '</root>'

        return values;

    };

})(jQuery);

