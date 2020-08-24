// use {id} to replace the control id in the script.

 var editorvar{id} = '';

 $(document).ready(function () {

     CKEDITOR.plugins.addExternal('dnnpagelinks', '/DesktopModules/NBright/NBrightData/ckeditor/plugins/dnnpagelinks/', 'plugin.js' );

     editorvar{id} = CKEDITOR.replace('editor{id}', {
         customConfig: '/DesktopModules/NBright/NBrightData/ckeditor/nbrightconfig.js'
     });

 editorvar{id}.on('change', function (event) {  
	var value = editorvar{id}.getData();
	$('#{id}').val(value); }); }
);
 