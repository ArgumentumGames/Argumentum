/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {

config.toolbar = 'NBrightMod';
 
config.toolbar_NBrightMod =
[
	{ name: 'document', items: ['Source'] },
	{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
	{ name: 'basicstyles', items : [ 'Bold','Italic','Strike' ] },
	{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'HorizontalRule', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
//    { name: 'links', items: ['Link', 'Unlink','Dnnpagelinks','Anchor' ] },
    { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	{ name: 'styles', items : [ 'Format' ] },
];

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;h4;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

	config.allowedContent = true;

};

