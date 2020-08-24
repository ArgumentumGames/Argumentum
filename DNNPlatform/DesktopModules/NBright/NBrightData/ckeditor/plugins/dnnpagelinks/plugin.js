/**
 * Copyright (c) 2014-2018, CKSource - Frederico Knabben. All rights reserved.
 * Licensed under the terms of the MIT License (see LICENSE.md).
 *
 * Simple CKEditor Widget (Part 2).
 *
 * Created out of the CKEditor Widget SDK:
 * http://docs.ckeditor.com/ckeditor4/docs/#!/guide/widget_sdk_tutorial_2
 */

// Register the plugin within the editor.
CKEDITOR.plugins.add( 'dnnpagelinks', {
	// This plugin requires the Widgets System defined in the 'widget' plugin.

	// Register the icon used for the toolbar button. It must be the same
	// as the name of the widget.
	icons: 'dnnpagelinks',

	// The plugin initialization logic goes inside this method.
	init: function( editor ) {
		// Register the editing dialog.
		CKEDITOR.dialog.add( 'dnnpagelinks', this.path + 'dialogs/dnnpagelinks.js' );

		// Register the dnnpagelinks widget.
		editor.widgets.add( 'dnnpagelinks', {
			template:
                '<div></div>',
            button: 'Pages',
            dialog: 'dnnpagelinks'
		} );
    }

} );
