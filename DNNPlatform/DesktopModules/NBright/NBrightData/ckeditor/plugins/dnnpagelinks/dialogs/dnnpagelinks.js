/**
 * Copyright (c) 2014-2018, CKSource - Frederico Knabben. All rights reserved.
 * Licensed under the terms of the MIT License (see LICENSE.md).
 */

// Note: This automatic widget to dialog window binding (the fact that every field is set up from the widget
// and is committed to the widget) is only possible when the dialog is opened by the Widgets System
// (i.e. the widgetDef.dialog property is set).
// When you are opening the dialog window by yourself, you need to take care of this by yourself too.

CKEDITOR.dialog.add( 'dnnpagelinks', function( editor ) {
	return {
		title: 'Edit Simple Box',
		minWidth: 200,
		minHeight: 100,
		contents: [
			{
				id: 'info',
				elements: [
					{
                        id: 'linkname',
                        type: 'text',
                        label: '',
                        width: '150px',
                        setup: function (widget) {
                            this.setValue(widget.data.width);
                        },
                        commit: function (widget) {
                            widget.setData('linkname', this.getValue());
                        }
                    },
                    {
                        type: 'select',
                        id: 'dnnpage',
                        width: '150px',
                        label: 'Page',
                        items: [],
                        setup: function (element) {
                            var element_id = '#' + this.getInputElement().$.id;
                            $.ajax({
                                type: 'POST',
                                url: '/DesktopModules/NBright/NBrightData/ApiConnector.ashx?cmd=dnnpages',
                                data: '{"cpID":' + window.parent.$("#cpID").val() + '}',
                                contentType: 'text/plain; charset=utf-8',
                                dataType: 'text',
                                async: false,
                                success: function (data) {
                                    if (window.DOMParser) {
                                        parser = new DOMParser();
                                        xmlDoc = parser.parseFromString(data, "text/xml");
                                    }
                                    else // Internet Explorer
                                    {
                                        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                                        xmlDoc.async = false;
                                        xmlDoc.loadXML(data);
                                    }

                                    if ($(element_id).get(0).options.length == 0) {
                                        var list = xmlDoc.getElementsByTagName("page");
                                        for (var i = 0; i < list.length; i++) {
                                            //alert(list[i].getAttribute("url"));
                                            $(element_id).get(0).options[$(element_id).get(0).options.length] = new Option(list[i].firstChild.nodeValue, list[i].getAttribute("url"));
                                        }
                                    }


                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert(xhr.status);
                                    alert(thrownError);
                                }
                            });
                        }

                    }

				]
			}
        ],
        onOk: function () {
            var dialog = this;
            var linkname = dialog.getValueOf('info','linkname');
            var dnnpage = dialog.getValueOf('info', 'dnnpage');
            editor.insertHtml('<a href="' + dnnpage + '">' + linkname + '</a>');
        }
	};
} );