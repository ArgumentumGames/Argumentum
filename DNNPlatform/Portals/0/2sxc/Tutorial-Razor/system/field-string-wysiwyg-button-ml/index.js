/*
  This examples shows a custom WYSIWYG JS WebComponent with additional language and button
*/

// always use an IFFE to ensure you don't put variables in the window scope
(() => {
  const tagName = 'field-string-wysiwyg-button-ml';
  const builtInWysiwyg = '[System:Path]/system/field-string-wysiwyg/index.js';

  // Button labels, mouse-over-tooltips, etc.
  const en = { "TestButton.Tooltip": "Tooltip Button EN!" };
  const de = { "TestButton.Tooltip": "Tooltip Knopf DE!" };

  /** Our custom Wysiwyg with an additional button having labels in 2 languages */
  class StringWysiwygCustom extends HTMLElement {

    /* connectedCallback() is the standard callback  when the component has been attached */
    connectedCallback() {
      // We need to ensure that the standard WYSIWYG is also loaded
      this.connector.loadScript('tinymce', builtInWysiwyg, (x) => { this.initWysiwygCallback() })
    }

    initWysiwygCallback() {
      // 1. Create a built-in field-string-wysiwyg control
      const wysiwyg = document.createElement('field-string-wysiwyg');
      // 2. tell it if it should start in preview or edit
      wysiwyg.mode = 'edit'; // can be 'preview' or 'edit'
      // 3. attach connector
      wysiwyg.connector = this.connector;
      // 4. also attach reconfigure object which can change the TinyMCE as it's initialized
      wysiwyg.reconfigure = new WysiwygReconfigurator();
      // 5. Append it to the DOM. Do this last, as it will trigger connectedCallback() in the wysiwyg
      this.appendChild(wysiwyg);
    }
  }

  /** The object which helps reconfigure what the editor will do */
  class WysiwygReconfigurator {
    addTranslations(editorManager, language) {
      console.log("add translations to tinyMCE", editorManager, language);
      editorManager.addI18n(language, language == 'de' ? de : en);
    }

    configureOptions(options) {
      console.log('will try to modify tinyMCE options', options);
      options.toolbar += " | testButton";
      return options;
    }

    configureAddOns(addOnSettings) {
      console.log('configure add-on settings for stuff added by the 2sxc form');
      addOnSettings.imgSizes = [25, 10];
      return addOnSettings;
    }

    editorOnInit(editor) {
      console.log('editor init, will add button to tinyMCE', editor);
      editor.ui.registry.addButton('testButton', {
        icon: 'close',
        tooltip: 'TestButton.Tooltip',
        onAction: (_) => { alert('test-button used!') },
      });
    }

    disableDnn = true;
  }

  // Register this web component - if it hasn't been registered yet
  if (!customElements.get(tagName)) customElements.define(tagName, StringWysiwygCustom);
})();